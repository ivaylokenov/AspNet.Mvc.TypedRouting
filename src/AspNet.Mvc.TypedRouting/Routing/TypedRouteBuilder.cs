namespace AspNet.Mvc.TypedRouting.Routing
{
    using System;
    using System.Collections.Generic;

    public class TypedRouteBuilder : ITypedRouteBuilder
    {
        public ITypedRouteBuilder Get(string template, Action<ITypedRoute> configuration)
        {
            return AddRoute(template, configuration, "GET");
        }

        public ITypedRouteBuilder Post(string template, Action<ITypedRoute> configuration)
        {
            return AddRoute(template, configuration, "POST");
        }

        public ITypedRouteBuilder Put(string template, Action<ITypedRoute> configuration)
        {
            return AddRoute(template, configuration, "PUT");
        }

        public ITypedRouteBuilder Delete(string template, Action<ITypedRoute> configuration)
        {
            return AddRoute(template, configuration, "DELETE");
        }

        public ITypedRouteBuilder Add(string template, Action<ITypedRoute> configuration)
        {
            return AddRoute(template, configuration);
        }

        private ITypedRouteBuilder AddRoute(string template, Action<ITypedRoute> configuration, params string[] httpMethods)
        {
            // Action template should be replaced because we are actually using attribute route models.
            var route = new TypedRoute(template.Replace("{action}", "[action]"), httpMethods);
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

            return this;
        }
    }
}
