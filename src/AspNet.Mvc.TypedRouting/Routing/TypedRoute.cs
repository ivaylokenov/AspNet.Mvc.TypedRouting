namespace AspNet.Mvc.TypedRouting.Routing
{
    using Microsoft.AspNetCore.Mvc.ActionConstraints;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    // http://www.strathweb.com/2015/03/strongly-typed-routing-asp-net-mvc-6-iapplicationmodelconvention/
    public class TypedRoute : AttributeRouteModel, ITypedRoute, ITypedRouteDetails
    {
        internal TypedRoute(string template, string[] httpMethods)
        {
            Template = template;
            HttpMethods = httpMethods ?? new string[0];
            Constraints = new List<IActionConstraintMetadata>();
        }

        internal TypeInfo ControllerType { get; private set; }

        internal MethodInfo ActionMember { get; private set; }

        internal IEnumerable<string> HttpMethods { get; private set; }

        internal List<IActionConstraintMetadata> Constraints { get; private set; }

        public ITypedRouteDetails ToController<TController>()
            where TController : class
        {
            ControllerType = typeof(TController).GetTypeInfo();
            return this;
        }

        public ITypedRouteDetails ToAction<TController>(Expression<Action<TController>> expression)
            where TController : class
        {
            return ProcessAction(expression);
        }

        public ITypedRouteDetails ToAction<TController>(Expression<Func<TController, Task>> expression)
            where TController : class
        {
            return ProcessAction(expression);
        }

        public ITypedRouteDetails WithName(string name)
        {
            Name = name;
            return this;
        }

        public ITypedRouteDetails ForHttpMethods(params string[] methods)
        {
            Constraints.Add(new HttpMethodActionConstraint(methods));
            return this;
        }

        public ITypedRouteDetails WithActionConstraints(params IActionConstraintMetadata[] constraints)
        {
            Constraints.AddRange(constraints);
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

        private ITypedRouteDetails ProcessAction(LambdaExpression expression)
        {
            ActionMember = GetMethodInfo(expression);
            ControllerType = ActionMember.DeclaringType.GetTypeInfo();
            return this;
        }
    }
}
