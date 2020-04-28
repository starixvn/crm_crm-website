using Fanex.CRM.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.CRM.PicAndHeadline.Controllers
{
    public class PicOfHeadlineController : BaseController
    {
        public ActionResult Index()
        {

            return View("~/Areas/PicAndHeadline/PicOfHeadline/Index.cshtml");
        }
    }
}
