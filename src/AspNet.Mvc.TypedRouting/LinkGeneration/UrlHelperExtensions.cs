using AspNet.Mvc.TypedRouting.Internals;

namespace Microsoft.AspNet.Mvc
{
    using AspNet.Routing;
    using Routing;
    using System;
    using System.Linq.Expressions;

    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(this IUrlHelper helper, Expression<Action<TController>> action)
        {
            return helper.Action(action, values: null, protocol: null, host: null, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved 
        /// and the specified additional route values.
        /// </summary>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="values">An object that contains additional route values.</param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(this IUrlHelper helper, Expression<Action<TController>> action, object values)
        {
            return helper.Action(action, values, protocol: null, host: null, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved
        /// and the specified additional route values and protocol to use.
        /// </summary>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="values">An object that contains additional route values.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(
            this IUrlHelper helper,
            Expression<Action<TController>> action,
            object values,
            string protocol)
        {
            return helper.Action(action, values, protocol, host: null, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved
        /// and the specified additional route values, protocol to use and host name.
        /// </summary>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="values">An object that contains additional route values.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="host">The host name for the URL.</param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(
            this IUrlHelper helper,
            Expression<Action<TController>> action,
            object values,
            string protocol,
            string host)
        {
            return helper.Action(action, values, protocol, host, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved
        /// and the specified additional route values, protocol to use, host name and fragment.
        /// </summary>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="values">An object that contains additional route values.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="host">The host name for the URL.</param>
        /// <param name="fragment">The fragment for the URL.</param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(
            this IUrlHelper helper,
            Expression<Action<TController>> action,
            object values,
            string protocol,
            string host,
            string fragment)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.Action(action, new UrlActionContext
            {
                Values = values,
                Protocol = protocol,
                Host = host,
                Fragment = fragment
            });
        }

        private static string Action<TController>(
            this IUrlHelper helper,
            Expression<Action<TController>> action,
            UrlActionContext actionContext)
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action);
            actionContext.Controller = expressionRouteValues.Controller;
            actionContext.Action = expressionRouteValues.Action;

            if (actionContext.Values != null)
            {
                var additionalRouteValues = new RouteValueDictionary(actionContext.Values);

                foreach (var additionalRouteValue in additionalRouteValues)
                {
                    expressionRouteValues.RouteValues[additionalRouteValue.Key] = additionalRouteValue.Value;
                }
            }

            actionContext.Values = expressionRouteValues.RouteValues;

            return helper.Action(actionContext);
        }
    }
}
