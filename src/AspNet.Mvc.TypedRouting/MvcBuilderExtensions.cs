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
        public static IMvcBuilder AddTypedRouting(this IMvcBuilder mvcBuilder, Action<ITypedRouteBuilder> routesConfiguration)
        {
            var typedRouteBuilder = new TypedRouteBuilder();

            mvcBuilder.Services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new TypedRoutingControllerModelConvention(typedRouteBuilder));
                options.Conventions.Add(new LinkGenerationApplicationModelConvention());
            });

            routesConfiguration(typedRouteBuilder);

            return mvcBuilder;
        }
    }
}
