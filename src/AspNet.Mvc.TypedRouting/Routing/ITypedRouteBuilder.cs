namespace AspNet.Mvc.TypedRouting.Routing
{
    using System;

    public interface ITypedRouteBuilder
    {
        /// <summary>
        /// Adds HTTP GET method route by the provided template. 
        /// </summary>
        /// <param name="template">Route template.</param>
        /// <param name="configuration">Route configuration.</param>
        /// <returns>Typed route.</returns>
        TypedRoute Get(string template, Action<TypedRoute> configuration);

        /// <summary>
        /// Adds HTTP POST method route by the provided template. 
        /// </summary>
        /// <param name="template">Route template.</param>
        /// <param name="configuration">Route configuration.</param>
        /// <returns>Typed route.</returns>
        TypedRoute Post(string template, Action<TypedRoute> configuration);

        /// <summary>
        /// Adds HTTP PUT method route by the provided template. 
        /// </summary>
        /// <param name="template">Route template.</param>
        /// <param name="configuration">Route configuration.</param>
        /// <returns>Typed route.</returns>
        TypedRoute Put(string template, Action<TypedRoute> configuration);

        /// <summary>
        /// Adds HTTP DELETE method route by the provided template. 
        /// </summary>
        /// <param name="template">Route template.</param>
        /// <param name="configuration">Route configuration.</param>
        /// <returns>Typed route.</returns>
        TypedRoute Delete(string template, Action<TypedRoute> configuration);

        /// <summary>
        /// Adds typed route by the provided template. 
        /// </summary>
        /// <param name="template">Route template.</param>
        /// <param name="configuration">Route configuration.</param>
        /// <returns>Typed route.</returns>
        TypedRoute Add(string template, Action<TypedRoute> configuration);
    }
}
