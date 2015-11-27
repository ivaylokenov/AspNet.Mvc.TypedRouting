namespace AspNet.Mvc.TypedRouting.Test.LinkGeneration
{
    using System.IO;
    using Microsoft.AspNet.Html.Abstractions;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Extensions.WebEncoders;
    using Moq;
    using Xunit;
    using System.Collections.Generic;
    using Microsoft.AspNet.Routing;

    // Since the original MVC helper is living hell to test, these unit tests just test whether
    // the typed extensions pass correct values.
    [Collection("TypedRoutingTests")]
    public class HtmlHelperExtensionsTest
    {
        [Fact]
        public void ActionLink_PassesCorrectValuesToHtmlHelper()
        {
            // Arrange
            var htmlHelper = GetHtmlHelper();

            // Act
            var content = htmlHelper.ActionLink<MyController>("Link", c => c.Action(1, "test"), "Protocol", "HostName", "Fragment", new { other = "value" }, new { @class = "css-class" }) as TestHtmlContent;

            // Assert
            Assert.Equal("Link", content.LinkText);
            Assert.Equal("My", content.ControllerName);
            Assert.Equal("Action", content.ActionName);
            Assert.Equal("Protocol", content.Protocol);
            Assert.Equal("HostName", content.Hostname);
            Assert.Equal("Fragment", content.Fragment);
            Assert.Equal(3, content.RouteValues.Count);
            Assert.Equal(1, content.RouteValues["id"]);
            Assert.Equal("test", content.RouteValues["text"]);
            Assert.Equal("value", content.RouteValues["other"]);
            Assert.Equal(1, content.HtmlAttributes.Count);
            Assert.Equal("css-class", content.HtmlAttributes["class"]);
        }

        [Fact]
        public void RouteLink_PassesCorrectValuesToHtmlHelper()
        {
            // Arrange
            var htmlHelper = GetHtmlHelper();

            // Act
            var content = htmlHelper.RouteLink<MyController>("Link", "Route", c => c.Action(1, "test"), "Protocol", "HostName", "Fragment", new { other = "value" }, new { @class = "css-class" }) as TestHtmlContent;

            // Assert
            Assert.Equal("Link", content.LinkText);
            Assert.Equal("Route", content.RouteName);
            Assert.Equal("Protocol", content.Protocol);
            Assert.Equal("HostName", content.Hostname);
            Assert.Equal("Fragment", content.Fragment);
            Assert.Equal(5, content.RouteValues.Count);
            Assert.Equal("My", content.RouteValues["controller"]);
            Assert.Equal("Action", content.RouteValues["action"]);
            Assert.Equal(1, content.RouteValues["id"]);
            Assert.Equal("test", content.RouteValues["text"]);
            Assert.Equal("value", content.RouteValues["other"]);
            Assert.Equal(1, content.HtmlAttributes.Count);
            Assert.Equal("css-class", content.HtmlAttributes["class"]);
        }

        private static IHtmlHelper GetHtmlHelper()
        {
            var htmlHelperMock = new Mock<IHtmlHelper>();

            htmlHelperMock.Setup(h => h.ActionLink(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<object>()))
                .Returns((string linkText, string actionName, string controllerName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes) =>
                {
                    return new TestHtmlContent(linkText, actionName, controllerName, protocol, hostname, fragment, routeValues, htmlAttributes);
                });

            htmlHelperMock.Setup(h => h.RouteLink(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<object>()))
                .Returns((string linkText, string routeName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes) =>
                {
                    return new TestHtmlContent(linkText, routeName, protocol, hostname, fragment, routeValues, htmlAttributes);
                });

            return htmlHelperMock.Object;
        }

        private class TestHtmlContent : IHtmlContent
        {
            public TestHtmlContent(string linkText, string actionName, string controllerName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
            {
                this.LinkText = linkText;
                this.ActionName = actionName;
                this.ControllerName = controllerName;
                this.Protocol = protocol;
                this.Hostname = hostname;
                this.Fragment = fragment;
                this.RouteValues = new RouteValueDictionary(routeValues);
                this.HtmlAttributes = new RouteValueDictionary(htmlAttributes);
            }

            public TestHtmlContent(string linkText, string routeName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
            {
                this.RouteName = routeName;
                this.LinkText = linkText;
                this.Protocol = protocol;
                this.Hostname = hostname;
                this.Fragment = fragment;
                this.RouteValues = new RouteValueDictionary(routeValues);
                this.HtmlAttributes = new RouteValueDictionary(htmlAttributes);
            }

            public string LinkText { get; private set; }

            public string RouteName { get; private set; }

            public string ActionName { get; private set; }

            public string ControllerName { get; private set; }

            public string Protocol { get; private set; }

            public string Hostname { get; private set; }

            public string Fragment { get; private set; }

            public IDictionary<string, object> RouteValues { get; private set; }

            public IDictionary<string, object> HtmlAttributes { get; private set; }

            public void WriteTo(TextWriter writer, IHtmlEncoder encoder)
            {
            }
        }

        public class MyController : Controller
        {
            public IActionResult Action(int id, string text)
            {
                return null;
            }
        }
    }
}
