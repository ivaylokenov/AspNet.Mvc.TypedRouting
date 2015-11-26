using AspNet.Mvc.TypedRouting.Internals;

namespace Microsoft.AspNet.Mvc
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
    }
}
