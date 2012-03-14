using System.Web.Mvc;

namespace YTech.IM.SenseCity.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
