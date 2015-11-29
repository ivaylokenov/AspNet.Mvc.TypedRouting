namespace AspNet.Mvc.TypedRouting.Internals
{
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Mvc.Controllers;
    using Microsoft.AspNet.Mvc.Infrastructure;
    using Microsoft.AspNet.Routing;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ExpressionRouteHelper
    {
        // This key should be ignored as it is used internally for route attribute matching.
        private static readonly string RouteGroupKey = "!__route_group";
        
        private static readonly ConcurrentDictionary<MethodInfo, ControllerActionDescriptor> ControllerActionDescriptorCache =
            new ConcurrentDictionary<MethodInfo, ControllerActionDescriptor>();
        
        private static IActionDescriptorsCollectionProvider actionDescriptorsCollectionProvider;
        
        public static void Initialize(IServiceProvider serviceProvider)
        {
            ClearActionCache();

            actionDescriptorsCollectionProvider = serviceProvider.GetService(typeof(IActionDescriptorsCollectionProvider)) as IActionDescriptorsCollectionProvider;

            if (actionDescriptorsCollectionProvider == null)
            {
                throw new ArgumentNullException(nameof(actionDescriptorsCollectionProvider));
            }
        }

        public static void ClearActionCache()
        {
            ControllerActionDescriptorCache.Clear();
        }
                
        public static ExpressionRouteValues Resolve<TController>(
            Expression<Action<TController>> expression,
            object additionalRouteValues = null,
            bool addControllerAndActionToRouteValues = false)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
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
                var controllerActionDescriptor = GetActionDescriptorFromCache(methodInfo);

                var controllerName = controllerActionDescriptor.ControllerName;
                var actionName = controllerActionDescriptor.Name;
                
                var routeValues = GetRouteValues(methodInfo, methodCallExpression, controllerActionDescriptor);

                // If there is a route constraint with specific expected value, add it to the result.
                var routeConstraints = controllerActionDescriptor.RouteConstraints;
                for (int i = 0; i < routeConstraints.Count; i++)
                {
                    var routeConstraint = routeConstraints[i];
                    var routeKey = routeConstraint.RouteKey;
                    var routeValue = routeConstraint.RouteValue;
                    
                    if (string.Equals(routeKey, RouteGroupKey))
                    {
                        continue;
                    }

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
                
                ApplyAdditionaRouteValues(additionalRouteValues, routeValues);

                if (addControllerAndActionToRouteValues)
                {
                    AddControllerAndActionToRouteValues(controllerName, actionName, routeValues);
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

        private static ControllerActionDescriptor GetActionDescriptorFromCache(MethodInfo methodInfo)
        {
            return ControllerActionDescriptorCache.GetOrAdd(methodInfo, _ =>
            {
                // we are only interested in controller actions
                ControllerActionDescriptor foundControllerActionDescriptor = null;
                var actionDescriptors = actionDescriptorsCollectionProvider.ActionDescriptors.Items;
                for (int i = 0; i < actionDescriptors.Count; i++)
                {
                    var actionDescriptor = actionDescriptors[i];
                    if (actionDescriptor is ControllerActionDescriptor && ((ControllerActionDescriptor)actionDescriptor).MethodInfo == methodInfo)
                    {
                        foundControllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
                        break;
                    }
                }

                if (foundControllerActionDescriptor == null)
                {
                    throw new InvalidOperationException($"Method {methodInfo.Name} in class {methodInfo.DeclaringType.Name} is not a valid controller action.");
                }

                return foundControllerActionDescriptor;
            });
        }

        private static IDictionary<string, object> GetRouteValues(
            MethodInfo methodInfo,
            MethodCallExpression methodCallExpression,
            ControllerActionDescriptor controllerActionDescriptor)
        {
            var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            var arguments = methodCallExpression.Arguments;
            if (arguments.Count == 0)
            {
                return result;
            }

            var methodParameterNames = methodInfo.GetParameters();

            var parameterDescriptors = new Dictionary<string, string>();
            var parameters = controllerActionDescriptor.Parameters;
            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                if (parameter.BindingInfo != null)
                {
                    parameterDescriptors.Add(parameter.Name, parameter.BindingInfo.BinderModelName);
                }
            }

            for (var i = 0; i < arguments.Count; i++)
            {
                var methodParameterName = methodParameterNames[i].Name;
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

                object value;
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

            return result;
        }

        private static void ApplyAdditionaRouteValues(object routeValues, IDictionary<string, object> result)
        {
            if (routeValues != null)
            {
                var additionalRouteValues = new RouteValueDictionary(routeValues);

                foreach (var additionalRouteValue in additionalRouteValues)
                {
                    result[additionalRouteValue.Key] = additionalRouteValue.Value;
                }
            }
        }

        private static void AddControllerAndActionToRouteValues(string controllerName, string actionName, IDictionary<string, object> routeValues)
        {
            routeValues["controller"] = controllerName;
            routeValues["action"] = actionName;
        }
    }
}
