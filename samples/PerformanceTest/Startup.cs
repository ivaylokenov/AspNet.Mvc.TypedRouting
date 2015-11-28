namespace PerformanceTest
{
    using AspNet.Mvc.TypedRouting.Internals;
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
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    public class Startup
    {
        private const int NumberOfIterations = 5000;

        // as described in http://www.codeproject.com/Articles/61964/Performance-Tests-Precise-Run-Time-Measurements-wi
        public static void Main()
        {
            PrepareTypedRouting();
            PrepareThread();
            
            var urlHelper = CreateUrlHelper();

            var id = 1;
            var text = "text";
            var model = new RequestModel { Integer = 2, String = "text" };

            // Actions without parameters - 7 ms VS 20 ms
            Console.WriteLine("Actions without parameters");
            Console.WriteLine(new string('-', 40));

            RunAndMeasure("(\"action\", \"controller\")",
                () => urlHelper.Action("Action", "My")); // ~7 ms

            RunAndMeasure("(c => c.Action())",
                () => urlHelper.Action<MyController>(c => c.Action())); // ~20 ms

            Console.WriteLine(new string('-', 40));

            // Actions with constant parameters - 8 ms VS 25 ms
            Console.WriteLine("Actions with constant parameters");
            Console.WriteLine(new string('-', 40));

            RunAndMeasure("(\"action\", \"controller\", new { id = 1, text = \"text\" })",
                () => urlHelper.Action("Action", "My", new { id = 1, text = "text" })); // ~8 ms

            RunAndMeasure("(c => c.Action(1, \"text\"))",
                () => urlHelper.Action<MyController>(c => c.Action(1, "text"))); // ~25 ms

            Console.WriteLine(new string('-', 40));

            // Actions with variable primitive parameters - 8 ms VS 499 ms
            Console.WriteLine("Actions with variable primitive parameters");
            Console.WriteLine(new string('-', 40));

            RunAndMeasure("(\"action\", \"controller\", new { id, text })",
                () => urlHelper.Action("Action", "My", new { id, text })); // ~8 ms

            RunAndMeasure("(c => c.Action(id, text))",
                () => urlHelper.Action<MyController>(c => c.Action(id, text))); // ~499 ms

            Console.WriteLine(new string('-', 40));
            
            // Actions with variable primitive parameters (using With.No<T>) - 7 ms VS 70 ms
            Console.WriteLine("Actions with variable primitive parameters (using With.No<T>)");
            Console.WriteLine(new string('-', 40));

            RunAndMeasure("(\"action\", \"controller\", new { id, text })",
                () => urlHelper.Action("Action", "My", new { id, text })); // ~7 ms

            RunAndMeasure("(c => c.Action(id, text))",
                () => urlHelper.Action<MyController>(c => c.Action(With.No<int>(), With.No<string>()), new { id, text })); // ~70 ms

            Console.WriteLine(new string('-', 40));

            // Actions with variable reference parameters - 7 ms VS 692 ms
            Console.WriteLine("Actions with variable reference parameters");
            Console.WriteLine(new string('-', 40));

            RunAndMeasure("(\"action\", \"controller\", new { id, model })",
                () => urlHelper.Action("Action", "My", new { id, model })); // ~7 ms

            RunAndMeasure("(c => c.Action(id, model))",
                () => urlHelper.Action<MyController>(c => c.Action(id, model))); // ~692 ms

            Console.WriteLine(new string('-', 40));

            // Actions with variable reference parameters (using With.No<T>) - 8 ms VS 67 ms
            Console.WriteLine("Actions with variable reference parameters (using With.No<T>)");
            Console.WriteLine(new string('-', 40));

            RunAndMeasure("(\"action\", \"controller\", new { id, model })",
                () => urlHelper.Action("Action", "My", new { id, model })); // ~8 ms

            RunAndMeasure("(c => c.Action(With.No<int>(), With.No<RequestModel>()))",
                () => urlHelper.Action<MyController>(c => c.Action(With.No<int>(), With.No<RequestModel>()), new { id, model })); // ~67 ms

            Console.WriteLine(new string('-', 40));
        }
        
        public class MyController : Controller
        {
            public IActionResult Action()
            {
                return null;
            }

            public IActionResult Action(int id, string text)
            {
                return null;
            }

            public IActionResult Action(int id, RequestModel model)
            {
                return null;
            }
        }

        public class RequestModel
        {
            public int Integer { get; set; }

            public string String { get; set; }
        }

        private static void RunAndMeasure(string text, Action action)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < 1200)  // A Warmup of 1000-1500 ms stabilizes the CPU cache and pipeline.
            {
                action(); // Warmup
            }
            stopwatch.Stop();

            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < NumberOfIterations; i++)
            {
                action();
            }
            
            stopwatch.Stop();
            
            Console.WriteLine($"{text} - {stopwatch.Elapsed.Milliseconds} ms");
        }

        #region Prepare Typed Routing Tests

        private static void PrepareThread()
        {
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        private static void PrepareTypedRouting()
        {
            var assemblyTestClasses = Assembly
                .GetExecutingAssembly()
                .DefinedTypes
                .ToList();

            // Run the full controller and action model building 
            // in order to simulate the default MVC behavior.
            var controllerTypes = new List<TypeInfo>();

            foreach (var testClass in assemblyTestClasses)
            {
                var controllers = testClass
                    .GetNestedTypes()
                    .Where(t => t.Name.EndsWith("Controller"))
                    .Select(t => t.GetTypeInfo())
                    .ToList();

                controllerTypes.AddRange(controllers);
            }

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

        #endregion

        #region Mocked Objects
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
                .Returns(Mock.Of<ILoggerFactory>());

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

        private static IUrlHelper CreateUrlHelper()
        {
            var services = GetServices();
            var context = CreateHttpContext(services, string.Empty);
            var actionContext = CreateActionContext(context);

            var actionSelector = new Mock<IActionSelector>();
            return new UrlHelper(actionContext, actionSelector.Object);
        }

        public class TestOptionsManager<T> : OptionsManager<T>
        where T : class, new()
        {
            public TestOptionsManager()
                : base(Enumerable.Empty<IConfigureOptions<T>>())
            {
            }
        }
        #endregion
    }
}
