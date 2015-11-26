namespace AspNet.Mvc.TypedRouting.LinkGeneration
{
    using Microsoft.AspNet.Mvc;
    using System;
    using System.Linq.Expressions;

    public static class ControllerExtensions
    {
        public static CreatedAtActionResult CreatedAtAction<TController>(
            this TController controller,
            Expression<Action<TController>> action,
            object value)
            where TController : Controller
        {
            return null;
        }

        public static CreatedAtActionResult CreatedAtAction<TController>(
            this TController controller,
            Expression<Action<TController>> action,
            object routeValues,
            object value)
            where TController : Controller
        {

            return null;
        }

        public static CreatedAtActionResult CreatedAtAction<TRedirectController>(
            this Controller controller,
            Expression<Action<TRedirectController>> action,
            object value)
        {
            return null;
        }

        public static CreatedAtActionResult CreatedAtAction<TRedirectController>(
            this Controller controller,
            Expression<Action<TRedirectController>> action,
            object routeValues,
            object value)
        {
            return null;
        }
    }
}
