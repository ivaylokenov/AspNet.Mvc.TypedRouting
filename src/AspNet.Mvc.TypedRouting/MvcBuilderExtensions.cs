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
            mvcBuilder.Services.Configure<MvcOptions>(opts =>
            {
                opts.Conventions.Add(new TypedRoutingApplicationModelConvention());
            });

            routesConfiguration(new TypedRouteBuilder());

            return mvcBuilder;
        }
    }
}
