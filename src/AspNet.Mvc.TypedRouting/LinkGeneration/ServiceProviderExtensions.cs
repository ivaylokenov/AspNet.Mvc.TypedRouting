namespace AspNet.Mvc.TypedRouting.LinkGeneration
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    internal static class ServiceProviderExtensions
    {
        public static IExpressionRouteHelper GetExpressionRouteHelper(this IServiceProvider serviceProvider)
        {
            var expressionRouteHelper = serviceProvider?.GetService<IExpressionRouteHelper>();
            if (expressionRouteHelper == null)
            {
                throw new InvalidOperationException("'AddTypedRouting' must be called after 'AddMvc' in order to use typed routing and link generation.");
            }

            return expressionRouteHelper;
        }
    }
}
