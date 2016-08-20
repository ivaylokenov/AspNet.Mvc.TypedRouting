namespace Microsoft.AspNetCore.Mvc
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Routing;
    using AspNet.Mvc.TypedRouting.LinkGeneration;

    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(this IUrlHelper helper, Expression<Action<TController>> action)
            where TController : class
        {
            return helper.Action(action, values: null, protocol: null, host: null, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(this IUrlHelper helper, Expression<Func<TController, Task>> action)
            where TController : class
        {
            return helper.Action(action, values: null, protocol: null, host: null, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved ,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="values">An object that contains additional route values.</param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(
            this IUrlHelper helper,
            Expression<Action<TController>> action,
            object values)
            where TController : class
        {
            return helper.Action(action, values, protocol: null, host: null, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved ,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="values">An object that contains additional route values.</param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(
            this IUrlHelper helper,
            Expression<Func<TController, Task>> action,
            object values)
            where TController : class
        {
            return helper.Action(action, values, protocol: null, host: null, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values and protocol to use.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
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
            where TController : class
        {
            return helper.Action(action, values, protocol, host: null, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values and protocol to use.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="values">An object that contains additional route values.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <returns>The fully qualified or absolute URL to an action method.</returns>
        public static string Action<TController>(
            this IUrlHelper helper,
            Expression<Func<TController, Task>> action,
            object values,
            string protocol)
            where TController : class
        {
            return helper.Action(action, values, protocol, host: null, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values, protocol to use and host name.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
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
            where TController : class
        {
            return helper.Action(action, values, protocol, host, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values, protocol to use and host name.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
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
            Expression<Func<TController, Task>> action,
            object values,
            string protocol,
            string host)
            where TController : class
        {
            return helper.Action(action, values, protocol, host, fragment: null);
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values, protocol to use, host name and fragment.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
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
            where TController : class
        {
            return helper.Action(action, new UrlActionContext
            {
                Values = values,
                Protocol = protocol,
                Host = host,
                Fragment = fragment
            });
        }

        /// <summary>
        /// Generates a fully qualified or absolute URL for an action method by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values, protocol to use, host name and fragment.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
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
            Expression<Func<TController, Task>> action,
            object values,
            string protocol,
            string host,
            string fragment)
            where TController : class
        {
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
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
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
            where TController : class
        {
            return helper.Link(routeName, action, values: null);
        }

        /// <summary>
        /// Generates an absolute URL using the specified route name and <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route that is used to generate the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The generated absolute URL.</returns>
        public static string Link<TController>(
            this IUrlHelper helper,
            string routeName,
            Expression<Func<TController, Task>> action)
            where TController : class
        {
            return helper.Link(routeName, action, values: null);
        }

        /// <summary>
        /// Generates an absolute URL using the specified route name, <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved, and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
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
            where TController : class
        {
            var expressionRouteValues = GetExpresionRouteHelper(helper).Resolve(action, values, addControllerAndActionToRouteValues: true);
            return helper.Link(routeName, expressionRouteValues.RouteValues);
        }

        /// <summary>
        /// Generates an absolute URL using the specified route name, <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved, and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
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
            Expression<Func<TController, Task>> action,
            object values)
            where TController : class
        {
            var expressionRouteValues = GetExpresionRouteHelper(helper).Resolve(action, values, addControllerAndActionToRouteValues: true);
            return helper.Link(routeName, expressionRouteValues.RouteValues);
        }

        private static string Action<TController>(
            this IUrlHelper helper,
            Expression<Action<TController>> action,
            UrlActionContext actionContext)
            where TController : class
        {
            var expressionRouteValues = GetExpresionRouteHelper(helper).Resolve(action, actionContext.Values);
            ApplyRouteValues(actionContext, expressionRouteValues);
            return helper.Action(actionContext);
        }
        
        private static string Action<TController>(
            this IUrlHelper helper,
            Expression<Func<TController, Task>> action,
            UrlActionContext actionContext)
            where TController : class
        {
            var expressionRouteValues = GetExpresionRouteHelper(helper).Resolve(action, actionContext.Values);
            ApplyRouteValues(actionContext, expressionRouteValues);
            return helper.Action(actionContext);
        }

        private static void ApplyRouteValues(UrlActionContext actionContext, ExpressionRouteValues expressionRouteValues)
        {
            actionContext.Controller = expressionRouteValues.Controller;
            actionContext.Action = expressionRouteValues.Action;
            actionContext.Values = expressionRouteValues.RouteValues;
        }

        private static IExpressionRouteHelper GetExpresionRouteHelper(IUrlHelper helper)
            => helper.ActionContext.HttpContext.RequestServices.GetExpressionRouteHelper();
    }
}
