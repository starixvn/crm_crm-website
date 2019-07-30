namespace Fanex.CRM.Common.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;

    [AllowAnonymous]
    public class CommonController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;

        public CommonController() : this(null)
        {
        }

        public CommonController(MaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        public ActionResult AccessDenied()
        {
            return View("~/Views/Common/AccessDenied.cshtml");
        }

        public ActionResult ErrorPage()
        {
            return View("~/Views/Common/ErrorPage.cshtml");
        }

        public ActionResult Offline()
        {
            if (!_maintenanceService.IsUnderMaintenance())
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            var downTime = _maintenanceService.GetUnderMaintenanceTime();
            ViewBag.UmtFrom = downTime.From;
            ViewBag.UmtTo = downTime.To;

            return View();
        }
    }
}