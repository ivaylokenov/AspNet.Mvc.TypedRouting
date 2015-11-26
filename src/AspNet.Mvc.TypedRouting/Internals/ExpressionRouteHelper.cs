namespace AspNet.Mvc.TypedRouting.Internals
{
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Mvc.Controllers;
    using Microsoft.AspNet.Mvc.Infrastructure;
    using Microsoft.AspNet.Routing;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ExpressionRouteHelper
    {
        private static readonly ConcurrentDictionary<MethodInfo, ControllerActionDescriptor> controllerActionDescriptorCache =
            new ConcurrentDictionary<MethodInfo, ControllerActionDescriptor>();

        private static IActionDescriptorsCollectionProvider lastUsedActionDescriptorsCollectionProvider = null;
        
        public static IServiceProvider ServiceProvider { get; set; }

        public static ExpressionRouteValues Resolve<TController>(
            Expression<Action<TController>> expression,
            object additionalRouteValues = null,
            bool addControllerAndActionToRouteValues = false)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var actionDescriptorsCollectionProvider = ServiceProvider.GetService(typeof(IActionDescriptorsCollectionProvider)) as IActionDescriptorsCollectionProvider;

            if (actionDescriptorsCollectionProvider == null)
            {
                throw new ArgumentNullException(nameof(actionDescriptorsCollectionProvider));
            }

            if (lastUsedActionDescriptorsCollectionProvider != actionDescriptorsCollectionProvider)
            {
                lastUsedActionDescriptorsCollectionProvider = actionDescriptorsCollectionProvider;
                controllerActionDescriptorCache.Clear();
            }

            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression != null)
            {
                var controllerType = methodCallExpression.Object?.Type;
                if (controllerType == null)
                {
                    // Method call is not valid because it is static.
                    throw new InvalidOperationException("Expression is not valid - expected instance method call but instead received static method call.");
                }

                var methodInfo = methodCallExpression.Method;

                // Find controller action descriptor from the provider with the same extracted method info.
                // This search is potentially slow, so it is cached after the first lookup.
                var controllerActionDescriptor = GetActionDescriptorFromCache(methodInfo, actionDescriptorsCollectionProvider);

                var controllerName = controllerActionDescriptor.ControllerName;
                var actionName = controllerActionDescriptor.Name;

                var routeValues = GetAdditionalRouteValues(methodInfo, methodCallExpression, controllerActionDescriptor, additionalRouteValues);

                // If there is a route constraint with specific expected value, add it to the result.
                var routeConstraints = controllerActionDescriptor.RouteConstraints;
                foreach (var routeConstraint in routeConstraints)
                {
                    var routeKey = routeConstraint.RouteKey;
                    var routeValue = routeConstraint.RouteValue;

                    if (routeValue != string.Empty)
                    {
                        // Override the 'default' values.
                        if (string.Equals(routeKey, "controller", StringComparison.OrdinalIgnoreCase))
                        {
                            controllerName = routeValue;
                        }
                        else if (string.Equals(routeKey, "action", StringComparison.OrdinalIgnoreCase))
                        {
                            actionName = routeValue;
                        }
                        else
                        {
                            routeValues[routeConstraint.RouteKey] = routeValue;
                        }
                    }
                }

                if (addControllerAndActionToRouteValues)
                {
                    routeValues["controller"] = controllerName;
                    routeValues["action"] = actionName;
                }

                return new ExpressionRouteValues
                {
                    Controller = controllerName,
                    Action = actionName,
                    RouteValues = routeValues
                };
            }

            // Expression is invalid because it is not a method call.
            throw new InvalidOperationException("Expression is not valid - expected instance method call but instead received other type of expression.");
        }

        private static ControllerActionDescriptor GetActionDescriptorFromCache(
            MethodInfo methodInfo,
            IActionDescriptorsCollectionProvider actionDescriptorsCollectionProvider)
        {
            return controllerActionDescriptorCache.GetOrAdd(methodInfo, _ =>
            {
                // we are only interested in controller actions
                var foundControllerActionDescriptor = actionDescriptorsCollectionProvider
                    .ActionDescriptors
                    .Items
                    .OfType<ControllerActionDescriptor>()
                    .FirstOrDefault(ca => ca.MethodInfo == methodInfo);

                if (foundControllerActionDescriptor == null)
                {
                    throw new InvalidOperationException($"Method {methodInfo.Name} in class {methodInfo.DeclaringType.Name} is not a valid controller action.");
                }

                return foundControllerActionDescriptor;
            });
        }

        private static IDictionary<string, object> GetAdditionalRouteValues(
            MethodInfo methodInfo,
            MethodCallExpression methodCallExpression,
            ControllerActionDescriptor controllerActionDescriptor,
            object routeValues)
        {
            var parameterDescriptors = controllerActionDescriptor
                    .Parameters
                    .Where(p => p.BindingInfo != null)
                    .ToDictionary(p => p.Name, p => p.BindingInfo.BinderModelName);

            var arguments = methodCallExpression.Arguments.ToArray();
            var methodParameterNames = methodInfo.GetParameters().Select(p => p.Name).ToArray();

            var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            for (var i = 0; i < arguments.Length; i++)
            {
                var methodParameterName = methodParameterNames[i];
                if (parameterDescriptors.ContainsKey(methodParameterName))
                {
                    methodParameterName = parameterDescriptors[methodParameterName];
                }

                var expressionArgument = arguments[i];

                if (expressionArgument.NodeType == ExpressionType.Call)
                {
                    // Expression of type c => c.Action(With.No<int>()) - value should be ignored and can be skipped.
                    var expressionArgumentAsMethodCall = (MethodCallExpression)expressionArgument;
                    if (expressionArgumentAsMethodCall.Object == null
                        && expressionArgumentAsMethodCall.Method.DeclaringType == typeof(With))
                    {
                        continue;
                    }
                }

                object value = null;
                if (expressionArgument.NodeType == ExpressionType.Constant)
                {
                    // Expression of type c => c.Action({const}) - value can be extracted without compiling.
                    value = ((ConstantExpression)expressionArgument).Value;
                }
                else
                {
                    // Expresion needs compiling because it is not of constant type.
                    var convertExpression = Expression.Convert(expressionArgument, typeof(object));
                    value = Expression.Lambda<Func<object>>(convertExpression).Compile().Invoke();
                }

                // We are interested only in not null route values.
                if (value != null)
                {
                    result[methodParameterName] = value;
                }
            }

            if (routeValues != null)
            {
                var additionalRouteValues = new RouteValueDictionary(routeValues);

                foreach (var additionalRouteValue in additionalRouteValues)
                {
                    result[additionalRouteValue.Key] = additionalRouteValue.Value;
                }
            }

            return result;
        }
    }
}
