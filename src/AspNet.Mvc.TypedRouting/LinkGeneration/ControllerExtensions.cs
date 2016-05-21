using AspNet.Mvc.TypedRouting.Internals;

namespace Microsoft.AspNetCore.Mvc
{
    using System;
    using System.Linq.Expressions;

    public static class ControllerExtensions
    {
        /// <summary>
        /// Creates a <see cref="CreatedAtActionResult"/> object that produces a Created (201) response
        /// by using <see cref="Expression{TDelegate}"/> for selecting the action.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created <see cref="CreatedAtActionResult"/> for the response.</returns>
        public static CreatedAtActionResult CreatedAtAction<TController>(
            this TController controller,
            Expression<Action<TController>> action,
            object value)
            where TController : Controller
        {
            return controller.CreatedAtAction(action, routeValues: null, value: value);
        }

        /// <summary>
        /// Creates a <see cref="CreatedAtActionResult"/> object that produces a Created (201) response
        /// by using <see cref="Expression{TDelegate}"/> for selecting the action.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created <see cref="CreatedAtActionResult"/> for the response.</returns>
        public static CreatedAtActionResult CreatedAtAction<TController>(
            this TController controller,
            Expression<Action<TController>> action,
            object routeValues,
            object value)
            where TController : Controller
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues);
            return controller.CreatedAtAction(
                expressionRouteValues.Action,
                expressionRouteValues.RouteValues,
                value);
        }

        /// <summary>
        /// Creates a <see cref="CreatedAtActionResult"/> object that produces a Created (201) response
        /// by using <see cref="Expression{TDelegate}"/> for selecting the action.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created <see cref="CreatedAtActionResult"/> for the response.</returns>
        public static CreatedAtActionResult CreatedAtAction<TRedirectController>(
            this Controller controller,
            Expression<Action<TRedirectController>> action,
            object value)
        {
            return controller.CreatedAtAction(action, routeValues: null, value: value);
        }

        /// <summary>
        /// Creates a <see cref="CreatedAtActionResult"/> object that produces a Created (201) response
        /// by using <see cref="Expression{TDelegate}"/> for selecting the action.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created <see cref="CreatedAtActionResult"/> for the response.</returns>
        public static CreatedAtActionResult CreatedAtAction<TRedirectController>(
            this Controller controller,
            Expression<Action<TRedirectController>> action,
            object routeValues,
            object value)
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues);
            return controller.CreatedAtAction(
                expressionRouteValues.Action,
                expressionRouteValues.Controller,
                routeValues,
                value);
        }

        /// <summary>
        /// Creates a <see cref="CreatedAtRouteResult"/> object that produces a Created (201) response.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created <see cref="CreatedAtRouteResult"/> for the response</returns>
        public static CreatedAtRouteResult CreatedAtRoute<TController>(
            this TController controller,
            string routeName,
            Expression<Action<TController>> action,
            object value)
            where TController : Controller
        {
            return controller.CreatedAtRoute(routeName, action, routeValues: null, value: value);
        }

        /// <summary>
        /// Creates a <see cref="CreatedAtRouteResult"/> object that produces a Created (201) response.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created <see cref="CreatedAtRouteResult"/> for the response</returns>
        public static CreatedAtRouteResult CreatedAtRoute<TController>(
            this TController controller,
            string routeName,
            Expression<Action<TController>> action,
            object routeValues,
            object value)
            where TController : Controller
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues, addControllerAndActionToRouteValues : true);
            return controller.CreatedAtRoute(
                routeName,
                expressionRouteValues.RouteValues,
                value);
        }

        /// <summary>
        /// Creates a <see cref="CreatedAtRouteResult"/> object that produces a Created (201) response.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created <see cref="CreatedAtRouteResult"/> for the response</returns>
        public static CreatedAtRouteResult CreatedAtRoute<TRedirectController>(
            this Controller controller,
            string routeName,
            Expression<Action<TRedirectController>> action,
            object value)
        {
            return controller.CreatedAtRoute(routeName, action, routeValues: null, value: value);
        }

        /// <summary>
        /// Creates a <see cref="CreatedAtRouteResult"/> object that produces a Created (201) response.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created <see cref="CreatedAtRouteResult"/> for the response</returns>
        public static CreatedAtRouteResult CreatedAtRoute<TRedirectController>(
            this Controller controller,
            string routeName,
            Expression<Action<TRedirectController>> action,
            object routeValues,
            object value)
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues, addControllerAndActionToRouteValues: true);
            return controller.CreatedAtRoute(
                routeName,
                expressionRouteValues.RouteValues,
                value);
        }

        /// <summary>
        /// Redirects to the specified action by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The created <see cref="RedirectToActionResult"/> for the response.</returns>
        public static RedirectToActionResult RedirectToAction<TController>(
            this TController controller,
            Expression<Action<TController>> action)
            where TController : Controller
        {
            return controller.RedirectToAction(action, routeValues: null);
        }

        /// <summary>
        /// Redirects to the specified action by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <returns>The created <see cref="RedirectToActionResult"/> for the response.</returns>
        public static RedirectToActionResult RedirectToAction<TController>(
            this TController controller,
            Expression<Action<TController>> action,
            object routeValues)
            where TController : Controller
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues);
            return controller.RedirectToAction(
                expressionRouteValues.Action,
                expressionRouteValues.RouteValues);
        }

        /// <summary>
        /// Redirects to the specified action by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The created <see cref="RedirectToActionResult"/> for the response.</returns>
        public static RedirectToActionResult RedirectToAction<TRedirectController>(
            this Controller controller,
            Expression<Action<TRedirectController>> action)
        {
            return controller.RedirectToAction(action, routeValues: null);
        }

        /// <summary>
        /// Redirects to the specified action by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <returns>The created <see cref="RedirectToActionResult"/> for the response.</returns>
        public static RedirectToActionResult RedirectToAction<TRedirectController>(
            this Controller controller,
            Expression<Action<TRedirectController>> action,
            object routeValues)
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues);
            return controller.RedirectToAction(
                expressionRouteValues.Action,
                expressionRouteValues.Controller,
                routeValues);
        }

        /// <summary>
        /// Redirects to the specified action with <see cref="RedirectToActionResult.Permanent"/> set to true by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The created <see cref="RedirectToActionResult"/> for the response.</returns>
        public static RedirectToActionResult RedirectToActionPermanent<TController>(
            this TController controller,
            Expression<Action<TController>> action)
            where TController : Controller
        {
            return controller.RedirectToActionPermanent(action, routeValues: null);
        }

        /// <summary>
        /// Redirects to the specified action with <see cref="RedirectToActionResult.Permanent"/> set to true by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <returns>The created <see cref="RedirectToActionResult"/> for the response.</returns>
        public static RedirectToActionResult RedirectToActionPermanent<TController>(
            this TController controller,
            Expression<Action<TController>> action,
            object routeValues)
            where TController : Controller
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues);
            return controller.RedirectToActionPermanent(
                expressionRouteValues.Action,
                expressionRouteValues.RouteValues);
        }

        /// <summary>
        /// Redirects to the specified action with <see cref="RedirectToActionResult.Permanent"/> set to true by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The created <see cref="RedirectToActionResult"/> for the response.</returns>
        public static RedirectToActionResult RedirectToActionPermanent<TRedirectController>(
            this Controller controller,
            Expression<Action<TRedirectController>> action)
        {
            return controller.RedirectToActionPermanent(action, routeValues: null);
        }

        /// <summary>
        /// Redirects to the specified action with <see cref="RedirectToActionResult.Permanent"/> set to true by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <returns>The created <see cref="RedirectToActionResult"/> for the response.</returns>
        public static RedirectToActionResult RedirectToActionPermanent<TRedirectController>(
            this Controller controller,
            Expression<Action<TRedirectController>> action,
            object routeValues)
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues);
            return controller.RedirectToActionPermanent(
                expressionRouteValues.Action,
                expressionRouteValues.Controller,
                routeValues);
        }

        /// <summary>
        /// Redirects to the specified route by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The created <see cref="RedirectToRouteResult"/> for the response.</returns>
        public static RedirectToRouteResult RedirectToRoute<TController>(
            this TController controller,
            string routeName,
            Expression<Action<TController>> action)
            where TController : Controller
        {
            return controller.RedirectToRoute(routeName, action, routeValues: null);
        }

        /// <summary>
        /// Redirects to the specified route by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <returns>The created <see cref="RedirectToRouteResult"/> for the response.</returns>
        public static RedirectToRouteResult RedirectToRoute<TController>(
            this TController controller,
            string routeName,
            Expression<Action<TController>> action,
            object routeValues)
            where TController : Controller
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues, addControllerAndActionToRouteValues: true);
            return controller.RedirectToRoute(
                routeName,
                expressionRouteValues.RouteValues);
        }

        /// <summary>
        /// Redirects to the specified route by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The created <see cref="RedirectToRouteResult"/> for the response.</returns>
        public static RedirectToRouteResult RedirectToRoute<TRedirectController>(
            this Controller controller,
            string routeName,
            Expression<Action<TRedirectController>> action)
        {
            return controller.RedirectToRoute(routeName, action, routeValues: null);
        }

        /// <summary>
        /// Redirects to the specified route by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <returns>The created <see cref="RedirectToRouteResult"/> for the response.</returns>
        public static RedirectToRouteResult RedirectToRoute<TRedirectController>(
            this Controller controller,
            string routeName,
            Expression<Action<TRedirectController>> action,
            object routeValues)
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues, addControllerAndActionToRouteValues: true);
            return controller.RedirectToRoute(
                routeName,
                expressionRouteValues.RouteValues);
        }

        /// <summary>
        /// Redirects to the specified route with <see cref="RedirectToActionResult.Permanent"/> set to true by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The created <see cref="RedirectToRouteResult"/> for the response.</returns>
        public static RedirectToRouteResult RedirectToRoutePermanent<TController>(
            this TController controller,
            string routeName,
            Expression<Action<TController>> action)
            where TController : Controller
        {
            return controller.RedirectToRoutePermanent(routeName, action, routeValues: null);
        }

        /// <summary>
        /// Redirects to the specified route with <see cref="RedirectToActionResult.Permanent"/> set to true by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <returns>The created <see cref="RedirectToRouteResult"/> for the response.</returns>
        public static RedirectToRouteResult RedirectToRoutePermanent<TController>(
            this TController controller,
            string routeName,
            Expression<Action<TController>> action,
            object routeValues)
            where TController : Controller
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues, addControllerAndActionToRouteValues: true);
            return controller.RedirectToRoutePermanent(
                routeName,
                expressionRouteValues.RouteValues);
        }

        /// <summary>
        /// Redirects to the specified route with <see cref="RedirectToActionResult.Permanent"/> set to true by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <returns>The created <see cref="RedirectToRouteResult"/> for the response.</returns>
        public static RedirectToRouteResult RedirectToRoutePermanent<TRedirectController>(
            this Controller controller,
            string routeName,
            Expression<Action<TRedirectController>> action)
        {
            return controller.RedirectToRoutePermanent(routeName, action, routeValues: null);
        }

        /// <summary>
        /// Redirects to the specified route with <see cref="RedirectToActionResult.Permanent"/> set to true by 
        /// using <see cref="Expression{TDelegate}"/> for an action method,
        /// from which action name, controller name and route values are resolved,
        /// and the specified additional route values.
        /// </summary>
        /// <typeparam name="TRedirectController">Controller, from which the action is specified.</typeparam>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="action">
        /// The <see cref="Expression{TDelegate}"/>, from which action name, 
        /// controller name and route values are resolved.
        /// </param>
        /// <param name="routeValues">Additional route data to use for generating the URL.</param>
        /// <returns>The created <see cref="RedirectToRouteResult"/> for the response.</returns>
        public static RedirectToRouteResult RedirectToRoutePermanent<TRedirectController>(
            this Controller controller,
            string routeName,
            Expression<Action<TRedirectController>> action,
            object routeValues)
        {
            var expressionRouteValues = ExpressionRouteHelper.Resolve(action, routeValues, addControllerAndActionToRouteValues: true);
            return controller.RedirectToRoutePermanent(
                routeName,
                expressionRouteValues.RouteValues);
        }
    }
}
