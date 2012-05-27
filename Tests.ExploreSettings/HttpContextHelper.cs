using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using DotNetNuke.Common;
using Moq;

namespace Tests.ExploreSettings
{
    public static class HttpContextHelper
    {
        private static Mock<HttpContextBase> CrateMockHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Items).Returns(new Hashtable());

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Headers).Returns(new NameValueCollection());
            request.SetupGet(x => x.Params).Returns(new NameValueCollection());
            context.SetupGet(x => x.Request).Returns(request.Object);

            context.SetupGet(x => x.Response).Returns(new Mock<HttpResponseBase>().Object);
            context.SetupGet(x => x.Session).Returns(new Mock<HttpSessionStateBase>().Object);
            context.SetupGet(x => x.Server).Returns(new Mock<HttpServerUtilityBase>().Object);

            return context;
        }

        public static Mock<HttpContextBase> RegisterMockHttpContext()
        {
            var mock = CrateMockHttpContext();
            HttpContextSource.RegisterInstance(mock.Object);
            return mock;
        }
    }
}