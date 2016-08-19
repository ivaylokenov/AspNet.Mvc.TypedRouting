namespace AspNet.Mvc.TypedRouting.Routing
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface ITypedRoute
    {
        ITypedRouteDetails ToController<TController>()
            where TController : class;

        ITypedRouteDetails ToAction<TController>(Expression<Action<TController>> expression)
            where TController : class;

        ITypedRouteDetails ToAction<TController>(Expression<Func<TController, Task>> expression)
            where TController : class;
    }
}
