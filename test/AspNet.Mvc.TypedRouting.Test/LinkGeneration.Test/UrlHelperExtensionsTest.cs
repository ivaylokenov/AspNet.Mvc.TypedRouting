namespace AspNet.Mvc.TypedRouting.Test.LinkGeneration.Test
{
    using TypedRouting.Internals;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Http;
    using Microsoft.AspNet.Http.Internal;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Mvc.Abstractions;
    using Microsoft.AspNet.Mvc.ApplicationModels;
    using Microsoft.AspNet.Mvc.Controllers;
    using Microsoft.AspNet.Mvc.Infrastructure;
    using Microsoft.AspNet.Mvc.Routing;
    using Microsoft.AspNet.Routing;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.OptionsModel;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class UrlHelperExtensionsTest
    {
        [Fact]
        public void UrlActionWithExpressionAndAllParameters_ReturnsExpectedResult()
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");

            // Act
            var url = urlHelper.Action<NormalController>(c => c.ActionWithoutParameters(),
                values: null,
                protocol: "https",
                host: "remotelyhost",
                fragment: "somefragment");
            
            // Assert
            Assert.Equal("https://remotelyhost/app/Normal/ActionWithoutParameters#somefragment", url);
        }

        [Fact]
        public void UrlActionWithExpressionActionWithParameters_ReturnsExpectedResult()
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");

            // Act
            var url = urlHelper.Action<NormalController>(c => c.ActionWithParameters(1, "sometext"));

            // Assert
            Assert.Equal("/app/Normal/ActionWithParameters/1?text=sometext", url);
        }

        [Fact]
        public void UrlActionWithExpressionActionWithParametersAndAdditionalValues_ReturnsExpectedResult()
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");

            // Act
            var url = urlHelper.Action<NormalController>(c => c.ActionWithParameters(1, "sometext"), new { text = "othertext" });

            // Assert
            Assert.Equal("/app/Normal/ActionWithParameters/1?text=othertext", url);
        }

        [Fact]
        public void UrlActionWithExpressionActionWithNoParameterssAndAdditionalValues_ReturnsExpectedResult()
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");

            // Act
            var url = urlHelper.Action<NormalController>(c => c.ActionWithParameters(With.No<int>(), With.No<string>()), new { id = 1, text = "othertext" });

            // Assert
            Assert.Equal("/app/Normal/ActionWithParameters/1?text=othertext", url);
        }

        private static void AttachActionDescriptorsCollectionProvider()
        {
            // Run the full controller and action model building 
            // in order to simulate the default MVC behavior.
            var controllerTypes = typeof(UrlHelperExtensionsTest)
                .GetNestedTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .Select(t => t.GetTypeInfo())
                .ToList();

            var options = new TestOptionsManager<MvcOptions>();

            var controllerTypeProvider = new StaticControllerTypeProvider(controllerTypes);
            var modelProvider = new DefaultApplicationModelProvider(options);

            var provider = new ControllerActionDescriptorProvider(
                controllerTypeProvider,
                new[] { modelProvider },
                options);

            var serviceContainer = new ServiceContainer();
            var list = new List<IActionDescriptorProvider>()
            {
                provider,
            };

            var actionDescriptorCollectionProvider = new DefaultActionDescriptorsCollectionProvider(serviceContainer);

            serviceContainer.AddService(typeof(IEnumerable<IActionDescriptorProvider>), list);
            serviceContainer.AddService(typeof(IActionDescriptorsCollectionProvider), actionDescriptorCollectionProvider);

            ExpressionRouteHelper.ServiceProvider = serviceContainer;
        }

        private static HttpContext CreateHttpContext(
            IServiceProvider services,
            string appRoot)
        {
            var context = new DefaultHttpContext();
            context.RequestServices = services;

            context.Request.PathBase = new PathString(appRoot);
            context.Request.Host = new HostString("localhost");

            return context;
        }

        private static IActionContextAccessor CreateActionContext(HttpContext context)
        {
            return CreateActionContext(context, (new Mock<IRouter>()).Object);
        }

        private static IActionContextAccessor CreateActionContext(HttpContext context, IRouter router)
        {
            var routeData = new RouteData();
            routeData.Routers.Add(router);

            var actionContext = new ActionContext(context, routeData, new ActionDescriptor());
            return new ActionContextAccessor() { ActionContext = actionContext };
        }

        private static IServiceProvider GetServices()
        {
            var services = new Mock<IServiceProvider>();

            var optionsAccessor = new Mock<IOptions<RouteOptions>>();
            optionsAccessor
                .SetupGet(o => o.Value)
                .Returns(new RouteOptions());
            services
                .Setup(s => s.GetService(typeof(IOptions<RouteOptions>)))
                .Returns(optionsAccessor.Object);

            services
                .Setup(s => s.GetService(typeof(IInlineConstraintResolver)))
                .Returns(new DefaultInlineConstraintResolver(optionsAccessor.Object));

            services
                .Setup(s => s.GetService(typeof(ILoggerFactory)))
                .Returns(new LoggerFactory());

            services
                .Setup(s => s.GetService(typeof(IActionContextAccessor)))
                .Returns(new ActionContextAccessor()
                {
                    ActionContext = new ActionContext()
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            RequestServices = services.Object,
                        },
                        RouteData = new RouteData(),
                    },
                });

            return services.Object;
        }

        private static UrlHelper CreateUrlHelperWithRouteCollection(IServiceProvider services, string appPrefix)
        {
            var routeCollection = GetRouter(services);
            return CreateUrlHelper(appPrefix, routeCollection);
        }
        
        private static IRouter GetRouter(IServiceProvider services)
        {
            return GetRouter(services, "mockRoute", "/mockTemplate");
        }

        private static IRouter GetRouter(
            IServiceProvider services,
            string mockRouteName,
            string mockTemplateValue)
        {
            var routeBuilder = new RouteBuilder();
            routeBuilder.ServiceProvider = services;

            var target = new Mock<IRouter>(MockBehavior.Strict);
            target
                .Setup(router => router.GetVirtualPath(It.IsAny<VirtualPathContext>()))
                .Callback<VirtualPathContext>(context => context.IsBound = true)
                .Returns<VirtualPathContext>(context => null);
            routeBuilder.DefaultHandler = target.Object;

            routeBuilder.MapRoute(string.Empty,
                        "{controller}/{action}/{id}",
                        new RouteValueDictionary(new { id = "defaultid" }));

            routeBuilder.MapRoute("namedroute",
                        "named/{controller}/{action}/{id}",
                        new RouteValueDictionary(new { id = "defaultid" }));

            var mockHttpRoute = new Mock<IRouter>();
            mockHttpRoute
                .Setup(mock => mock.GetVirtualPath(It.Is<VirtualPathContext>(c => string.Equals(c.RouteName, mockRouteName))))
                .Callback<VirtualPathContext>(c => c.IsBound = true)
                .Returns(new VirtualPathData(mockHttpRoute.Object, mockTemplateValue));

            routeBuilder.Routes.Add(mockHttpRoute.Object);
            return routeBuilder.Build();
        }

        private static UrlHelper CreateUrlHelper(string appBase, IRouter router)
        {
            var services = GetServices();
            var context = CreateHttpContext(services, appBase);
            var actionContext = CreateActionContext(context, router);

            var actionSelector = new Mock<IActionSelector>();
            return new UrlHelper(actionContext, actionSelector.Object);
        }

        private static IUrlHelper CreateUrlHelper()
        {
            var services = GetServices();
            var context = CreateHttpContext(services, string.Empty);
            var actionContext = CreateActionContext(context);

            var actionSelector = new Mock<IActionSelector>();
            return new UrlHelper(actionContext, actionSelector.Object);
        }

        public class NormalController : Controller
        {
            public IActionResult ActionWithoutParameters()
            {
                return null;
            }
            
            public IActionResult ActionWithParameters(int id, string text)
            {
                return null;
            }
        }
    }
}
