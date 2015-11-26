namespace AspNet.Mvc.TypedRouting.Test
{
    using TypedRouting.Internals;
    using Internals.Test;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Mvc.Abstractions;
    using Microsoft.AspNet.Mvc.ApplicationModels;
    using Microsoft.AspNet.Mvc.Controllers;
    using Microsoft.AspNet.Mvc.Infrastructure;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class TestInit
    {
        public TestInit()
        {
            var assemblyTestClasses = Assembly
                .GetExecutingAssembly()
                .DefinedTypes
                .Where(t => t.Name.EndsWith("Test"))
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
        
        [CollectionDefinition("TypedRoutingTests")]
        public class TestCollection : ICollectionFixture<TestInit>
        {
        }
    }
}
