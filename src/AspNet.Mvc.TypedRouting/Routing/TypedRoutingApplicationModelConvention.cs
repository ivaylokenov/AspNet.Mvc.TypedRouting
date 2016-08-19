namespace AspNet.Mvc.TypedRouting.Routing
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;

    // http://www.strathweb.com/2015/03/strongly-typed-routing-asp-net-mvc-6-iapplicationmodelconvention/
    public class TypedRoutingApplicationModelConvention : IApplicationModelConvention
    {
        internal static readonly Dictionary<TypeInfo, List<TypedRoute>> Routes = new Dictionary<TypeInfo, List<TypedRoute>>();

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                if (Routes.ContainsKey(controller.ControllerType))
                {
                    var typedRoutes = Routes[controller.ControllerType];
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
}
