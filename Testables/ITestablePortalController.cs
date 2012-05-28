using System;
using System.Collections.Generic;

namespace ExploreSettings.Testables
{
    public interface ITestablePortalController
    {
        IDictionary<string, string> GetPortalSettingsDictionary(int portalId);
        void UpdatePortalSetting(int portalId, string key, string value);
    }
}