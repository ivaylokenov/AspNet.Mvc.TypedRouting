namespace Microsoft.AspNetCore.Builder
{
    using Routing;
    using System;

    public static class RouteBuilderExtensions
    {
        /// <summary>
        /// Allows using typed expression based link generation in ASP.NET Core MVC application.
        /// </summary>
        [Obsolete("UseTypedRouting is no longer needed and will be removed in the next version. Call 'AddMvc().AddTypedRouting()' instead.")]
        public static IRouteBuilder UseTypedRouting(this IRouteBuilder routeBuilder)
        {
            return routeBuilder;
        }
    }
}
