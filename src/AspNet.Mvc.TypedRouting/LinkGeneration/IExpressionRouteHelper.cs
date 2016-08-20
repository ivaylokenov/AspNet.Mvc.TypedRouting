namespace AspNet.Mvc.TypedRouting.LinkGeneration
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IExpressionRouteHelper
    {
        ExpressionRouteValues Resolve<TController>(
               Expression<Action<TController>> expression,
               object additionalRouteValues = null,
               bool addControllerAndActionToRouteValues = false);

        ExpressionRouteValues Resolve<TController>(
            Expression<Func<TController, Task>> expression,
            object additionalRouteValues = null,
            bool addControllerAndActionToRouteValues = false);

        void ClearActionCache();
    }
}
