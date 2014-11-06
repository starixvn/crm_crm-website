// --------------------------------------------------------------------------
// <copyright file="CRMAuthAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Services
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// CRM Authenticate Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class CRMAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            AppSession.CurrentHttpMethod = filterContext.HttpContext.Request.HttpMethod;
            base.OnAuthorization(filterContext);
        }
    }
}