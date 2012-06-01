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

            //Not needed, included in the call for anti-forgery
//            jQuery.RequestRegistration();
//            ServicesFramework.Instance.RequestAjaxScriptSupport();

            ServicesFramework.Instance.RequestAjaxAntiForgerySupport();
            
        }
    }
}