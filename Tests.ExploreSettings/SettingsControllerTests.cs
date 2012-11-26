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

using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using DotNetNuke.ComponentModel;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Web.Api;
using ExploreSettings;
using ExploreSettings.Testables;
using Moq;
using NUnit.Framework;

namespace Tests.ExploreSettings
{
    [TestFixture]
    public class SettingsControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            ComponentFactory.Container = new SimpleContainer();
        }

        [Test]
        public void HostSettingsRetreivesAllSettings()
        {
            //Arrange
            var expected = new Dictionary<string, string>{{"SettingA", "valueA"}, {"SettingB", "valueB"}};
            var mockHostController = new Mock<IHostController>();
            mockHostController.Setup(x => x.GetSettingsDictionary()).Returns(expected);
            HostController.RegisterInstance(mockHostController.Object);

            //Act
            SettingsController settingsController = SetupControllerForTests(new SettingsController(), HttpMethod.Get);
            HttpResponseMessage result = settingsController.HostSettings();

            //Assert
            Assert.IsTrue(result.IsSuccessStatusCode);
            CollectionAssert.AreEquivalent(expected, result.Content.ReadAsAsync<Dictionary<string, string>>().Result);
        }

        [Test]
        public void PortalSettingsRetreivesAllPortalSettingsForCurrentPortal()
        {
            const int currentPortalId = 0;
            
            //Arrange
            var expected = new Dictionary<string, string> { { "PortalSettingA", "valueA" }, { "PoratalSettingB", "valueB" } };
            var mockPortalController = new Mock<ITestablePortalController>();
            
            mockPortalController.Setup(x => x.GetPortalSettingsDictionary(currentPortalId)).Returns(expected);
            TestablePortalController.SetTestableInstance(mockPortalController.Object);

            var controller = SetupControllerForTests(new SettingsController(), HttpMethod.Get);
            var mockInternalTestablePortalController = new Mock<IPortalController>();
            mockInternalTestablePortalController.Setup(x => x.GetCurrentPortalSettings()).Returns(new PortalSettings {PortalId = currentPortalId});
            DotNetNuke.Entities.Portals.Internal.TestablePortalController.SetTestableInstance(mockInternalTestablePortalController.Object);
            
            //Act
            var result = controller.CurrentPortalSettings();

            //Assert
            Assert.IsTrue(result.IsSuccessStatusCode);
            CollectionAssert.AreEquivalent(expected, result.Content.ReadAsAsync<Dictionary<string, string>>().Result);
        }

        [Test]
        public void UpdateHostSettingsCallsHostControllerUpdate()
        {
            //Arrange
            var mockHostController = new Mock<IHostController>();
            HostController.RegisterInstance(mockHostController.Object);
            
            //Act
            var controller = SetupControllerForTests(new SettingsController(), HttpMethod.Post);
            var result = controller.UpdateHostSetting("key", "value");

            //Assert
            Assert.IsTrue(result.IsSuccessStatusCode);
            mockHostController.Verify(x => x.Update("key", "value"), Times.Once());
        }

        [Test]
        public void UpdatePortalSettingCalls()
        {
            const int currentPortalId = 0;

            //Arrange
            var mockPortalController = new Mock<ITestablePortalController>();
            TestablePortalController.SetTestableInstance(mockPortalController.Object);

            var mockInternalTestablePortalController = new Mock<IPortalController>();
            mockInternalTestablePortalController.Setup(x => x.GetCurrentPortalSettings()).Returns(new PortalSettings { PortalId = currentPortalId });
            DotNetNuke.Entities.Portals.Internal.TestablePortalController.SetTestableInstance(mockInternalTestablePortalController.Object);

            var controller = SetupControllerForTests(new SettingsController(), HttpMethod.Post);

            //Act
            var result = controller.UpdatePortalSetting("key", "value");

            //Assert
            Assert.IsTrue(result.IsSuccessStatusCode);
            mockPortalController.Verify(x => x.UpdatePortalSetting(currentPortalId, "key", "value"), Times.Once());
        }

        private static T SetupControllerForTests<T>(T controller, HttpMethod method) where T: DnnApiController
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(method, "http://localhost/");  //ignoring url in tests so far, if it becomes important it will need changed
            var route = config.Routes.MapHttpRoute("name", "{controller}/{action}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Settings" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            return controller;
        }
    }
}