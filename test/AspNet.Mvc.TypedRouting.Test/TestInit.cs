namespace AspNet.Mvc.TypedRouting.Test
{
    using AspNet.Mvc.TypedRouting.Internals;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Setups;
    using Xunit;

    public class TestInit
    {
        public TestInit()
        {
            var testAssembly = Assembly.Load(new AssemblyName("AspNet.Mvc.TypedRouting.Test"));

            // Run the full controller and action model building 
            // in order to simulate the default MVC behavior.
            var options = new TestOptionsManager<MvcOptions>();

            var applicationPartManager = new ApplicationPartManager();
            applicationPartManager.FeatureProviders.Add(new ControllerFeatureProvider());
            applicationPartManager.ApplicationParts.Add(new AssemblyPart(testAssembly));

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

            ExpressionRouteHelper.Initialize(serviceCollection.BuildServiceProvider());
        }
        
        [CollectionDefinition("TypedRoutingTests")]
        public class TestCollection : ICollectionFixture<TestInit>
        {
        }
    }
}
