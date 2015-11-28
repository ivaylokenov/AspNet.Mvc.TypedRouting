namespace AspNet.Mvc.TypedRouting.Routing
{
    using System;
    using System.Collections.Generic;

    public class TypedRouteBuilder : ITypedRouteBuilder
    {
        public TypedRoute Get(string template, Action<TypedRoute> configuration)
        {
            return AddRoute(template, configuration).ForHttpMethods("GET");
        }

        public TypedRoute Post(string template, Action<TypedRoute> configuration)
        {
            return AddRoute(template, configuration).ForHttpMethods("POST");
        }

        public TypedRoute Put(string template, Action<TypedRoute> configuration)
        {
            return AddRoute(template, configuration).ForHttpMethods("PUT");
        }

        public TypedRoute Delete(string template, Action<TypedRoute> configuration)
        {
            return AddRoute(template, configuration).ForHttpMethods("DELETE");
        }

        public TypedRoute Add(string template, Action<TypedRoute> configuration)
        {
            return AddRoute(template, configuration);
        }

        private TypedRoute AddRoute(string template, Action<TypedRoute> configuration)
        {
            // Action template should be replaced because we are actually using attribute route models.
            var route = new TypedRoute(template.Replace("{action}", "[action]"));
            configuration(route);

            if (TypedRoutingApplicationModelConvention.Routes.ContainsKey(route.ControllerType))
            {
                var controllerActions = TypedRoutingApplicationModelConvention.Routes[route.ControllerType];
                controllerActions.Add(route);
            }
            else
            {
                var controllerActions = new List<TypedRoute> { route };
                TypedRoutingApplicationModelConvention.Routes.Add(route.ControllerType, controllerActions);
            }

            return route;
        }
    }
}
