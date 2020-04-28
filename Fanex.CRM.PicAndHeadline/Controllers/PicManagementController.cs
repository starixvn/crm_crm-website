using Fanex.CRM.Common.Controllers;
using System.Web.Mvc;

namespace Fanex.CRM.PicAndHeadline.Controllers
{
    public class PicManagementController : BaseController
    {
        public ActionResult Index()
        {

            return View("~/Areas/PicAndHeadline/PicManagement/Index.cshtml");
        }
    }
}
