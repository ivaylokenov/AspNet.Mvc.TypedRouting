namespace TypedRoutingWebSite.Controllers
{
    using Microsoft.AspNet.Mvc;

    public class ExpressionsController : Controller
    {
        public IActionResult Index()
        {
            return this.RedirectToAction(c => c.Redirect());
        }

        public IActionResult Redirect()
        {
            return this.RedirectToAction<HomeController>(c => c.Contact());
        }

        public IActionResult WithRouteValues(int id)
        {
            return this.RedirectToAction<HomeController>(c => c.Index(With.No<int>()), new { id });
        }

        public IActionResult CustomUrl(int id)
        {
            return this.Content(this.Url.Action<ExpressionsController>(c => c.WithRouteValues(With.No<int>()), new { id }));
        }

        public IActionResult Created()
        {
            return this.CreatedAtAction<HomeController>(c => c.Index(), 5);
        }

        [HttpGet("SubmitForm")]
        public void Submit()
        {
        }
    }
}
