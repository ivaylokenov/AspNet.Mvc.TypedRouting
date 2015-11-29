using AspNet.Mvc.TypedRouting.Internals;

namespace Microsoft.AspNet.Builder
{
    using AspNet.Routing;

    public static class RouteBuilderExtensions
    {
        /// <summary>
        /// Allows using typed expression based link generation in ASP.NET MVC 6 application.
        /// </summary>
        public static void UseTypedRouting(this IRouteBuilder routeBuilder)
        {
            ExpressionRouteHelper.Initialize(routeBuilder.ServiceProvider);
        }
    }
}
