using Microsoft.SharePoint.Client;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Get, "PnPHubSite")]
    public class GetHubSite : PnPSharePointOnlineAdminCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        public HubSitePipeBind Identity { get; set; }

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                try
                {
                    var hubSiteProperties = Identity.GetHubSite(Tenant);
                    AdminContext.Load(hubSiteProperties);
                    AdminContext.ExecuteQueryRetry();
                    WriteObject(hubSiteProperties);
                }
                catch (ServerObjectNullReferenceException)
                {
                }
            }
            else
            {
                var hubSitesProperties = base.Tenant.GetHubSitesProperties();
                AdminContext.Load(hubSitesProperties);
                AdminContext.ExecuteQueryRetry();
                WriteObject(hubSitesProperties, true);
            }
        }
    }
}