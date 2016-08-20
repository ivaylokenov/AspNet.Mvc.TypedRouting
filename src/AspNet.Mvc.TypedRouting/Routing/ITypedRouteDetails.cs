namespace AspNet.Mvc.TypedRouting.Routing
{
    using Microsoft.AspNetCore.Mvc.ActionConstraints;

    public interface ITypedRouteDetails
    {
        /// <summary>
        /// Sets name to the specified route.
        /// </summary>
        /// <param name="name">Route name to set.</param>
        /// <returns>Typed route details.</returns>
        ITypedRouteDetails WithName(string name);

        /// <summary>
        /// Adds action constraint to the specified route.
        /// </summary>
        /// <param name="constraint">Action constraint to set to the specified route.</param>
        /// <returns>Typed route details.</returns>
        ITypedRouteDetails WithActionConstraint(IActionConstraintMetadata constraint);

        /// <summary>
        /// Adds action constraints to the specified route.
        /// </summary>
        /// <param name="constraints">Action constraints to set to the specified route.</param>
        /// <returns>Typed route details.</returns>
        ITypedRouteDetails WithActionConstraints(params IActionConstraintMetadata[] constraints);

        /// <summary>
        /// Adds HTTP method constraint to the specified route.
        /// </summary>
        /// <param name="method">Allowed HTTP method for the specified route.</param>
        /// <returns>Typed route details.</returns>
        ITypedRouteDetails ForHttpMethod(string method);

        /// <summary>
        /// Adds HTTP method constraints to the specified route.
        /// </summary>
        /// <param name="methods">Allowed HTTP methods for the specified route.</param>
        /// <returns>Typed route details.</returns>
        ITypedRouteDetails ForHttpMethods(params string[] methods);
    }
}
