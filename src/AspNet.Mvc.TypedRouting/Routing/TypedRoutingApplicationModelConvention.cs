namespace AspNet.Mvc.TypedRouting.Routing
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Internal;

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
                        var action = controller.Actions.FirstOrDefault(x => x.ActionMethod == route.ActionMember);
                        if (action != null)
                        {
                            action.Selectors.Clear();

                            action.Selectors.Add(new SelectorModel
                            {
                                AttributeRouteModel = route,
                                ActionConstraints =
                                {
                                    new HttpMethodActionConstraint(route.HttpMethods)
                                }
                            });
                        }
                        else
                        {
                            controller.Selectors.Clear();

                            controller.Selectors.Add(new SelectorModel
                            {
                                AttributeRouteModel = route
                            });
                        }
                    }
                }
            }
        }
    }
}
