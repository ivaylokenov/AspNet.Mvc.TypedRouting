namespace AspNet.Mvc.TypedRouting.Test.LinkGeneration
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    [Collection("TypedRoutingTests")]
    public class ControllerExtensionsTest
    {
        [Fact]
        public void CreatedAtAction_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.CreatedAtActionSameController() as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(null, result.ControllerName);
            Assert.Equal("CreatedAtActionSameController", result.ActionName);
            Assert.Empty(result.RouteValues);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtActionWithRouteValues_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.CreatedAtActionSameControllerRouteValues() as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(null, result.ControllerName);
            Assert.Equal("CreatedAtActionSameControllerRouteValues", result.ActionName);
            Assert.Equal(1, result.RouteValues.Count);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtAction_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.CreatedAtActionOtherController() as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Other", result.ControllerName);
            Assert.Equal("Action", result.ActionName);
            Assert.Null(result.RouteValues);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtActionWithRouteValues_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.CreatedAtActionOtherControllerRouteValues() as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Other", result.ControllerName);
            Assert.Equal("Action", result.ActionName);
            Assert.Equal(1, result.RouteValues.Count);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtRoute_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.CreatedAtRouteSameController() as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(2, result.RouteValues.Count);
            Assert.Equal("MyTest", result.RouteValues["controller"]);
            Assert.Equal("CreatedAtRouteSameController", result.RouteValues["action"]);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtRouteWithRouteValues_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.CreatedAtRouteSameControllerRouteValues() as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(3, result.RouteValues.Count);
            Assert.Equal("MyTest", result.RouteValues["controller"]);
            Assert.Equal("CreatedAtRouteSameControllerRouteValues", result.RouteValues["action"]);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtRoute_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.CreatedAtRouteOtherController() as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(2, result.RouteValues.Count);
            Assert.Equal("Other", result.RouteValues["controller"]);
            Assert.Equal("Action", result.RouteValues["action"]);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtRouteWithRouteValues_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.CreatedAtRouteOtherControllerRouteValues() as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(3, result.RouteValues.Count);
            Assert.Equal("Other", result.RouteValues["controller"]);
            Assert.Equal("Action", result.RouteValues["action"]);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void RedirectToAction_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToActionSameController() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.RouteValues);
            Assert.Equal(null, result.ControllerName);
            Assert.Equal("CreatedAtRouteSameController", result.ActionName);
            Assert.Equal(false, result.Permanent);
        }

        [Fact]
        public void RedirectToActionWithRouteValues_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToActionSameControllerRouteValues() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.RouteValues.Count);
            Assert.Equal(null, result.ControllerName);
            Assert.Equal("CreatedAtRouteSameControllerRouteValues", result.ActionName);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal(false, result.Permanent);
        }

        [Fact]
        public void RedirectToAction_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToActionOtherController() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.RouteValues);
            Assert.Equal("Other", result.ControllerName);
            Assert.Equal("Action", result.ActionName);
            Assert.Equal(false, result.Permanent);
        }

        [Fact]
        public void RedirectToActionWithRouteValues_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToActionOtherControllerRouteValues() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.RouteValues.Count);
            Assert.Equal("Other", result.ControllerName);
            Assert.Equal("Action", result.ActionName);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal(false, result.Permanent);
        }

        [Fact]
        public void RedirectToActionPermanent_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToActionPermanentSameController() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.RouteValues);
            Assert.Equal(null, result.ControllerName);
            Assert.Equal("CreatedAtRouteSameController", result.ActionName);
            Assert.Equal(true, result.Permanent);
        }

        [Fact]
        public void RedirectToActionPermanentWithRouteValues_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToActionPermanentSameControllerRouteValues() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.RouteValues.Count);
            Assert.Equal(null, result.ControllerName);
            Assert.Equal("CreatedAtRouteSameControllerRouteValues", result.ActionName);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal(true, result.Permanent);
        }

        [Fact]
        public void RedirectToActionPermanent_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToActionPermanentOtherController() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.RouteValues);
            Assert.Equal("Other", result.ControllerName);
            Assert.Equal("Action", result.ActionName);
            Assert.Equal(true, result.Permanent);
        }

        [Fact]
        public void RedirectToActionPermanentWithRouteValues_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToActionPermanentOtherControllerRouteValues() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.RouteValues.Count);
            Assert.Equal("Other", result.ControllerName);
            Assert.Equal("Action", result.ActionName);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal(true, result.Permanent);
        }

        [Fact]
        public void RedirectToRoute_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToRouteSameController() as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(2, result.RouteValues.Count);
            Assert.Equal("MyTest", result.RouteValues["controller"]);
            Assert.Equal("CreatedAtRouteSameController", result.RouteValues["action"]);
            Assert.Equal(false, result.Permanent);
        }

        [Fact]
        public void RedirectToRouteWithRouteValues_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToRouteSameControllerRouteValues() as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(3, result.RouteValues.Count);
            Assert.Equal("MyTest", result.RouteValues["controller"]);
            Assert.Equal("CreatedAtRouteSameControllerRouteValues", result.RouteValues["action"]);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal(false, result.Permanent);
        }

        [Fact]
        public void RedirectToRoute_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToRouteOtherController() as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(2, result.RouteValues.Count);
            Assert.Equal("Other", result.RouteValues["controller"]);
            Assert.Equal("Action", result.RouteValues["action"]);
            Assert.Equal(false, result.Permanent);
        }

        [Fact]
        public void RedirectToRouteWithRouteValues_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToRouteOtherControllerRouteValues() as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(3, result.RouteValues.Count);
            Assert.Equal("Other", result.RouteValues["controller"]);
            Assert.Equal("Action", result.RouteValues["action"]);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal(false, result.Permanent);
        }

        [Fact]
        public void RedirectToRoutePermanent_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToRoutePermanentSameController() as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(2, result.RouteValues.Count);
            Assert.Equal("MyTest", result.RouteValues["controller"]);
            Assert.Equal("CreatedAtRouteSameController", result.RouteValues["action"]);
            Assert.Equal(true, result.Permanent);
        }

        [Fact]
        public void RedirectToRoutePermanentWithRouteValues_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToRoutePermanentSameControllerRouteValues() as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(3, result.RouteValues.Count);
            Assert.Equal("MyTest", result.RouteValues["controller"]);
            Assert.Equal("CreatedAtRouteSameControllerRouteValues", result.RouteValues["action"]);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal(true, result.Permanent);
        }

        [Fact]
        public void RedirectToRoutePermanent_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToRoutePermanentOtherController() as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(2, result.RouteValues.Count);
            Assert.Equal("Other", result.RouteValues["controller"]);
            Assert.Equal("Action", result.RouteValues["action"]);
            Assert.Equal(true, result.Permanent);
        }

        [Fact]
        public void RedirectToRoutePermanentWithRouteValues_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyTestController();

            // Act
            var result = controller.RedirectToRoutePermanentOtherControllerRouteValues() as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("route", result.RouteName);
            Assert.Equal(3, result.RouteValues.Count);
            Assert.Equal("Other", result.RouteValues["controller"]);
            Assert.Equal("Action", result.RouteValues["action"]);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal(true, result.Permanent);
        }
    }

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = TestServices.Global
                }
            };
        }
    }

    public class MyTestController : BaseController
    {
        public IActionResult CreatedAtActionSameController()
        {
            return this.CreatedAtAction(c => c.CreatedAtActionSameController(), "test");
        }

        public IActionResult CreatedAtActionSameControllerRouteValues()
        {
            return this.CreatedAtAction(c => c.CreatedAtActionSameControllerRouteValues(), new { id = 1 }, "test");
        }

        public IActionResult CreatedAtActionOtherController()
        {
            return this.CreatedAtAction<OtherController>(c => c.Action(), "test");
        }

        public IActionResult CreatedAtActionOtherControllerRouteValues()
        {
            return this.CreatedAtAction<OtherController>(c => c.Action(), new { id = 1 }, "test");
        }

        public IActionResult CreatedAtRouteSameController()
        {
            return this.CreatedAtRoute("route", c => c.CreatedAtRouteSameController(), "test");
        }

        public IActionResult CreatedAtRouteSameControllerRouteValues()
        {
            return this.CreatedAtRoute("route", c => c.CreatedAtRouteSameControllerRouteValues(), new { id = 1 }, "test");
        }

        public IActionResult CreatedAtRouteOtherController()
        {
            return this.CreatedAtRoute<OtherController>("route", c => c.Action(), "test");
        }

        public IActionResult CreatedAtRouteOtherControllerRouteValues()
        {
            return this.CreatedAtRoute<OtherController>("route", c => c.Action(), new { id = 1 }, "test");
        }

        public IActionResult RedirectToActionSameController()
        {
            return this.RedirectToAction(c => c.CreatedAtRouteSameController());
        }

        public IActionResult RedirectToActionSameControllerRouteValues()
        {
            return this.RedirectToAction(c => c.CreatedAtRouteSameControllerRouteValues(), new { id = 1 });
        }

        public IActionResult RedirectToActionOtherController()
        {
            return this.RedirectToAction<OtherController>(c => c.Action());
        }

        public IActionResult RedirectToActionOtherControllerRouteValues()
        {
            return this.RedirectToAction<OtherController>(c => c.Action(), new { id = 1 });
        }

        public IActionResult RedirectToActionPermanentSameController()
        {
            return this.RedirectToActionPermanent(c => c.CreatedAtRouteSameController());
        }

        public IActionResult RedirectToActionPermanentSameControllerRouteValues()
        {
            return this.RedirectToActionPermanent(c => c.CreatedAtRouteSameControllerRouteValues(), new { id = 1 });
        }

        public IActionResult RedirectToActionPermanentOtherController()
        {
            return this.RedirectToActionPermanent<OtherController>(c => c.Action());
        }

        public IActionResult RedirectToActionPermanentOtherControllerRouteValues()
        {
            return this.RedirectToActionPermanent<OtherController>(c => c.Action(), new { id = 1 });
        }

        public IActionResult RedirectToRouteSameController()
        {
            return this.RedirectToRoute("route", c => c.CreatedAtRouteSameController());
        }

        public IActionResult RedirectToRouteSameControllerRouteValues()
        {
            return this.RedirectToRoute("route", c => c.CreatedAtRouteSameControllerRouteValues(), new { id = 1 });
        }

        public IActionResult RedirectToRouteOtherController()
        {
            return this.RedirectToRoute<OtherController>("route", c => c.Action());
        }

        public IActionResult RedirectToRouteOtherControllerRouteValues()
        {
            return this.RedirectToRoute<OtherController>("route", c => c.Action(), new { id = 1 });
        }

        public IActionResult RedirectToRoutePermanentSameController()
        {
            return this.RedirectToRoutePermanent("route", c => c.CreatedAtRouteSameController());
        }

        public IActionResult RedirectToRoutePermanentSameControllerRouteValues()
        {
            return this.RedirectToRoutePermanent("route", c => c.CreatedAtRouteSameControllerRouteValues(), new { id = 1 });
        }

        public IActionResult RedirectToRoutePermanentOtherController()
        {
            return this.RedirectToRoutePermanent<OtherController>("route", c => c.Action());
        }

        public IActionResult RedirectToRoutePermanentOtherControllerRouteValues()
        {
            return this.RedirectToRoutePermanent<OtherController>("route", c => c.Action(), new { id = 1 });
        }
    }

    public class OtherController : BaseController
    {
        public IActionResult Action()
        {
            return null;
        }
    }
}
