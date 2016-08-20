namespace PerformanceTest
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.AspNetCore.Mvc.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using AspNet.Mvc.TypedRouting.LinkGeneration;

    public class Startup
    {
        private const int NumberOfIterations = 5000;

        public static IServiceProvider Services { get; private set; }

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
            // Run the full controller and action model building 
            // in order to simulate the default MVC behavior.

            var applicationPartManager = new ApplicationPartManager();
            applicationPartManager.ApplicationParts.Add(new AssemblyPart(Assembly.GetExecutingAssembly()));
            applicationPartManager.FeatureProviders.Add(new ControllerFeatureProvider());
            
            var options = new TestOptionsManager<MvcOptions>();

            var modelProvider = new DefaultApplicationModelProvider(options);

            var provider = new ControllerActionDescriptorProvider(
                applicationPartManager,
                new[] { modelProvider },
                options);

            var serviceCollection = new ServiceCollection();
            var list = new List<IActionDescriptorProvider>()
            {
                provider,
            };

            serviceCollection.AddSingleton(typeof(IEnumerable<IActionDescriptorProvider>), list);
            serviceCollection.AddSingleton(typeof(IActionDescriptorCollectionProvider), typeof(ActionDescriptorCollectionProvider));
            serviceCollection.AddSingleton(typeof(IUniqueRouteKeysProvider), typeof(UniqueRouteKeysProvider));
            serviceCollection.AddSingleton(typeof(IExpressionRouteHelper), typeof(ExpressionRouteHelper));

            Services = serviceCollection.BuildServiceProvider();
        }

        #endregion

        #region Mocked Objects
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
            var context = CreateHttpContext(Services, string.Empty);
            var actionContext = CreateActionContext(context);

            var actionSelector = new Mock<IActionSelector>();
            return new UrlHelper(actionContext);
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

    public class MyController : Controller
    {
        public MyController()
        {
            this.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = Startup.Services
                }
            };
        }

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
}
