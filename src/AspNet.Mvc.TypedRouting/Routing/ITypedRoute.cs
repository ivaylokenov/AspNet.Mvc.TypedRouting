namespace AspNet.Mvc.TypedRouting.Routing
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface ITypedRoute
    {
        /// <summary>
        /// Resolves the specified route to the provided controller type.
        /// </summary>
        /// <typeparam name="TController">Controller type to which the route will be resolved.</typeparam>
        /// <returns>Typed route details.</returns>
        ITypedRouteDetails ToController<TController>()
            where TController : class;

        /// <summary>
        /// Resolves the specified route to the provided action.
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="expression">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>Typed route details.</returns>
        ITypedRouteDetails ToAction<TController>(Expression<Action<TController>> expression)
            where TController : class;

        /// <summary>
        /// Resolves the specified route to the provided action.
        /// </summary>
        /// <typeparam name="TController">Controller type to which the route will be resolved.</typeparam>
        /// <param name="expression">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>Typed route details.</returns>
        ITypedRouteDetails ToAction<TController>(Expression<Func<TController, Task>> expression)
            where TController : class;
    }
}
