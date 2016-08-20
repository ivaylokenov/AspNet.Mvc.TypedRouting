namespace AspNet.Mvc.TypedRouting.Test
{
    using LinkGeneration;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Internal;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Setups;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using TypedRouting.LinkGeneration;
    using Xunit;

    public class TestInit
    {
        public TestInit()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(typeof(IEnumerable<IActionDescriptorProvider>), GetActionDescriptorProviders());
            serviceCollection.AddSingleton(typeof(IActionDescriptorCollectionProvider), typeof(ActionDescriptorCollectionProvider));
            serviceCollection.AddSingleton(typeof(IUniqueRouteKeysProvider), typeof(UniqueRouteKeysProvider));
            serviceCollection.AddSingleton(typeof(IExpressionRouteHelper), typeof(ExpressionRouteHelper));
            serviceCollection.AddSingleton(typeof(IUrlHelperFactory), typeof(UrlHelperFactory));

            // test exception, if ExpressionRouteHelper is not initialized
            var exceptionMessage = Assert.Throws<InvalidOperationException>(() =>
            {
                new MyTestController().CreatedAtActionSameController();
            });

            Assert.Equal("'AddTypedRouting' must be called after 'AddMvc' in order to use typed routing and link generation.", exceptionMessage.Message);

            TestServices.Global = serviceCollection.BuildServiceProvider();
        }

        public static List<IActionDescriptorProvider> GetActionDescriptorProviders()
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
            return new List<IActionDescriptorProvider>()
            {
                provider,
            };
        }
        
        [CollectionDefinition("TypedRoutingTests")]
        public class TestCollection : ICollectionFixture<TestInit>
        {
        }
    }
}
