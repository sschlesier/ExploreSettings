using System;
using DotNetNuke.Framework;
using ExploreSettings;

namespace ExploreSettings.Testables
{
    public class TestablePortalController : ServiceLocator<ITestablePortalController, TestablePortalController>
    {
        protected override Func<ITestablePortalController> GetFactory()
        {
            return () => new TestablePortalControllerImpl();
        }
    }
}