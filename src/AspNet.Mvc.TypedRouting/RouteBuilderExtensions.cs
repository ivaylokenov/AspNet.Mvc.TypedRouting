using AspNet.Mvc.TypedRouting.Internals;

namespace Microsoft.AspNetCore.Builder
{
    using Routing;

    public static class RouteBuilderExtensions
    {
        /// <summary>
        /// Allows using typed expression based link generation in ASP.NET Core MVC application.
        /// </summary>
        public static void UseTypedRouting(this IRouteBuilder routeBuilder)
        {
            ExpressionRouteHelper.Initialize(routeBuilder.ServiceProvider);
        }
    }
}
