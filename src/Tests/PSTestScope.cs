using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Runtime.InteropServices;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Tests
{
    public class PSTestScope : IDisposable
    {
        private Runspace _runSpace;

        private Configuration configuration;

        public PSTestScope(bool connect = true)
            : this(null, connect)
        {
        }

        public PSTestScope(string siteUrl, bool connect = true)
        {
            configuration = new Configuration();

            var iss = InitialSessionState.CreateDefault();
            RegisterPnPCmdlets(iss);

            _runSpace = RunspaceFactory.CreateRunspace(iss);
            _runSpace.Open();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var executionPolicyPipeline = _runSpace.CreatePipeline();
                var executionPolicyCmd = new Command("Set-ExecutionPolicy");
                executionPolicyCmd.Parameters.Add("ExecutionPolicy", "Unrestricted");
                executionPolicyCmd.Parameters.Add("Scope", "Process");
                executionPolicyPipeline.Commands.Add(executionPolicyCmd);
                executionPolicyPipeline.Invoke();
            }

            if (connect)
            {
                var pipeLine = _runSpace.CreatePipeline();
                var cmd = new Command("Connect-PnPOnline");
                cmd.Parameters.Add("Url", string.IsNullOrEmpty(siteUrl) ? configuration.SiteUrl : siteUrl);

                if (configuration.UseInteractive)
                {
                    cmd.Parameters.Add("ClientId", configuration.ClientId);
                    cmd.Parameters.Add("Interactive", true);
                }
                else
                {
                    cmd.Parameters.Add("Credentials", configuration.Credentials);
                }

                pipeLine.Commands.Add(cmd);
                pipeLine.Invoke();
            }
        }

        private static void RegisterPnPCmdlets(InitialSessionState iss)
        {
            // The scaffolded tests only registered Connect-PnPOnline by name, so other
            // PnP cmdlets weren't discoverable in the runspace. Auto-register every type
            // in the Commands assembly that carries a [Cmdlet] attribute.
            var commandsAssembly = typeof(ConnectOnline).Assembly;
            foreach (var type in commandsAssembly.GetTypes())
            {
                if (!typeof(Cmdlet).IsAssignableFrom(type) || type.IsAbstract)
                {
                    continue;
                }

                var cmdletAttr = type.GetCustomAttributes(typeof(CmdletAttribute), inherit: false)
                    .Cast<CmdletAttribute>()
                    .FirstOrDefault();
                if (cmdletAttr == null)
                {
                    continue;
                }

                var name = $"{cmdletAttr.VerbName}-{cmdletAttr.NounName}";
                iss.Commands.Add(new SessionStateCmdletEntry(name, type, helpFileName: null));
            }
        }

        public Collection<PSObject> ExecuteCommand(string cmdletString)
        {
            return ExecuteCommand(cmdletString, null);
        }

        public Collection<PSObject> ExecuteCommand(string cmdletString, params CommandParameter[] parameters)
        {
            var pipeLine = _runSpace.CreatePipeline();
            Command cmd = new Command(cmdletString);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            pipeLine.Commands.Add(cmd);
            return pipeLine.Invoke();
        }

        public Collection<PSObject> ExecuteScript(string script)
        {
            var pipeLine = _runSpace.CreatePipeline();

            pipeLine.Commands.AddScript(script);

            return pipeLine.Invoke();
        }

        public void Dispose()
        {
            if (_runSpace != null)
            {
                _runSpace.Dispose();
            }
        }
    }
}
