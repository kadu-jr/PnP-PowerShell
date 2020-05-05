﻿using Newtonsoft.Json;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Model;
using SharePointPnP.PowerShell.Commands.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsData.Initialize, "PnPPowerShellAuthentication")]

    [CmdletAdditionalParameter(ParameterType = typeof(string[]), ParameterName = "Scopes", HelpMessage = "Specify which permissions scopes to request.", ParameterSetName = ParameterSet_NEWCERT)]
    [CmdletAdditionalParameter(ParameterType = typeof(string[]), ParameterName = "Scopes", HelpMessage = "Specify which permissions scopes to request.", ParameterSetName = ParameterSet_EXISTINGCERT)]

    [CmdletExample(
       Code = @"PS:> Initialize-PnPPowerShellAuthentication -ApplicationName TestApp -Tenant yourtenant.onmicrosoft.com -StoreLocation CurrentUser",
       Remarks = "Creates a new Azure AD Application registration, creates a new self signed certificate, and adds it to the local certificate store. It will upload the certificate to the azure app registration and it will request the following permissions: Sites.FullControl.All, Group.ReadWrite.All, User.Read.All",
       SortOrder = 1)]

    public class InitializePowerShellAuthentication : BasePSCmdlet, IDynamicParameters
    {
        private const string ParameterSet_EXISTINGCERT = "Existing Certificate";
        private const string ParameterSet_NEWCERT = "Generate Certificate";

        [Parameter(Mandatory = true, HelpMessage = "The name of the Azure AD Application to create", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string ApplicationName;

        [Parameter(Mandatory = true, HelpMessage = "The identifier of your tenant, e.g. mytenant.onmicrosoft.com", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string Tenant;

        [Parameter(Mandatory = true, HelpMessage = "Password for the certificate being created", ParameterSetName = ParameterSet_EXISTINGCERT)]
        public string CertificatePath;

        [Parameter(Mandatory = false, HelpMessage = "Common Name (e.g. server FQDN or YOUR name). defaults to 'pnp.contoso.com'", Position = 0, ParameterSetName = ParameterSet_NEWCERT)]
        public string CommonName;

        [Parameter(Mandatory = false, HelpMessage = "Country Name (2 letter code)", Position = 1, ParameterSetName = ParameterSet_NEWCERT)]
        public string Country = String.Empty;

        [Parameter(Mandatory = false, HelpMessage = "State or Province Name (full name)", Position = 2, ParameterSetName = ParameterSet_NEWCERT)]
        public string State = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Locality Name (eg, city)", Position = 3, ParameterSetName = ParameterSet_NEWCERT)]
        public string Locality = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Organization Name (eg, company)", Position = 4, ParameterSetName = ParameterSet_NEWCERT)]
        public string Organization = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Organizational Unit Name (eg, section)", Position = 5, ParameterSetName = ParameterSet_NEWCERT)]
        public string OrganizationUnit = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Number of years until expiration (default is 10, max is 30)", Position = 7, ParameterSetName = ParameterSet_NEWCERT)]
        public int ValidYears = 10;

        [Parameter(Mandatory = false, HelpMessage = "Optional certificate password", Position = 8, ParameterSetName = ParameterSet_NEWCERT)]
        [Parameter(Mandatory = false, HelpMessage = "Optional certificate password", Position = 8, ParameterSetName = ParameterSet_EXISTINGCERT)]
        public SecureString CertificatePassword;

        [Parameter(Mandatory = false, HelpMessage = "Folder to create certificate files in (.CER and .PFX)", ParameterSetName = ParameterSet_NEWCERT)]
        public string OutPath;

        [Parameter(Mandatory = false, HelpMessage = "Local Certificate Store to add the certificate to", ParameterSetName = ParameterSet_NEWCERT)]
        public StoreLocation Store;

        protected override void ProcessRecord()
        {
            var record = new PSObject();
            var token = AzureAuthHelper.AuthenticateAsync(Tenant).GetAwaiter().GetResult();

            var cert = new X509Certificate2();
            if (ParameterSetName == ParameterSet_EXISTINGCERT)
            {
                if (ParameterSpecified(nameof(CertificatePassword)))
                {
                    cert.Import(CertificatePath, CertificatePassword, X509KeyStorageFlags.Exportable);
                }
                else
                {
                    cert.Import(CertificatePath);
                }
            }
            else
            {

                // Generate a certificate
                var x500Values = new List<string>();
                if (!MyInvocation.BoundParameters.ContainsKey("CommonName"))
                {
                    CommonName = ApplicationName;
                }
                if (!string.IsNullOrWhiteSpace(CommonName)) x500Values.Add($"CN={CommonName}");
                if (!string.IsNullOrWhiteSpace(Country)) x500Values.Add($"C={Country}");
                if (!string.IsNullOrWhiteSpace(State)) x500Values.Add($"S={State}");
                if (!string.IsNullOrWhiteSpace(Locality)) x500Values.Add($"L={Locality}");
                if (!string.IsNullOrWhiteSpace(Organization)) x500Values.Add($"O={Organization}");
                if (!string.IsNullOrWhiteSpace(OrganizationUnit)) x500Values.Add($"OU={OrganizationUnit}");

                string x500 = string.Join("; ", x500Values);

                if (ValidYears < 1 || ValidYears > 30)
                {
                    ValidYears = 10;
                }
                DateTime validFrom = DateTime.Today;
                DateTime validTo = validFrom.AddYears(ValidYears);

                byte[] certificateBytes = CertificateHelper.CreateSelfSignCertificatePfx(x500, validFrom, validTo, CertificatePassword);
                cert = new X509Certificate2(certificateBytes, CertificatePassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

                if (!string.IsNullOrWhiteSpace(OutPath))
                {
                    if (!Path.IsPathRooted(OutPath))
                    {
                        OutPath = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, OutPath);
                    }
                    if (System.IO.Directory.Exists(OutPath))
                    {
                        var pfxPath = Path.Combine(OutPath, $"{ApplicationName}.pfx");
                        byte[] certPfxData = cert.Export(X509ContentType.Pfx, CertificatePassword);
                        File.WriteAllBytes(pfxPath, certPfxData);
                        record.Properties.Add(new PSVariableProperty(new PSVariable("Pfx file", pfxPath)));
                        var cerPath = Path.Combine(OutPath, $"{ApplicationName}.cer");
                        byte[] certCerData = cert.Export(X509ContentType.Cert);
                        File.WriteAllBytes(cerPath, certCerData);
                        record.Properties.Add(new PSVariableProperty(new PSVariable("Cer file", cerPath)));
                    }
                }
                if (ParameterSpecified(nameof(Store)))
                {
                    using (var store = new X509Store("My",Store))
                    {
                        store.Open(OpenFlags.ReadWrite);
                        store.Add(cert);
                        store.Close();
                    }
                    Host.UI.WriteLine(ConsoleColor.Yellow, Host.UI.RawUI.BackgroundColor, "Certificate added to store");
                }
            }

            var expirationDate = DateTime.Parse(cert.GetExpirationDateString()).ToUniversalTime();
            var startDate = DateTime.Parse(cert.GetEffectiveDateString()).ToUniversalTime();

            if (token != null)
            {
                var permissionScopes = new PermissionScopes();
                var scopes = new List<PermissionScope>();
                if (this.Scopes != null)
                {
                    foreach (var scopeIdentifier in this.Scopes)
                    {
                        scopes.Add(permissionScopes.GetScope(scopeIdentifier));
                    }
                }
                else
                {
                    scopes.Add(permissionScopes.GetScope("SPO.Sites.FullControl.All"));
                    scopes.Add(permissionScopes.GetScope("MSGraph.Group.ReadWrite.All"));
                    scopes.Add(permissionScopes.GetScope("SPO.User.Read.All"));
                    scopes.Add(permissionScopes.GetScope("MSGraph.User.Read.All"));
                }

                var scopesPayload = GetScopesPayload(scopes);

                var payload = new
                {
                    displayName = ApplicationName,
                    signInAudience = "AzureADMyOrg",
                    keyCredentials = new[] {
                        new {
                            customKeyIdentifier = cert.GetCertHashString(),
                            endDateTime = expirationDate,
                            keyId = Guid.NewGuid().ToString(),
                            startDateTime = startDate,
                            type= "AsymmetricX509Cert",
                            usage= "Verify",
                            key = Convert.ToBase64String(cert.GetRawCertData())
                        }
                    },
                    publicClient = new
                    {
                        redirectUris = new[] {
                        "https://login.microsoftonline.com/common/oauth2/nativeclient",
                        }
                    },
                    requiredResourceAccess = scopesPayload
                };
                var postResult = HttpHelper.MakePostRequestForString("https://graph.microsoft.com/beta/applications", payload, "application/json", token);
                var azureApp = JsonConvert.DeserializeObject<AzureApp>(postResult);
                record.Properties.Add(new PSVariableProperty(new PSVariable("AzureAppId", azureApp.AppId)));

                var waitTime = 60;
                this.Host.UI.Write(ConsoleColor.Yellow, this.Host.UI.RawUI.BackgroundColor, $"Waiting {waitTime} seconds to launch consent flow in a browser window. This wait is required to make sure that Azure AD is able to initialize all required artifacts.");
                for (var i = 0; i < waitTime; i++)
                {
                    this.Host.UI.Write(ConsoleColor.Yellow, this.Host.UI.RawUI.BackgroundColor, ".");
                    System.Threading.Thread.Sleep(1000);
                }
                this.Host.UI.WriteLine();
                var consentUrl = $"https://login.microsoftonline.com/{Tenant}/v2.0/adminconsent?client_id={azureApp.AppId}&scope=https://microsoft.sharepoint-df.com/.default";
                record.Properties.Add(new PSVariableProperty(new PSVariable("Certificate Thumbprint", cert.GetCertHashString())));

                AzureAuthHelper.OpenConsentFlow(consentUrl, (message) =>
                {
                    this.Host.UI.WriteLine(ConsoleColor.Red, this.Host.UI.RawUI.BackgroundColor, message);
                });
                WriteObject(record);
            }


        }

        private static object GetScopesPayload(List<PermissionScope> scopes)
        {
            var resourcePermissions = new List<AppResource>();
            var distinctResources = scopes.GroupBy(s => s.resourceAppId).Select(r => r.First()).ToList();
            foreach (var distinctResource in distinctResources)
            {
                var id = distinctResource.resourceAppId;
                var appResource = new AppResource() { Id = id };
                appResource.ResourceAccess.AddRange(scopes.Where(s => s.resourceAppId == id).ToList());
                resourcePermissions.Add(appResource);
            }
            return resourcePermissions;
        }

        protected IEnumerable<string> Scopes
        {
            get
            {
                if (ParameterSpecified(nameof(Scopes)) && MyInvocation.BoundParameters["Scopes"] != null)
                {
                    return MyInvocation.BoundParameters["Scopes"] as string[];
                }
                else
                {
                    return null;
                }
            }
        }

        public object GetDynamicParameters()
        {
            var classAttribute = this.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is PropertyLoadingAttribute);
            const string parameterName = "Scopes";

            var parameterDictionary = new RuntimeDefinedParameterDictionary();
            var attributeCollection = new System.Collections.ObjectModel.Collection<Attribute>();

            var parameterAttribute = new ParameterAttribute
            {
                ValueFromPipeline = false,
                ValueFromPipelineByPropertyName = false,
                Mandatory = false
            };

            attributeCollection.Add(parameterAttribute);

            var identifiers = new PermissionScopes().GetIdentifiers();

            var validateSetAttribute = new ValidateSetAttribute(identifiers);
            attributeCollection.Add(validateSetAttribute);

            var runtimeParameter = new RuntimeDefinedParameter(parameterName, typeof(string[]), attributeCollection);

            parameterDictionary.Add(parameterName, runtimeParameter);

            return parameterDictionary;
        }
    }
}
