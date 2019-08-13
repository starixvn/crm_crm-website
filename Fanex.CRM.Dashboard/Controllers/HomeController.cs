namespace Fanex.CRM.Dashboard.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using Services;

    public class HomeController : BaseController
    {
        private readonly IPermissionService _permissionService;
        private readonly ILanguageService _langService;
        private readonly IHomeService _homeService;

        public HomeController()
            : this(null, null, null)
        {
        }

        public HomeController(
            IPermissionService permissionService,
            ILanguageService langService,
            IHomeService homeService)
        {
            _permissionService = permissionService ?? new PermissionService();
            _langService = langService ?? new LanguageService();
            _homeService = homeService ?? new HomeService();
        }

        public RedirectResult Index()
        {
            return new RedirectResult("~/Home/Default");
        }

        public ActionResult BlankPage()
        {
            return View("~/Views/Home/BlankPage.cshtml");
        }

        public ActionResult Default(string language)
        {
            if (language != null)
            {
                _langService.ChangeLanguage(language);
            }

            ViewBag.AccUsername = AppSession.CurrentUser.Username;
            ViewBag.CreateCustomerInfoPolicy = _permissionService.GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);
            ViewBag.CurrentCustomer = AppSession.CurrentCustomer;
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;
            ViewBag.SettingLanguage = AppSession.CurrentUser.Language;

            return View("~/Views/Home/Default.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult GettingUserMenu()
        {
            var currentUserMenu = _homeService.UpdateMenuLanguage(AppSession.CurrentUserMenu);
            return PartialView("~/Views/Home/_Menu.cshtml", currentUserMenu);
        }

        public PartialViewResult GettingDashboardMenu()
        {
            ViewBag.HasCustomerInfoPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Read);
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;
            return PartialView("~/Views/Home/_DashboardMenu.cshtml");
        }

        public PartialViewResult SearchCustomerRemind()
        {
            return PartialView("~/Views/Home/_SearchCustomerRemind.cshtml");
        }
    }
}