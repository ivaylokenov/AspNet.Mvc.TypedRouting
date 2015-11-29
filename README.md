# AspNet.Mvc.TypedRouting

Resolving controller and action names for various purposes in ASP.NET MVC was always unreliable because the framework uses magic strings in its methods (for example `Url.Action("Action", "Controller")`). With the C# 6.0 `nameof` operator, the problem was partially solved. However, `nameof` cannot be used with various MVC 6 features like `ActionNameAttribute`, `AreaAttribute`, `RouteConstraintAttribute`, `IControllerModelConvention`, `IActionModelConvention`, `IParameterModelConvention` and more. Here comes `AspNet.Mvc.TypedRouting` to the rescue!

This package gives you typed expression based routing and link generation in a [ASP.NET MVC 6](https://github.com/aspnet/Mvc) web application. Currently working with version 6.0.0-rc1-final.

For example:

```c#
// adding route to specific action
routes.Add("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index()))

// generating action link
Html.ActionLink<HomeController>("Index", c => c.Index())
```

## Installation

You can install this library using NuGet into your web project. There is no need to add any namespace usings since the package uses the default ones to add extension methods.

    Install-Package AspNet.Mvc.TypedRouting -Pre

For other interesting packages check out:

 - [MyTested.WebApi](https://github.com/ivaylokenov/MyTested.WebApi) - fluent testing framework for ASP.NET Web API 2
 - [ASP.NET MVC 5 Lambda Expression Helpers](https://github.com/ivaylokenov/ASP.NET-MVC-Lambda-Expression-Helpers) - typed expression based link generation for ASP.NET MVC 5
	
## How to use

You can check the provided [sample](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting/tree/master/samples/TypedRoutingWebSite) to see a working web application with this library. 

To register typed route into your application, you need to do the following into your `Startup` class:

```c#
public void ConfigureServices(IServiceCollection services)
{
	services.AddMvc().AddTypedRouting(routes =>
	{
		routes.Get("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index(With.Any<int>())));
	});
}
```

This will register route http://mysite.com/MyRoute/{id} to match 'HomeController', 'Index' action with any integer as 'id'. Full list of available methods:

```c#
// adding route to specific controller and any action name
routes.Add("MyRoute/{action}", route => route.ToController<HomeController>());

// adding route to specific action without parameters
routes.Add("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index()));

// adding route to specific action with any parameters 
// * With.Any<TParameter>() is just expressive sugar, you can pass any value
routes.Add("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index(With.Any<int>())));

// adding route with specific name
routes.Add("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index()).WithName("RouteName"));

// adding route to specific HTTP methods
routes.Add("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index()).ForHttpMethods("GET", "POST"));

// you can also specify methods without magic strings
routes.Get("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index()));
routes.Post("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index()));
routes.Put("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index()));
routes.Delete("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index()));
```

To use expression based link generation, you need to do the following into your `Startup` class:

```c#
public void Configure(IApplicationBuilder app)
{
   // other configuration code
   
   app.UseMvc(routes =>
   {
   	    routes.UseTypedRouting();
   });
}
```

Basically, you can do the following:

```c#
// generating link without parameters - /Home/Index
urlHelper.Action<HomeController>(c => c.Index());

// generating link with parameters - /Home/Index/1
urlHelper.Action<HomeController>(c => c.Index(1));

// generating link with additional route values - /Home/Index/1?key=value
urlHelper.Action<HomeController>(c => c.Index(1), new { key = "value" });

// generating link where action needs parameters to be compiled, but you do not want to pass them - /Home/Index
// * With.No<TParameter>() is just expressive sugar, you can pass 'null' for reference types but it looks ugly
urlHelper.Action<HomeController>(c => c.Index(With.No<int>()));
```

All methods resolve all kinds of route changing features like `ActionNameAttribute`, `AreaAttribute`, `RouteConstraintAttribute`, `IControllerModelConvention`, `IActionModelConvention`, `IParameterModelConvention` and potentially others. The expressions use the internally created by the MVC framework `ControllerActionDescriptor` objects, which contain all route specific information.

**Make sure you read carefully the [Internal caching](#internal-caching) and [Performance consideration](#performance-consideration) sections of this documentation, before you start using the link generation features of the library!**

### Controller extension methods:

```c#
// uses the same controller in the expression and created object
controller.CreatedAtAction(c => c.Index(), someObject);

// uses the same controller in the expression, additional route values and created object
controller.CreatedAtAction(c => c.Index(), new { key = "value" }, someObject);

// uses another controller in the expression and created object
controller.CreatedAtAction<HomeController>(c => c.Index(), someObject);

// uses another controller in the expression, additional route values and created object
controller.CreatedAtAction<HomeController>(c => c.Index(), new { key = "value" }, someObject);

// uses route name, the same controller in the expression and created object
controller.CreatedAtRoute("RouteName", c => c.Index(), someObject);

// uses route name, the same controller in the expression, additional route values and created object
controller.CreatedAtRoute("RouteName", c => c.Index(), new { key = "value" }, someObject);

// uses route name, another controller in the expression and created object
controller.CreatedAtRoute<HomeController>("RouteName", c => c.Index(), someObject);

// uses route name, another controller in the expression, additional route values and created object
controller.CreatedAtRoute<HomeController>("RouteName", c => c.Index(), new { key = "value" }, someObject);

// uses the same controller in the expression to return redirect result
controller.RedirectToAction(c => c.Index());

// uses the same controller in the expression and additional route values to return redirect result
controller.RedirectToAction(c => c.Index(), new { key = "value" });

// uses another controller in the expression to return redirect result
controller.RedirectToAction<HomeController>(c => c.Index());

// uses another controller in the expression and additional route values to return redirect result
controller.RedirectToAction<HomeController>(c => c.Index(), new { key = "value" });

// uses the same controller in the expression to return permanent redirect result
controller.RedirectToActionPermanent(c => c.Index());

// uses the same controller in the expression and additional route values to return permanent redirect result
controller.RedirectToActionPermanent(c => c.Index(), new { key = "value" });

// uses another controller in the expression to return permanent redirect result
controller.RedirectToActionPermanent<HomeController>(c => c.Index());

// uses another controller in the expression and additional route values to return permanent redirect result
controller.RedirectToActionPermanent<HomeController>(c => c.Index(), new { key = "value" });

// uses route name, the same controller in the expression to return redirect result
controller.RedirectToRoute(c => c.Index());

// uses route name, the same controller in the expression and additional route values to return redirect result
controller.RedirectToRoute(c => c.Index(), new { key = "value" });

// uses route name, another controller in the expression to return redirect result
controller.RedirectToRoute<HomeController>(c => c.Index());

// uses route name, another controller in the expression and additional route values to return redirect result
controller.RedirectToRoute<HomeController>(c => c.Index(), new { key = "value" });

// uses route name, the same controller in the expression to return permanent redirect result
controller.RedirectToRoutePermanent(c => c.Index());

// uses route name, the same controller in the expression and additional route values to return permanent redirect result
controller.RedirectToRoutePermanent(c => c.Index(), new { key = "value" });

// uses route name, another controller in the expression to return permanent redirect result
controller.RedirectToRoutePermanent<HomeController>(c => c.Index());

// uses route name, another controller in the expression and additional route values to return permanent redirect result
controller.RedirectToRoutePermanent<HomeController>(c => c.Index(), new { key = "value" });
```

### IHtmlHelper extension methods:

```c#
// generates action link with the link text and the expression
Html.ActionLink<HomeController>("Link text", c => c.Index());

// generates action link with the link text, the expression and additional route values
Html.ActionLink<HomeController>("Link text", c => c.Index(), new { key = "value" });

// generates action link with the link text, the expression, additional route values and HTML attributes
Html.ActionLink<HomeController>("Link text", c => c.Index(), new { key = "value" }, new { @class = "my-class" });

// generates action link with the link text, the expression, protocol, host name, fragment, additional route values and HTML attributes
Html.ActionLink<HomeController>("Link text", c => c.Index(), "protocol", "hostname", "fragment", new { key = "value" }, new { @class = "my-class" });

// generates action link with route name, the link text and the expression
Html.RouteLink<HomeController>("Route name", "Link text", c => c.Index());

// generates action link with route name, the link text, the expression and additional route values
Html.RouteLink<HomeController>("Route name", "Link text", c => c.Index(), new { key = "value" });

// generates action link with route name, the link text, the expression, additional route values and HTML attributes
Html.RouteLink<HomeController>("Route name", "Link text", c => c.Index(), new { key = "value" }, new { @class = "my-class" });

// generates action link with route name, the link text, the expression, protocol, host name, fragment, additional route values and HTML attributes
Html.RouteLink<HomeController>("Route name", "Link text", c => c.Index(), "protocol", "hostname", "fragment", new { key = "value" }, new { @class = "my-class" });

// begins form to the action from the expression
Html.BeginForm<HomeController>(c => c.Index());

// begins form to the action from the expression and additional route values
Html.BeginForm<HomeController>(c => c.Index(), new { key = "value" });

// begins form to the action from the expression and form method
Html.BeginForm<HomeController>(c => c.Index(), FormMethod.Post);

// begins form to the action from the expression, additional route values and form method
Html.BeginForm<HomeController>(c => c.Index(), new { key = "value" },  FormMethod.Post);

// begins form to the action from the expression, form method and HTML attributes
Html.BeginForm<HomeController>(c => c.Index(), FormMethod.Post, new { @class = "my-class" });

// begins form to the action from the expression, form method and HTML attributes
Html.BeginForm<HomeController>(c => c.Index(), new { key = "value" }, FormMethod.Post, new { @class = "my-class" });

// begins form to the action from the expression by specifying route name
Html.BeginRouteForm<HomeController>("Route name", c => c.Index());

// begins form to the action from the expression and additional route values by specifying route name
Html.BeginRouteForm<HomeController>("Route name", c => c.Index(), new { key = "value" });

// begins form to the action from the expression and form method by specifying route name
Html.BeginRouteForm<HomeController>("Route name", c => c.Index(), FormMethod.Post);

// begins form to the action from the expression, additional route values and form method by specifying route name
Html.BeginRouteForm<HomeController>("Route name", c => c.Index(), new { key = "value" },  FormMethod.Post);

// begins form to the action from the expression, form method and HTML attributes by specifying route name
Html.BeginRouteForm<HomeController>("Route name", c => c.Index(), FormMethod.Post, new { @class = "my-class" });

// begins form to the action from the expression, form method and HTML attributes by specifying route name
Html.BeginRouteForm<HomeController>("Route name", c => c.Index(), new { key = "value" }, FormMethod.Post, new { @class = "my-class" });
```

### IUrlHelper extension methods:

```c#
// generates link to the action from the expression
urlHelper.Action<HomeController>(c => c.Index());

// generates link to the action from the expression with additional route values
urlHelper.Action<HomeController>(c => c.Index(), new { key = "value" });

// generates link to the action from the expression with additional route values and protocol
urlHelper.Action<HomeController>(c => c.Index(), new { key = "value" }, "protocol");

// generates link to the action from the expression with additional route values, protocol and host name
urlHelper.Action<HomeController>(c => c.Index(), new { key = "value" }, "protocol", "hostname");

// generates link to the action from the expression with additional route values, protocol, host name and fragment
urlHelper.Action<HomeController>(c => c.Index(), new { key = "value" }, "protocol", "hostname", "fragment");

// generates link to the action from the expression by specifying route name
urlHelper.Link<HomeController>("Route name", c => c.Index());

// generates link to the action from the expression with additional route values and by specifying route name
urlHelper.Link<HomeController>("Route name", c => c.Index(), new { key = "value" });
```

All these methods are well documented, tested and resolve route values successfully.

### Internal caching

The expression parser uses an internal cache to improve the performance of the route resolving. The important for this package objects from the MVC framework are `IActionDescriptorsCollectionProvider` and the collection of `ControllerActionDescriptor` it provides. These are created at application start up and cached by the MVC framework afterwards for later usage. It is really highly unlikely for a developer to change these objects run-time, but if you do for some reason and see unexpected results from the link generation, you need to clear the expression parser's internal cache or reinitialize it so that it continues to function properly. Here is how it can be done:

```c#
// clears the internal cache without changing the IActionDescriptorsCollectionProvider
ExpressionRouteHelper.ClearActionCache();

// reinitializes the link generation with the provided IServiceProvider
// from which the IActionDescriptorsCollectionProvider will be resolved again
ExpressionRouteHelper.Initialize(serviceProvider);
```

### Performance consideration

The expression parsing gives small overhead when generating links but the overall performance is quite good. However, keep in mind the following measurements (you can see the [PerformanceTest sample project](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting/tree/master/samples/PerformanceTest))

```c#
// * All these measurements are collected using System.Diagnostics.Stopwatch. It is not the best way
// * to measure execution time but it gives enough information to compare how much slower is one method to another.

// * All measurements are for exactly 5000 generated URLs.

// When using expressions without parameters, the results are fine.
urlHelper.Action("Action", "My"); // ~7ms
urlHelper.Action<MyController>(c => c.Action()); // ~20ms

// When using expressions with constant parameters, the results are also fine.
urlHelper.Action("Action", "My", new { id = 1, text = "text" }); // ~8 ms
urlHelper.Action<MyController>(c => c.Action(1, "text")); // ~25 ms

// When using expression with variables as parameters, the results
// get quite slower compared to the magic string based method.
// Half a second for 5000 links is still OK for an average
// web application (thank you, C#) but this can be improved quite easily.
// * This is because expressions have to be compiled to examine the actual values behind the parameters,
// * while the anonymous objects are cached internally by the MVC framework.
urlHelper.Action("Action", "My", new { id, text }); // ~8 ms
urlHelper.Action<MyController>(c => c.Action(id, text)); // ~499 ms

// With model objects as values, the results are quite slower too.
urlHelper.Action("Action", "My", new { id, model = new Model { Integer = 2, String = "text" } }); // ~7 ms
urlHelper.Action<MyController>(c => c.Action(id, new Model { Integer = 2, String = "text" })); // ~692 ms

// Now lets see how we can improve this to become up to ten times faster.
// You can use With.No<TParameter>() in the expression and pass the values as anonymous object.
// * The expression parser recognises the With.No<TParameter>() method call and skips it without compiling the expression
// * and the MVC framework caches the anonymous object results.
urlHelper.Action("Action", "My", new { id, text }); // ~7 ms
urlHelper.Action<MyController>(c => c.Action(With.No<int>(), With.No<string>()), new { id, text }); // ~70 ms

// And with model objects.
urlHelper.Action("Action", "My", new { id, model = new Model { Integer = 2, String = "text" } })); // ~7 ms
urlHelper.Action<MyController>(c => c.Action(With.No<int>(), With.No<Model>()), new { id, model new Model { Integer = 2, String = "text" }); // ~67 ms

// The code is a bit uglier this way, but if you care about performance, it should be fine.
// You still get typed link generation with all the intellisense and refactoring goodies from your IDE
// and no magic values at all for specifying controllers and actions.
``` 

## Licence

Code by Ivaylo Kenov. Copyright 2015 Ivaylo Kenov.

This package has MIT license. Refer to the [LICENSE](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting/blob/master/LICENSE) for detailed information.

## Any questions, comments or additions?

If you have a feature request or bug report, leave an issue on the [issues page](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting/issues) or send a [pull request](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting/pulls). For general questions and comments, use the [StackOverflow](http://stackoverflow.com/) forum.