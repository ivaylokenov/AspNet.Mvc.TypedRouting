namespace AspNet.Mvc.TypedRouting.LinkGeneration
{
    using Microsoft.AspNetCore.Mvc.ApplicationModels;

    internal class LinkGenerationControllerModelConvention : IControllerModelConvention
    {
        private readonly UniqueRouteKeysProvider routeKeysProvider;

        public LinkGenerationControllerModelConvention(UniqueRouteKeysProvider uniqueRouteKeysProvider)
        {
            routeKeysProvider = uniqueRouteKeysProvider;
        }

        public void Apply(ControllerModel controller)
        {
            foreach (var routeValue in controller.RouteValues)
            {
                routeKeysProvider.AddKey(routeValue.Key);
            }
        }
    }
}
