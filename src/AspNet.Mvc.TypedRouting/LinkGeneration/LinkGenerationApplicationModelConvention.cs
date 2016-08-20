namespace AspNet.Mvc.TypedRouting.LinkGeneration
{
    using Internals;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;

    public class LinkGenerationApplicationModelConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            foreach (var routeValue in controller.RouteValues)
            {
                ExpressionRouteHelper.UniqueRouteKeys.Add(routeValue.Key);
            }
        }
    }
}
