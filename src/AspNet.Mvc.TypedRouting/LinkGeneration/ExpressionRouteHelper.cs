namespace AspNet.Mvc.TypedRouting.LinkGeneration
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Routing;

    public class ExpressionRouteHelper : IExpressionRouteHelper
    {
        // This key should be ignored as it is used internally for route attribute matching.
        private const string RouteGroupKey = "!__route_group";

        private readonly ConcurrentDictionary<MethodInfo, ControllerActionDescriptor> controllerActionDescriptorCache;
        private readonly IActionDescriptorCollectionProvider actionDescriptorsCollection;
        private readonly ISet<string> uniqueRouteKeys;

        public ExpressionRouteHelper(
            IActionDescriptorCollectionProvider actionDescriptorsCollectionProvider,
            IUniqueRouteKeysProvider uniqueRouteKeysProvider)
        {
            controllerActionDescriptorCache = new ConcurrentDictionary<MethodInfo, ControllerActionDescriptor>();

            uniqueRouteKeys = uniqueRouteKeysProvider.GetUniqueKeys();
            actionDescriptorsCollection = actionDescriptorsCollectionProvider;
        }
        
        public ExpressionRouteValues Resolve<TController>(
            Expression<Action<TController>> expression,
            object additionalRouteValues = null,
            bool addControllerAndActionToRouteValues = false)
        {
            return ResolveLambdaExpression(
                expression,
                additionalRouteValues,
                addControllerAndActionToRouteValues);
        }

        public ExpressionRouteValues Resolve<TController>(
            Expression<Func<TController, Task>> expression,
            object additionalRouteValues = null,
            bool addControllerAndActionToRouteValues = false)
        {
            return ResolveLambdaExpression(
                expression,
                additionalRouteValues,
                addControllerAndActionToRouteValues);
        }

        public void ClearActionCache()
        {
            controllerActionDescriptorCache.Clear();
        }

        private ExpressionRouteValues ResolveLambdaExpression(
            LambdaExpression expression,
            object additionalRouteValues,
            bool addControllerAndActionToRouteValues)
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
                var actionName = controllerActionDescriptor.ActionName;
                
                var routeValues = GetRouteValues(methodInfo, methodCallExpression, controllerActionDescriptor);
                
                // If there is a required route value, add it to the result.
                foreach (var requiredRouteValue in controllerActionDescriptor.RouteValues)
                {
                    var routeKey = requiredRouteValue.Key;
                    var routeValue = requiredRouteValue.Value;

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
                            routeValues[routeKey] = routeValue;
                        }
                    }
                }
                
                ApplyAdditionalRouteValues(additionalRouteValues, routeValues);

                if (addControllerAndActionToRouteValues)
                {
                    AddControllerAndActionToRouteValues(controllerName, actionName, routeValues);
                }

                foreach (var uniqueRouteKey in uniqueRouteKeys)
                {
                    if (!routeValues.ContainsKey(uniqueRouteKey))
                    {
                        routeValues.Add(uniqueRouteKey, string.Empty);
                    }
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

        private ControllerActionDescriptor GetActionDescriptorFromCache(MethodInfo methodInfo)
        {
            return controllerActionDescriptorCache.GetOrAdd(methodInfo, _ =>
            {
                // we are only interested in controller actions
                ControllerActionDescriptor foundControllerActionDescriptor = null;
                var actionDescriptors = actionDescriptorsCollection.ActionDescriptors.Items;
                for (int i = 0; i < actionDescriptors.Count; i++)
                {
                    var actionDescriptor = actionDescriptors[i];
                    var descriptor = actionDescriptor as ControllerActionDescriptor;
                    if (descriptor != null && descriptor.MethodInfo == methodInfo)
                    {
                        foundControllerActionDescriptor = descriptor;
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

        private IDictionary<string, object> GetRouteValues(
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
                    methodParameterName = parameterDescriptors[methodParameterName] ?? methodParameterName;
                }

                var expressionArgument = arguments[i];

                if (expressionArgument.NodeType == ExpressionType.Convert)
                {
                    // Expression which contains converting from type to type
                    var expressionArgumentAsUnary = (UnaryExpression)expressionArgument;
                    expressionArgument = expressionArgumentAsUnary.Operand;
                }

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
                    // Expression needs compiling because it is not of constant type.
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

        private static void ApplyAdditionalRouteValues(object routeValues, IDictionary<string, object> result)
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
