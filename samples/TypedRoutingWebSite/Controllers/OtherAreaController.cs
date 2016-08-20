namespace TypedRoutingWebSite.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Area("Other")]
    public class OtherAreaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
