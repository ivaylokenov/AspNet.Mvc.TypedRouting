namespace AspNet.Mvc.TypedRouting.Routing
{
    using Microsoft.AspNetCore.Mvc.ActionConstraints;

    public interface ITypedRouteDetails
    {
        ITypedRouteDetails WithName(string name);

        ITypedRouteDetails WithActionConstraints(params IActionConstraintMetadata[] constraints);

        ITypedRouteDetails ForHttpMethods(params string[] methods);
    }
}
