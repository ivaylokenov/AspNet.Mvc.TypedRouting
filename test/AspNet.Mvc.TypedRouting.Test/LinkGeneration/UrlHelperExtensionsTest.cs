namespace AspNet.Mvc.TypedRouting.Test.LinkGeneration
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Builder.Internal;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Internal;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing.Internal;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.ObjectPool;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text.Encodings.Web;
    using Xunit;

    using With = Microsoft.AspNetCore.Mvc.With;
    using TypedRouting.LinkGeneration;

    [Collection("TypedRoutingTests")]
    public class UrlHelperExtensionsTest
    {
        [Fact]
        public void UrlActionWithExpressionAndAllParameters_ReturnsExpectedResult()
        {
            // Arrange
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
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");

            // Act
            var url = urlHelper.Action<NormalController>(c => c.ActionWithParameters(With.No<int>(), With.No<string>()), new { id = 1, text = "othertext" });

            // Assert
            Assert.Equal("/app/Normal/ActionWithParameters/1?text=othertext", url);
        }

        [Fact]
        public void LinkWithAllParameters_ReturnsExpectedResult()
        {
            // Arrange
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");

            // Act
            var url = urlHelper.Link<NormalController>("namedroute", c => c.ActionWithParameters(1, "sometext"));

            // Assert
            Assert.Equal("http://localhost/app/named/Normal/ActionWithParameters/1?text=sometext", url);
        }

        [Fact]
        public void LinkWithNullRouteName_ReturnsExpectedResult()
        {
            // Arrange
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");

            // Act
            var url = urlHelper.Link<NormalController>(null, c => c.ActionWithParameters(1, "sometext"));

            // Assert
            Assert.Equal("http://localhost/app/Normal/ActionWithParameters/1?text=sometext", url);
        }

        [Fact]
        public void LinkWithAdditionalRouteValues_ReturnsExpectedResult()
        {
            // Arrange
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");

            // Act
            var url = urlHelper.Link<NormalController>(null, c => c.ActionWithParameters(1, "sometext"), new { text = "othertext" });

            // Assert
            Assert.Equal("http://localhost/app/Normal/ActionWithParameters/1?text=othertext", url);
        }

        [Fact]
        public void NormalControllerToAreaController_GeneratesCorrectLink()
        {
            // Arrange
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");
            var controller = new NormalController { Url = urlHelper };

            // Act
            var result = controller.ToAreaAction() as ContentResult;

            // Assert
            Assert.Equal("/app/Admin/Area/ToEmptyAreaAction", result.Content);
        }
        
        [Fact]
        public void AreaControllerToNormalController_GeneratesCorrectLink()
        {
            // Arrange
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");
            var controller = new AreaController { Url = urlHelper };

            // Act
            var result = controller.ToEmptyAreaAction() as ContentResult;

            // Assert
            Assert.Equal("/app/Normal/ActionWithoutParameters", result.Content);
        }

        [Fact]
        public void AreaControllerToAreaController_GeneratesCorrectLink()
        {
            // Arrange
            var services = GetServices();
            var urlHelper = CreateUrlHelperWithRouteCollection(services, "/app");
            var controller = new AreaController { Url = urlHelper };

            // Act
            var result = controller.ToOtherAreaAction() as ContentResult;

            // Assert
            Assert.Equal("/app/Support/AnotherArea", result.Content);
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

        private static ActionContext CreateActionContext(HttpContext context)
        {
            return CreateActionContext(context, (new Mock<IRouter>()).Object);
        }

        private static ActionContext CreateActionContext(HttpContext context, IRouter router)
        {
            var routeData = new RouteData();
            routeData.Routers.Add(router);

            return new ActionContext(context, routeData, new ActionDescriptor());
        }

        private static IServiceProvider GetServices()
        {
            var services = new Mock<IServiceProvider>();

            var routeOptions = new RouteOptions();
            routeOptions.ConstraintMap.Add("exists", typeof(KnownRouteValueConstraint));

            var optionsAccessor = new Mock<IOptions<RouteOptions>>();
            optionsAccessor
                .SetupGet(o => o.Value)
                .Returns(routeOptions);
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

            var actionDescriptorCollectionProvider = new ActionDescriptorCollectionProvider(services.Object);

            services
                .Setup(s => s.GetService(typeof(IActionDescriptorCollectionProvider)))
                .Returns(actionDescriptorCollectionProvider);

            services
                .Setup(s => s.GetService(typeof(IEnumerable<IActionDescriptorProvider>)))
                .Returns(TestInit.GetActionDescriptorProviders());

            services
                .Setup(s => s.GetService(typeof(RoutingMarkerService)))
                .Returns(new RoutingMarkerService());

            services
                .Setup(s => s.GetService(typeof(UrlEncoder)))
                .Returns(UrlEncoder.Default);

            services
                .Setup(s => s.GetService(typeof(IExpressionRouteHelper)))
                .Returns(new ExpressionRouteHelper(actionDescriptorCollectionProvider, new UniqueRouteKeysProvider()));

            var objectPoolProvider = new DefaultObjectPoolProvider();
            var objectPolicy = new UriBuilderContextPooledObjectPolicy(UrlEncoder.Default);
            var objectPool = objectPoolProvider.Create(objectPolicy);

            services
                .Setup(s => s.GetService(typeof(ObjectPool<UriBuildingContext>)))
                .Returns(objectPool);

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
            var applicationBuilder = new ApplicationBuilder(services);
            var routeBuilder = new RouteBuilder(applicationBuilder);

            var target = new Mock<IRouter>(MockBehavior.Strict);
            target
                .Setup(router => router.GetVirtualPath(It.IsAny<VirtualPathContext>()))
                .Returns<VirtualPathContext>(context => null);
            routeBuilder.DefaultHandler = target.Object;

            routeBuilder.MapRoute("areaRoute",
                        "{area:exists}/{controller=Home}/{action=Index}");

            routeBuilder.MapRoute(string.Empty,
                        "{controller}/{action}/{id}",
                        new RouteValueDictionary(new { id = "defaultid" }));

            routeBuilder.MapRoute("namedroute",
                        "named/{controller}/{action}/{id}",
                        new RouteValueDictionary(new { id = "defaultid" }));

            var mockHttpRoute = new Mock<IRouter>();
            mockHttpRoute
                .Setup(mock => mock.GetVirtualPath(It.Is<VirtualPathContext>(c => string.Equals(c.RouteName, mockRouteName))))
                .Returns(new VirtualPathData(mockHttpRoute.Object, mockTemplateValue));

            routeBuilder.Routes.Add(mockHttpRoute.Object);
            return routeBuilder.Build();
        }

        private static UrlHelper CreateUrlHelper(string appBase, IRouter router)
        {
            var services = GetServices();
            var context = CreateHttpContext(services, appBase);
            var actionContext = CreateActionContext(context, router);
            
            return new UrlHelper(actionContext);
        }

        private static IUrlHelper CreateUrlHelper()
        {
            var services = GetServices();
            var context = CreateHttpContext(services, string.Empty);
            var actionContext = CreateActionContext(context);
            
            return new UrlHelper(actionContext);
        }
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

        public IActionResult ToAreaAction()
        {
            return Content(Url.Action<AreaController>(c => c.ToEmptyAreaAction()));
        }
    }

    [Area("Admin")]
    public class AreaController : Controller
    {
        public IActionResult ToEmptyAreaAction()
        {
            return Content(Url.Action<NormalController>(c => c.ActionWithoutParameters()));
        }

        public IActionResult ToOtherAreaAction()
        {
            return Content(Url.Action<AnotherAreaController>(c => c.Index()));
        }
    }

    [Area("Support")]
    public class AnotherAreaController : Controller
    {
        public IActionResult Index()
        {
            return null;
        }
    }

    internal class LoggerFactory : ILoggerFactory
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger();
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }
    }

    internal class Logger : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new Disposable();
        }
    }

    internal class Disposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}