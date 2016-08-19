<h1><img src="https://raw.githubusercontent.com/ivaylokenov/AspNet.Mvc.TypedRouting/master/tools/logo.png" align="left" alt="AspNet.Mvc.TypedRouting" width="100">&nbsp; AspNet.Mvc.TypedRouting - Typed routing<br />&nbsp; and link generation for ASP.NET Core MVC</h1>
====================================

Resolving controller and action names for various purposes in ASP.NET MVC was always unreliable because the framework uses magic strings in its methods (for example `Url.Action("Action", "Controller")`). With the C# 6.0 `nameof` operator, the problem was partially solved. However, `nameof` cannot be used with various MVC Core features like `ActionNameAttribute`, `AreaAttribute`, `RouteValueAttribute`, `IControllerModelConvention`, `IActionModelConvention`, `IParameterModelConvention` and more. Here comes `AspNet.Mvc.TypedRouting` to the rescue!

This package gives you typed expression based routing and link generation in a [ASP.NET Core MVC](https://github.com/aspnet/Mvc) web application. Currently working with version 1.0.0.

For example:

```c#
// adding route to specific action
routes.Add("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index()))

// generating action link
Html.ActionLink<HomeController>("Index", c => c.Index())
```

[![Build status](https://ci.appveyor.com/api/projects/status/mvoobyf3s99pkpkf?svg=true)](https://ci.appveyor.com/project/ivaylokenov/aspnet-mvc-typedrouting) [![NuGet Version](http://img.shields.io/nuget/v/AspNet.Mvc.TypedRouting.svg?style=flat)](https://www.nuget.org/packages/AspNet.Mvc.TypedRouting/) [![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting/blob/master/LICENSE)

## Installation

You can install this library using NuGet into your web project. There is no need to add any namespace usings since the package uses the default ones to add extension methods.

    Install-Package AspNet.Mvc.TypedRouting

For other interesting packages check out:

 - [MyTested.AspNetCore.Mvc](https://github.com/ivaylokenov/MyTested.AspNetCore.Mvc) - fluent testing framework for ASP.NET Core MVC
 - [MyTested.HttpServer](https://github.com/ivaylokenov/MyTested.HttpServer) - fluent testing framework for remote HTTP servers
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
// adding route to specific controller and action name taken from name of method
routes.Add("MyRoute/{action}", route => route.ToController<HomeController>());

// adding route to specific action without parameters
routes.Add("MyRoute/MyAction", route => route.ToAction<HomeController>(a => a.Index()));

// adding route to specific action with any parameters 
// * With.Any<TParameter>() is just expressive sugar, you can pass any value
routes.Add("MyRoute/MyAction/{id}", route => route.ToAction<HomeController>(a => a.Index(With.Any<int>())));

// adding route with specific name
routes.Add("MyRoute/MyAction", route => route
	.ToAction<HomeController>(a => a.Index())
	.WithName("RouteName"));

// adding route with custom action constraint
routes.Add("MyRoute/MyAction", route => route
	.ToAction<HomeController>(a => a.Index())
	.WithActionConstraint(new MyCustomConstraint()));
	
// adding route to specific HTTP methods
routes.Add("MyRoute/MyAction", route => route
	.ToAction<HomeController>(a => a.Index())
	.ForHttpMethods("GET", "POST"));

// you can also specify methods without magic strings
routes.Get("MyRoute/MyAction", route => route.ToAction<HomeController>(a => a.Index()));
routes.Post("MyRoute/MyAction", route => route.ToAction<HomeController>(a => a.Index()));
routes.Put("MyRoute/MyAction", route => route.ToAction<HomeController>(a => a.Index()));
routes.Delete("MyRoute/MyAction", route => route.ToAction<HomeController>(a => a.Index()));
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
controller.RedirectToRoute("RouteName", c => c.Index());

// uses route name, the same controller in the expression and additional route values to return redirect result
controller.RedirectToRoute("RouteName", c => c.Index(), new { key = "value" });

// uses route name, another controller in the expression to return redirect result
controller.RedirectToRoute<HomeController>("RouteName", c => c.Index());

// uses route name, another controller in the expression and additional route values to return redirect result
controller.RedirectToRoute<HomeController>("RouteName", c => c.Index(), new { key = "value" });

// uses route name, the same controller in the expression to return permanent redirect result
controller.RedirectToRoutePermanent("RouteName", c => c.Index());

// uses route name, the same controller in the expression and additional route values to return permanent redirect result
controller.RedirectToRoutePermanent("RouteName", c => c.Index(), new { key = "value" });

// uses route name, another controller in the expression to return permanent redirect result
controller.RedirectToRoutePermanent<HomeController>("RouteName", c => c.Index());

// uses route name, another controller in the expression and additional route values to return permanent redirect result
controller.RedirectToRoutePermanent<HomeController>("RouteName", c => c.Index(), new { key = "value" });
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
Html.BeginForm<HomeController>(c => c.Index(), new { key = "value" }, FormMethod.Post);

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

* Note: All form generation methods have additional overloads which allow adding an anti-forgery token.

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

## Licence

Code by Ivaylo Kenov. Copyright 2015-2016 Ivaylo Kenov.

This package has MIT license. Refer to the [LICENSE](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting/blob/master/LICENSE) for detailed information.

## Any questions, comments or additions?

If you have a feature request or bug report, leave an issue on the [issues page](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting/issues) or send a [pull request](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting/pulls). For general questions and comments, use the [StackOverflow](http://stackoverflow.com/) forum.
