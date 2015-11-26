using AspNet.Mvc.TypedRouting.Internals;

namespace Microsoft.AspNet.Mvc
{
    using AspNet.Routing;

    public static class RouterExtensions
    {
        /// <summary>
        /// Allows using typed expression based routes and link generations in MVC.
        /// </summary>
        public static void AddTypedRouting(this IRouteBuilder router)
        {
            ExpressionRouteHelper.ServiceProvider = router.ServiceProvider;
        }
    }
}
