using System;
using System.Collections.Generic;
using DotNetNuke.Entities.Portals;

namespace ExploreSettings.Testables
{
    internal class TestablePortalControllerImpl : ITestablePortalController
    {
        public IDictionary<string, string> GetPortalSettingsDictionary(int portalId)
        {
            return PortalController.GetPortalSettingsDictionary(portalId);
        }
    }
}