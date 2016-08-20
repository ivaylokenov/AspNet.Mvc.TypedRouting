namespace TypedRoutingWebSite.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    public class AreaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ToOther()
        {
            return PartialView();
        }
    }
}
