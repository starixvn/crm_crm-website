
namespace Fanex.CRM.Web.Controllers
{
    using System.Web.Mvc;

    public class CommonController: BaseController
    {

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}