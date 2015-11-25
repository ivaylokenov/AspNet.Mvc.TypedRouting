using AspNet.Mvc.TypedRouting.Internals;

namespace Microsoft.AspNet.Mvc
{
    using AspNet.Routing;

    public static class IRouterExtensions
    {
        public static void AddTypedRouting(this IRouteBuilder router)
        {
            ExpressionRouteHelper.ServiceProvider = router.ServiceProvider;
        }
    }
}
