
namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Web.Controllers.Helper;
    using Fanex.CRM.Service.Interface;
    using Fanex.CRM.Service.Impl;

    public class HomeController: BaseController
    {
        private int currentParentId = 0;
        IPermissionService _permission;

        public HomeController(): this(null) {
        }

        public HomeController(IPermissionService permission) {
            _permission = permission ?? new PermissionService();
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult BlankPage()
        {
            return View();
        }

        public ActionResult Default()
        {
            if (AppSession.UserMenu == null)
            {
                var url = VirtualPathUtility.ToAbsolute("~");
                return Content("<html><script>top.location.href = '" + VirtualPathUtility.ToAbsolute("Account/SignIn.aspx") + "'; </script></html>");
            }
            else
            {
                if (AppSession.CurrentUser != null)
                {
                    ViewBag.AccUsername = AppSession.CurrentUser.Username;
                    ViewBag.TopMenu = "";
                    var menu = AppSession.UserMenu;
                    if (menu != null)
                    {
                        ViewBag.TopMenu = "<div id=\"smoothmenu1\" class=\"ddsmoothmenu\">";

                        IEnumerable<Menu> menuCollection = null;
                        menuCollection = AppSession.UserMenu as List<Menu>;
                        WriteMenu(menuCollection.Where(item => item.Parent == null).Select(s => s), menuCollection, 0);
                        ViewBag.TopMenu += "<br style=\"clear:left\" />";
                        ViewBag.TopMenu += "</div>";
                    }

                    return View();
                }
                else
                    return ForceLogin();
                
                
            }
        }
       
        public ActionResult Footer()
        {
            return View();
        }
        #region private functions
        private void WriteMenu(IEnumerable<Menu> parentMenu, IEnumerable<Menu> allMenu, int level)
        {
            try
            {                
                if (parentMenu.Count() > 0)
                {
                    string url;
                    string target;
                    string parentId;
                    var parentIndex = 0;

                    ViewBag.TopMenu += "<ul>";
                    foreach (Menu menu in parentMenu)
                    {
                        if (menu.Name.ToLower().Contains("deleted"))
                        {
                            if (!_permission.GetHostPermission(Fanex.CRM.Infrastructure.Common.Enumerations.Functions.Feedback.DeleteFeedback, AppSession.RoleAccess))
                                continue;
                        }

                        var childList = menu.GetChildList(allMenu);

                        parentId = string.Format("id=\"pId{0}\"", parentIndex);

                        url = menu.Url.ToLower().Equals("n/a") ? "" : "javascript:gotoURL('centerFr','" + menu.Url + "'); ";

                        target = menu.Target == "_blank" ? "_blank" : "centerFr";

                        ViewBag.TopMenu += "<li " + (level == 0 ? parentId : "") + " class=\" menu_" + level + "\">" +
                                "<a class=\"rmLink\" " +
                                    (level != 0 && childList.Count() == 0 ?
                                    ("onclick=\"" + url + (target == "_blank" ? " jQuery.unblockUI();" : "") + 
                                    "; closedSubmenu('pId" + currentParentId + "', " + (url.Equals("") ? 1 : 0) + ");\">") : ">") + menu.Name + 
                                        "<span class=\"" + (level == 0 && childList.Count() > 0 ? "rmExpandDown" : "rmExpand") + "\"></span>" +
                                "</a>";

                        WriteMenu(childList, allMenu, level + 1);
                        ViewBag.TopMenu += "</li>";

                        if (level != 0) continue;
                        parentIndex++;
                        currentParentId = parentIndex;
                    }
                    ViewBag.TopMenu += "</ul>";
                }
            }
            catch (Exception exception)
            {

                LogExtensions.ExceptionHandler(exception);
                ViewBag.TopMenu = string.Empty;
            }

            
        }
        
        #endregion
    }
}