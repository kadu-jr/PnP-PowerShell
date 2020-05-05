﻿#if !ONPREMISES
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Remove, "PnPKnowledgeHubSite")]
    [CmdletHelp("Removes the Knowledge Hub Site setting for your tenant",
     SupportedPlatform = CmdletSupportedPlatform.Online,
     Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
     Code = @"PS:> Remove-PnPKnowledgeHubSite",
     Remarks = @"Removes the Knowledge Hub Site setting for your tenant", SortOrder = 1)]
    public class RemoveKnowledgeHubSite : PnPAdminCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            Tenant.RemoveKnowledgeHubSite();
            Tenant.Context.ExecuteQueryRetry();
        }
    }
}
#endif