// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using Fanex.CRM.Web.Controllers.Helper;

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : BaseController
    {
        private int _currentParentId = 0;

        private IPermissionService _permission;

        public HomeController()
            : this(null)
        {
        }

        public HomeController(IPermissionService permission)
        {
            _permission = permission ?? new PermissionService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BlankPage()
        {
            return View();
        }

        /// <summary>
        /// Access default page.
        /// </summary>
        /// <returns>Default page.</returns>
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
                    ViewBag.TopMenu = string.Empty;
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
                {
                    return ForceLogin();
                }
            }
        }

        public ActionResult Footer()
        {
            return View();
        }

        #region PrivateFunctions

        /// <summary>
        /// Writes the menu.
        /// </summary>
        /// <param name="parentMenu">The parent menu.</param>
        /// <param name="allMenu">All menu value.</param>
        /// <param name="level">The level value.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
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
                            if (!_permission.GetHostPermission(Enumerations.Functions.Feedback.DeleteFeedback, AppSession.RoleAccess))
                            {
                                continue;
                            }
                        }

                        var childList = menu.GetChildList(allMenu);
                        parentId = string.Format("id=\"pId{0}\"", parentIndex);
                        url = menu.Url.ToLower().Equals("n/a") ? string.Empty : "javascript:gotoURL('centerFr','" + menu.Url + "'); ";
                        target = menu.Target == "_blank" ? "_blank" : "centerFr";

                        ViewBag.TopMenu += "<li " + (level == 0 ? parentId : string.Empty) + " class=\" menu_" + level + "\">" +
                                "<a class=\"rmLink\" " +
                                    (level != 0 && childList.Count() == 0 ?
                                    ("onclick=\"" + url + (target == "_blank" ? " jQuery.unblockUI();" : string.Empty) +
                                    "; closedSubmenu('pId" + _currentParentId + "', " + (url.Equals(string.Empty) ? 1 : 0) + ");\">") : ">") + menu.Name +
                                        "<span class=\"" + (level == 0 && childList.Count() > 0 ? "rmExpandDown" : "rmExpand") + "\"></span>" +
                                "</a>";

                        WriteMenu(childList, allMenu, level + 1);
                        ViewBag.TopMenu += "</li>";
                        if (level != 0)
                        {
                            continue;
                        }

                        parentIndex++;
                        _currentParentId = parentIndex;
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

        #endregion PrivateFunctions
    }
}