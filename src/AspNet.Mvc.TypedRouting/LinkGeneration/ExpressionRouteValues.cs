namespace AspNet.Mvc.TypedRouting.LinkGeneration
{
    using System.Collections.Generic;

    public class ExpressionRouteValues
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public IDictionary<string, object> RouteValues { get; set; }
    }
}
