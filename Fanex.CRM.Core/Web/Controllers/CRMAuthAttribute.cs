// --------------------------------------------------------------------------
// <copyright file="CRMAuthAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Web.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;

    /// <summary>
    /// CRM Authenticate Attribute.
    /// </summary>
    public class CRMAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            AppSession.CurrentHttpMethod = filterContext.HttpContext.Request.HttpMethod;
            base.OnAuthorization(filterContext);
        }
    }
}