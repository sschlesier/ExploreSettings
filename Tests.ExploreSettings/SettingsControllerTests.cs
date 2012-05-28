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
using System.Web.Mvc;
using DotNetNuke.Common;
using DotNetNuke.ComponentModel;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Portals;
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
            HttpContextHelper.RegisterMockHttpContext();
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
            JsonResult result = new SettingsController().HostSettings();

            //Assert
            CollectionAssert.AreEquivalent(expected, (Dictionary<string, string>)result.Data);
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

            var controller = new SettingsController
                                 {ControllerContext = new ControllerContext {HttpContext = HttpContextSource.Current}};
            HttpContextSource.Current.Items["PortalSettings"] = new PortalSettings{PortalId = currentPortalId};
            
            //Act
            JsonResult result = controller.CurrentPortalSettings();

            //Assert
            CollectionAssert.AreEquivalent(expected, (Dictionary<string, string>)result.Data);
        }

        [Test]
        public void UpdateHostSettingsCallsHostControllerUpdate()
        {
            //Arrange
            var mockHostController = new Mock<IHostController>();
            HostController.RegisterInstance(mockHostController.Object);
            
            //Act
            var controller = new SettingsController();
            string result = controller.UpdateHostSetting("key", "value");

            //Assert
            Assert.AreEqual("OK", result);
            mockHostController.Verify(x => x.Update("key", "value"), Times.Once());
        }
    }
}