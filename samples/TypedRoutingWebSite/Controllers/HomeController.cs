namespace TypedRoutingWebSite.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index(int id)
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult NamedRedirect()
        {
            return Content(Url.Link("CustomName", new { returnUrl = "Test" }));
        }

        public IActionResult LinkGeneration()
        {
            return Content(Url.Action<HomeController>(c => c.Contact()));
        }

        public IActionResult ToArea()
        {
            return PartialView();
        }
    }
}
