﻿using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsLifecycle.Disable, "PnPResponsiveUI")]
    [CmdletHelp("Deactivate the PnP Response UI add-on",
        "Disables the PnP Responsive UI implementation on a classic SharePoint Site", Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = "PS:> Disable-PnPResponsiveUI",
        Remarks = @"If enabled previously, this will remove the PnP Responsive UI from a site.",
        SortOrder = 1)]
    [Obsolete("The PnP responsive UI (classic view) has been deprecated.")]
    public class DisableResponsiveUI : PnPWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var site = ClientContext.Site;
            site.DisableResponsiveUI();
        }
    }
}
