// --------------------------------------------------------------------------
// <copyright file="LogExceptionFilter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers.CommonFilters
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Logging;

    /// <summary>
    /// Log Exception Filter.
    /// </summary>
    public class LogExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
        }

        private static void UnhandleException(ExceptionContext filterContext)
        {
        }
    }
}