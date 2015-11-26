namespace AspNet.Mvc.TypedRouting.Test.LinkGeneration
{
    using Microsoft.AspNet.Mvc;
    using Xunit;

    [Collection("TypedRoutingTests")]
    public class ControllerExtensionsTest
    {
        [Fact]
        public void CreatedAtAction_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyController();

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
            var controller = new MyController();

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
            var controller = new MyController();

            // Act
            var result = controller.CreatedAtActionOtherController() as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Other", result.ControllerName);
            Assert.Equal("Action", result.ActionName);
            Assert.Empty(result.RouteValues);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtActionWithRouteValues_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyController();

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
            var controller = new MyController();

            // Act
            var result = controller.CreatedAtRouteSameController() as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.RouteValues);
            Assert.Equal("My", result.RouteValues["controller"]);
            Assert.Equal("CreatedAtRouteSameController", result.RouteValues["action"]);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtRouteWithRouteValues_SameController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyController();

            // Act
            var result = controller.CreatedAtRouteSameControllerRouteValues() as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.RouteValues);
            Assert.Equal("My", result.RouteValues["controller"]);
            Assert.Equal("CreatedAtRouteSameControllerRouteValues", result.RouteValues["action"]);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtRoute_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyController();

            // Act
            var result = controller.CreatedAtRouteOtherController() as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.RouteValues);
            Assert.Equal("Other", result.RouteValues["controller"]);
            Assert.Equal("Action", result.RouteValues["action"]);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public void CreatedAtRouteWithRouteValues_OtherController_ResolvesCorrectly()
        {
            // Arrange
            var controller = new MyController();

            // Act
            var result = controller.CreatedAtRouteOtherControllerRouteValues() as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.RouteValues);
            Assert.Equal("Other", result.RouteValues["controller"]);
            Assert.Equal("Action", result.RouteValues["action"]);
            Assert.Equal(1, result.RouteValues["id"]);
            Assert.Equal("test", result.Value);
        }

        public class MyController : Controller
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
        }

        public class OtherController : Controller
        {
            public IActionResult Action()
            {
                return null;
            }
        }
    }
}
