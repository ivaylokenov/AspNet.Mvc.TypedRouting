namespace AspNet.Mvc.TypedRouting.Routing
{
    using Microsoft.AspNetCore.Mvc.ActionConstraints;

    public interface ITypedRouteDetails
    {
        ITypedRouteDetails WithName(string name);
        
        ITypedRouteDetails WithActionConstraint(IActionConstraintMetadata constraint);

        ITypedRouteDetails WithActionConstraints(params IActionConstraintMetadata[] constraints);

        ITypedRouteDetails ForHttpMethod(string method);

        ITypedRouteDetails ForHttpMethods(params string[] methods);
    }
}
