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
    
    public class TypedRoute : AttributeRouteModel, ITypedRoute, ITypedRouteDetails
    {
        internal TypedRoute(string template, string[] httpMethods)
        {
            Template = template;
            Constraints = new List<IActionConstraintMetadata>();

            if (httpMethods != null && httpMethods.Length > 0)
            {
                Constraints.Add(new HttpMethodActionConstraint(httpMethods));
            }
        }

        internal TypeInfo ControllerType { get; private set; }

        internal MethodInfo ActionMember { get; private set; }
        
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

        public ITypedRouteDetails ForHttpMethod(string method)
        {
            return this.ForHttpMethods(method);
        }

        public ITypedRouteDetails ForHttpMethods(params string[] methods)
        {
            Constraints.Add(new HttpMethodActionConstraint(methods));
            return this;
        }

        public ITypedRouteDetails WithActionConstraint(IActionConstraintMetadata constraint)
        {
            return this.WithActionConstraints(constraint);
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
