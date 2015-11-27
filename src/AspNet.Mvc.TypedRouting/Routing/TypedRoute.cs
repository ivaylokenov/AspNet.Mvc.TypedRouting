namespace AspNet.Mvc.TypedRouting.Routing
{
    using Microsoft.AspNet.Mvc.ApplicationModels;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    // http://www.strathweb.com/2015/03/strongly-typed-routing-asp-net-mvc-6-iapplicationmodelconvention/
    public class TypedRoute : AttributeRouteModel
    {
        public TypedRoute(string template)
        {
            Template = template;
            HttpMethods = new string[0];
        }

        internal TypeInfo ControllerType { get; private set; }

        internal MethodInfo ActionMember { get; private set; }

        internal IEnumerable<string> HttpMethods { get; private set; }

        public TypedRoute ToController<TController>()
        {
            ControllerType = typeof(TController).GetTypeInfo();
            return this;
        }

        public TypedRoute ToAction<TController>(Expression<Action<TController>> expression)
        {
            ActionMember = GetMethodInfo(expression);
            ControllerType = ActionMember.DeclaringType.GetTypeInfo();
            return this;
        }

        public TypedRoute WithName(string name)
        {
            Name = name;
            return this;
        }

        public TypedRoute ForHttpMethods(params string[] methods)
        {
            HttpMethods = methods;
            return this;
        }

        private static MethodInfo GetMethodInfo(LambdaExpression expression)
        {
            var method = expression.Body as MethodCallExpression;
            if (method == null)
            {
                throw new InvalidOperationException("Expression is not valid - expected instance method call but instead received other type of expression.");
            }

            return method.Method;
        }
    }
}
