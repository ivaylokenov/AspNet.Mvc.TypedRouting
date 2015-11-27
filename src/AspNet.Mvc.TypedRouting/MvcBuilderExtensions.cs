using AspNet.Mvc.TypedRouting.Routing;

namespace Microsoft.AspNet.Builder
{
    using Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds typed expression based routes in ASP.NET MVC 6 application.
        /// </summary>
        /// <param name="routesConfiguration">Typed routes configuration.</param>
        public static void AddTypedRouting(this IMvcBuilder mvcBuilder, Action<ITypedRouteBuilder> routesConfiguration)
        {
            mvcBuilder.Services.Configure<MvcOptions>(opts =>
            {
                opts.Conventions.Add(new TypedRoutingApplicationModelConvention());
            });

            routesConfiguration(new TypedRouteBuilder());
        }
    }
}
