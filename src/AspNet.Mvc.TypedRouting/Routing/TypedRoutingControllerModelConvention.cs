namespace AspNet.Mvc.TypedRouting.Routing
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;

    internal class TypedRoutingControllerModelConvention : IControllerModelConvention
    {
        private TypedRouteBuilder routeBuilder;

        public TypedRoutingControllerModelConvention(TypedRouteBuilder typedRouteBuilder)
        {
            routeBuilder = typedRouteBuilder;
        }

        public void Apply(ControllerModel controller)
        {
            var routes = routeBuilder.GetTypedRoutes();

            if (routes.ContainsKey(controller.ControllerType))
            {
                var typedRoutes = routes[controller.ControllerType];
                foreach (var route in typedRoutes)
                {
                    var selectorModel = new SelectorModel
                    {
                        AttributeRouteModel = route
                    };

                    var selectors = controller.Selectors;

                    var action = controller.Actions.FirstOrDefault(x => x.ActionMethod == route.ActionMember);
                    if (action != null)
                    {
                        foreach (var constraint in route.Constraints)
                        {
                            selectorModel.ActionConstraints.Add(constraint);
                        }

                        selectors = action.Selectors;
                    }

                    selectors.Clear();
                    selectors.Add(selectorModel);
                }
            }
        }
    }
}
