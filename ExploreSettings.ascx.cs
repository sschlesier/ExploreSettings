using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;

namespace ExploreSettings
{
    public partial class ExploreSettings : PortalModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ServicesFramework.Instance.RequestAjaxScriptSupport();
            ServicesFramework.Instance.RequestAjaxAntiForgerySupport();
            
            jQuery.RequestRegistration();
        }
    }
}