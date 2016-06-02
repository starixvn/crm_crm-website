// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Common.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using ONEbook.UM.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [OverridableNapSupperAuth]
    [CheckUMFilter]
    public class BaseController : Controller
    {
        private readonly IPermissionService _permissionService;

        public BaseController()
        {
            _permissionService = new PermissionService();
        }

        protected IPermissionService PermissionService
        {
            get
            {
                return _permissionService;
            }
        }

        /// <summary>
        /// HasPermission: check that user has permission to access one menu link.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>
        /// Has Read Permission indicator.
        /// </returns>
        public bool HasReadPermission(string functionName)
        {
            return PermissionService.HasReadPermission(functionName);
        }

        public bool HasHostPermission(string functionName)
        {
            return PermissionService.HasHostPermission(functionName);
        }

        public bool HasPermission(string functionName, Constant.Policy policy)
        {
            return PermissionService.HasPermission(functionName, policy);
        }

        public Constant.Policy GetPolicy(string functionName)
        {
            return PermissionService.GetPolicy(functionName);
        }
    }
}