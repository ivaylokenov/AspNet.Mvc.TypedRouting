using AspNet.Mvc.TypedRouting.Internals;

namespace Microsoft.AspNet.Mvc
{
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
        /// <typeparam name="TController">Controller, from which the action is specified.</param>
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
        /// from which action name, controller name and route values are resolved ,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</param>
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
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values and protocol to use.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</param>
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
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values, protocol to use and host name.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</param>
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
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values, protocol to use, host name and fragment.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</param>
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

        /// <summary>
        /// Generates an absolute URL using the specified route name and <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</param>
        /// <param name="routeName">The name of the route that is used to generate the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The generated absolute URL.</returns>
        public static string Link<TController>(
            this IUrlHelper helper,
            string routeName,
            Expression<Action<TController>> action)
        {
            return helper.Link(routeName, action, values: null);
        }

        /// <summary>
        /// Generates an absolute URL using the specified route name, <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved, and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</param>
        /// <param name="routeName">The name of the route that is used to generate the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="values">An object that contains additional route values.</param>
        /// <returns>The generated absolute URL.</returns>
        public static string Link<TController>(
            this IUrlHelper helper,
            string routeName,
            Expression<Action<TController>> action,
            object values)
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, values, addControllerAndActionToRouteValues: true);
            return helper.Link(routeName, expressionRouteValues.RouteValues);
        }

        private static string Action<TController>(
            this IUrlHelper helper,
            Expression<Action<TController>> action,
            UrlActionContext actionContext)
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, actionContext.Values);

            actionContext.Controller = expressionRouteValues.Controller;
            actionContext.Action = expressionRouteValues.Action;
            actionContext.Values = expressionRouteValues.RouteValues;

            return helper.Action(actionContext);
        }
    }
}
