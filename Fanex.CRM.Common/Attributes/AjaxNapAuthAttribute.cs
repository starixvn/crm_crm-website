// --------------------------------------------------------------------------
// <copyright file="AjaxNapAuthAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2014.12.18">Created</change>
// </history>

namespace Fanex.CRM.Common.Attributes
{
    using System;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Resources;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Ajax Nap Authentication attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class AjaxNapAuthAttribute : NapAuthAttribute
    {
        /// <summary>
        /// Called when [authorization].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (!CheckFunctionAuthorization(AppSession.CurrentUser.Id))
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new Result
                    {
                        Code = ErrorCodeConstants.NoPermission,
                        Message = Labels.NoPermission
                    }
                };
            }
        }
    }
}
