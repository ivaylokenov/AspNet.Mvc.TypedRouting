using AspNet.Mvc.TypedRouting.LinkGeneration;
using AspNet.Mvc.TypedRouting.Routing;

namespace Microsoft.AspNetCore.Builder
{
    using Mvc;
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds typed expression based routes in ASP.NET Core MVC application.
        /// </summary>
        /// <param name="routesConfiguration">Typed routes configuration.</param>
        public static IMvcBuilder AddTypedRouting(this IMvcBuilder mvcBuilder, Action<ITypedRouteBuilder> routesConfiguration = null)
        {
            var typedRouteBuilder = new TypedRouteBuilder();
            var uniqueRouteKeysProvider = new UniqueRouteKeysProvider();

            var services = mvcBuilder.Services;

            services.AddSingleton<IUniqueRouteKeysProvider>(uniqueRouteKeysProvider);
            services.AddSingleton<IExpressionRouteHelper, ExpressionRouteHelper>();

            services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new TypedRoutingControllerModelConvention(typedRouteBuilder));
                options.Conventions.Add(new LinkGenerationControllerModelConvention(uniqueRouteKeysProvider));
            });

            routesConfiguration?.Invoke(typedRouteBuilder);

            return mvcBuilder;
        }
    }
}
