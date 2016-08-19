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
        /// <returns>Typed route builder.</returns>
        ITypedRouteBuilder Get(string template, Action<ITypedRoute> configuration);

        /// <summary>
        /// Adds HTTP POST method route by the provided template. 
        /// </summary>
        /// <param name="template">Route template.</param>
        /// <param name="configuration">Route configuration.</param>
        /// <returns>Typed route builder.</returns>
        ITypedRouteBuilder Post(string template, Action<ITypedRoute> configuration);

        /// <summary>
        /// Adds HTTP PUT method route by the provided template. 
        /// </summary>
        /// <param name="template">Route template.</param>
        /// <param name="configuration">Route configuration.</param>
        /// <returns>Typed route builder.</returns>
        ITypedRouteBuilder Put(string template, Action<ITypedRoute> configuration);

        /// <summary>
        /// Adds HTTP DELETE method route by the provided template. 
        /// </summary>
        /// <param name="template">Route template.</param>
        /// <param name="configuration">Route configuration.</param>
        /// <returns>Typed route builder.</returns>
        ITypedRouteBuilder Delete(string template, Action<ITypedRoute> configuration);

        /// <summary>
        /// Adds typed route by the provided template. 
        /// </summary>
        /// <param name="template">Route template.</param>
        /// <param name="configuration">Route configuration.</param>
        /// <returns>Typed route builder.</returns>
        ITypedRouteBuilder Add(string template, Action<ITypedRoute> configuration);
    }
}
