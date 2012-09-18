// // DotNetNuke® - http://www.dotnetnuke.com
// // Copyright (c) 2002-2012
// // by DotNetNuke Corporation
// // 
// // Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// // documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// // the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// // to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// // 
// // The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// // of the Software.
// // 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// // TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// // THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// // CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// // DEALINGS IN THE SOFTWARE.

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using ExploreSettings.Testables;

namespace ExploreSettings
{
    /// <summary>
    /// Services Framework controller for accessing Settings
    /// <remarks>
    /// Path is DesktopModules/ExploreSettings/API/Settings/{action}
    /// </remarks>
    /// </summary>
    [SupportedModules("ExploreSettings")]
    public class SettingsController : DnnApiController
    {
        [HttpGet]
        public HttpResponseMessage HostSettings()
        {
            return Request.CreateResponse(HttpStatusCode.OK, HostController.Instance.GetSettingsDictionary());
        }

        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage CurrentPortalSettings()
        {
            return Request.CreateResponse(HttpStatusCode.OK, TestablePortalController.Instance.GetPortalSettingsDictionary(PortalSettings.PortalId));
        }

        public class UpdateSettingDTO
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage UpdateHostSetting(UpdateSettingDTO submitted)
        {
            HostController.Instance.Update(submitted.Key, submitted.Value);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage UpdatePortalSetting(UpdateSettingDTO submitted)
        {
            TestablePortalController.Instance.UpdatePortalSetting(PortalSettings.PortalId, submitted.Key, submitted.Value);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}