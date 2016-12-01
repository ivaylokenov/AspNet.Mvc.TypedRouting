namespace TypedRoutingWebSite.Test
{
    using Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class TypedRoutingTest
    {
        [Fact]
        public void RegularRoutes_ShouldWorkCorrectly()
        {
            MyMvc
                .Routing()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index());
        }

        [Fact]
        public void ToController_ShouldWorkCorrectly()
        {
            MyMvc
                .Routing()
                .ShouldMap("/CustomController/Redirect")
                .To<ExpressionsController>(c => c.Redirect());
        }
        
        [Fact]
        public void ToAction_ShouldWorkCorrectly()
        {
            MyMvc
                .Routing()
                .ShouldMap("/CustomContact")
                .To<HomeController>(c => c.Contact());
        }

        [Fact]
        public void WithAny_ShouldWorkCorrectly()
        {
            MyMvc
                .Routing()
                .ShouldMap("/WithParameter/1")
                .To<HomeController>(c => c.Index(1));
        }

        [Fact]
        public void HttpConstrains_ShouldWorkCorrectly()
        {
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/CustomContact"))
                .ToNonExistingRoute();
        }
        
        [Fact]
        public void MultipleHttpConstrainsShouldWorkCorrectly()
        {
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/MultipleMethods"))
                .To<HomeController>(c => c.About());
            
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Get)
                    .WithLocation("/MultipleMethods"))
                .To<HomeController>(c => c.About());

            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Put)
                    .WithLocation("/MultipleMethods"))
                .ToNonExistingRoute();
        }

        [Fact]
        public void WithActionConstraintsShouldWorkCorrectly()
        {
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Put)
                    .WithLocation("/Constraint"))
                .To<AccountController>(c => c.Login(With.Any<string>()));

            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Get)
                    .WithLocation("/Constraint"))
                .ToNonExistingRoute();
        }

        [Fact]
        public void AsyncActionShouldWorkCorrectly()
        {
            MyMvc
                .Routing()
                .ShouldMap("/Async")
                .To<AccountController>(c => c.LogOff());
        }

        [Fact]
        public void NamedRouteShouldWorkCorrectly()
        {
            MyMvc
                .Controller<HomeController>()
                .WithRouteData()
                .Calling(c => c.NamedRedirect())
                .ShouldReturn()
                .Content("/Named?returnUrl=Test");
        }

        [Fact]
        public void LinkGenerationShouldWorkCorrectlyInController()
        {
            MyMvc
                .Controller<HomeController>()
                .WithRouteData()
                .Calling(c => c.LinkGeneration())
                .ShouldReturn()
                .Content("/CustomContact");
        }
    }
}
