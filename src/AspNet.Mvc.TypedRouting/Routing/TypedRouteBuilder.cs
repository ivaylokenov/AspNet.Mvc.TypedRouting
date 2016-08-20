namespace AspNet.Mvc.TypedRouting.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class TypedRouteBuilder : ITypedRouteBuilder
    {
        private IDictionary<TypeInfo, List<TypedRoute>> routes;

        public TypedRouteBuilder()
        {
            routes = new Dictionary<TypeInfo, List<TypedRoute>>();
        }

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

        internal IDictionary<TypeInfo, List<TypedRoute>> GetTypedRoutes()
        {
            return routes;
        }

        private ITypedRouteBuilder AddRoute(string template, Action<ITypedRoute> configuration, params string[] httpMethods)
        {
            // Action template should be replaced because we are actually using attribute route models.
            var route = new TypedRoute(template.Trim('/').Replace("{action}", "[action]"), httpMethods);
            configuration(route);

            if (routes.ContainsKey(route.ControllerType))
            {
                var controllerActions = routes[route.ControllerType];
                controllerActions.Add(route);
            }
            else
            {
                var controllerActions = new List<TypedRoute> { route };
                routes.Add(route.ControllerType, controllerActions);
            }

            return this;
        }
    }
}
