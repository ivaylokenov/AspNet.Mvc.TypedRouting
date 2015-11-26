namespace AspNet.Mvc.TypedRouting.Test.Internals.Test
{
    using TypedRouting.Internals;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Mvc.Abstractions;
    using Microsoft.AspNet.Mvc.ApplicationModels;
    using Microsoft.AspNet.Mvc.Controllers;
    using Microsoft.AspNet.Mvc.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Xunit;
    using Microsoft.AspNet.Mvc.ModelBinding;
    using System.ComponentModel.Design;

    public class ExpressionRouteHelperTest
    {
        [Theory]
        [MemberData(nameof(NormalActionsWithNoParametersData))]
        public void Resolve_ControllerAndActionWithoutParameters_ControllerAndActionNameAreResolved(
            Expression<Action<NormalController>> action, string controllerName, string actionName)
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();

            // Act
            var result = ExpressionRouteHelper.Resolve(action);

            // Assert
            Assert.Equal(controllerName, result.Controller);
            Assert.Equal(actionName, result.Action);
            Assert.Empty(result.RouteValues);
        }

        [Theory]
        [MemberData(nameof(NormalActionssWithParametersData))]
        public void Resolve_ControllerAndActionWithPrimitiveParameters_ControllerActionNameAndParametersAreResolved(
            Expression<Action<NormalController>> action, string controllerName, string actionName, IDictionary<string, object> routeValues)
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();

            // Act
            var result = ExpressionRouteHelper.Resolve(action);

            // Assert
            Assert.Equal(controllerName, result.Controller);
            Assert.Equal(actionName, result.Action);
            Assert.Equal(routeValues.Count, result.RouteValues.Count);

            foreach (var routeValue in routeValues)
            {
                Assert.True(result.RouteValues.ContainsKey(routeValue.Key));
                Assert.Equal(routeValue.Value, result.RouteValues[routeValue.Key]);
            }
        }

        [Fact]
        public void Resolve_ControllerAndActionWithObjectParameters_ControllerActionNameAndParametersAreResolved()
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();

            // Act
            var result = ExpressionRouteHelper.Resolve<NormalController>(c => c.ActionWithMultipleParameters(1, "string", new RequestModel { Integer = 1, String = "Text" }));

            // Assert
            Assert.Equal("Normal", result.Controller);
            Assert.Equal("ActionWithMultipleParameters", result.Action);
            Assert.Equal(3, result.RouteValues.Count);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal("string", result.RouteValues["text"]);
            Assert.IsAssignableFrom<RequestModel>(result.RouteValues["model"]);

            var model = (RequestModel)result.RouteValues["model"];
            Assert.Equal(1, model.Integer);
            Assert.Equal("Text", model.String);
        }

        [Fact]
        public void Resolve_PocoController_ControllerActionNameAndParametersAreResolved()
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();

            // Act
            var result = ExpressionRouteHelper.Resolve<PocoController>(c => c.Action(1));

            // Assert
            Assert.Equal("Poco", result.Controller);
            Assert.Equal("Action", result.Action);
            Assert.Equal(1, result.RouteValues.Count);
            Assert.True(result.RouteValues.ContainsKey("id"));
            Assert.Equal(1, result.RouteValues["id"]);
        }

        [Fact]
        public void Resolve_InAreaController_ControllerActionNameAndAreaAreResolved()
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();

            // Act
            var result = ExpressionRouteHelper.Resolve<InAreaController>(c => c.Action(1));

            // Assert
            Assert.Equal("InArea", result.Controller);
            Assert.Equal("Action", result.Action);
            Assert.Equal(2, result.RouteValues.Count);
            Assert.True(result.RouteValues.ContainsKey("id"));
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.True(result.RouteValues.ContainsKey("area"));
            Assert.Equal("MyArea", result.RouteValues["area"]);
        }

        [Fact]
        public void Resolve_ActionWithCustomRouteConstraints_RouteConstraintsAreResolved()
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();

            // Act
            var result = ExpressionRouteHelper.Resolve<RouteConstraintController>(c => c.Action(1, 2));

            // Assert
            Assert.Equal("CustomController", result.Controller);
            Assert.Equal("CustomAction", result.Action);
            Assert.Equal(3, result.RouteValues.Count);
            Assert.True(result.RouteValues.ContainsKey("id"));
            Assert.Equal("5", result.RouteValues["id"]);
            Assert.True(result.RouteValues.ContainsKey("key"));
            Assert.Equal("value", result.RouteValues["key"]);
            Assert.True(result.RouteValues.ContainsKey("anotherId"));
            Assert.Equal(2, result.RouteValues["anotherId"]);
        }

        [Fact]
        public void Resolve_CustomConventions_CustomConventionsAreResolved()
        {
            // Arrange
            AttachActionDescriptorsCollectionProvider();

            // Act
            var result = ExpressionRouteHelper.Resolve<ConventionsController>(c => c.ConventionsAction(1));

            // Assert
            Assert.Equal("ChangedController", result.Controller);
            Assert.Equal("ChangedAction", result.Action);
            Assert.Equal(1, result.RouteValues.Count);
            Assert.True(result.RouteValues.ContainsKey("ChangedParameter"));
            Assert.Equal(1, result.RouteValues["ChangedParameter"]);
        }

        public static TheoryData<Expression<Action<NormalController>>, string, string> NormalActionsWithNoParametersData
        {
            get
            {
                var data = new TheoryData<Expression<Action<NormalController>>, string, string>();

                const string controllerName = "Normal";
                data.Add(c => c.ActionWithoutParameters(), controllerName, "ActionWithoutParameters");
                data.Add(c => c.ActionWithOverloads(), controllerName, "ActionWithOverloads");
                data.Add(c => c.VoidAction(), controllerName, "VoidAction");
                data.Add(c => c.ActionWithChangedName(), controllerName, "AnotherName");

                return data;
            }
        }

        public static TheoryData<
            Expression<Action<NormalController>>,
            string,
            string,
            IDictionary<string, object>> NormalActionssWithParametersData
        {
            get
            {
                var data = new TheoryData<Expression<Action<NormalController>>, string, string, IDictionary<string, object>>();

                const string controllerName = "Normal";
                data.Add(
                    c => c.ActionWithOverloads(1),
                    controllerName,
                    "ActionWithOverloads",
                    new Dictionary<string, object> {["id"] = 1 });

                data.Add(
                    c => c.ActionWithMultipleParameters(1, "string", null),
                    controllerName,
                    "ActionWithMultipleParameters",
                    new Dictionary<string, object> {["id"] = 1,["text"] = "string" });

                return data;
            }
        }

        private void AttachActionDescriptorsCollectionProvider()
        {
            // Run the full controller and action model building 
            // in order to simulate the default MVC behavior.
            var controllerTypes = typeof(ExpressionRouteHelperTest)
                .GetNestedTypes()
                .Select(t => t.GetTypeInfo())
                .ToList();

            var options = new TestOptionsManager<MvcOptions>();

            var controllerTypeProvider = new StaticControllerTypeProvider(controllerTypes);
            var modelProvider = new DefaultApplicationModelProvider(options);

            var provider = new ControllerActionDescriptorProvider(
                controllerTypeProvider,
                new[] { modelProvider },
                options);

            var serviceContainer = new ServiceContainer();
            var list = new List<IActionDescriptorProvider>()
            {
                provider,
            };

            var actionDescriptorCollectionProvider = new DefaultActionDescriptorsCollectionProvider(serviceContainer);

            serviceContainer.AddService(typeof(IEnumerable<IActionDescriptorProvider>), list);
            serviceContainer.AddService(typeof(IActionDescriptorsCollectionProvider), actionDescriptorCollectionProvider);

            ExpressionRouteHelper.ServiceProvider = serviceContainer;
        }

        public class RequestModel
        {
            public int Integer { get; set; }

            public string String { get; set; }
        }

        public class NormalController : Controller
        {
            public IActionResult ActionWithoutParameters()
            {
                return null;
            }

            public IActionResult ActionWithMultipleParameters(int id, string text, RequestModel model)
            {
                return null;
            }

            public IActionResult ActionWithOverloads()
            {
                return null;
            }

            public IActionResult ActionWithOverloads(int id)
            {
                return null;
            }

            [ActionName("AnotherName")]
            public IActionResult ActionWithChangedName()
            {
                return null;
            }

            public void VoidAction()
            {
            }
        }

        public class PocoController
        {
            public IActionResult Action(int id)
            {
                return null;
            }
        }

        [Area("MyArea")]
        public class InAreaController
        {
            public IActionResult Action(int id)
            {
                return null;
            }
        }

        [MyRouteConstraint("controller", "CustomController")]
        public class RouteConstraintController
        {
            [MyRouteConstraint("action", "CustomAction")]
            [MyRouteConstraint("id", "5")]
            [MyRouteConstraint("key", "value")]
            public IActionResult Action(int id, int anotherId)
            {
                return null;
            }
        }

        [CustomControllerConvention]
        public class ConventionsController
        {
            [CustomActionConvention]
            public IActionResult ConventionsAction([CustomParameterConvention]int id)
            {
                return null;
            }
        }

        public class CustomControllerConventionAttribute : Attribute, IControllerModelConvention
        {
            public void Apply(ControllerModel controller)
            {
                controller.ControllerName = "ChangedController";
            }
        }

        public class CustomActionConventionAttribute : Attribute, IActionModelConvention
        {
            public void Apply(ActionModel action)
            {
                action.ActionName = "ChangedAction";
            }
        }

        public class CustomParameterConventionAttribute : Attribute, IParameterModelConvention
        {
            public void Apply(ParameterModel parameter)
            {
                parameter.BindingInfo = parameter.BindingInfo ?? new BindingInfo();
                parameter.BindingInfo.BinderModelName = "ChangedParameter";
            }
        }

        public class MyRouteConstraintAttribute : RouteConstraintAttribute
        {
            public MyRouteConstraintAttribute(string routeKey, string routeValue)
                : base(routeKey, routeValue, true)
            {
            }
        }
    }
}
