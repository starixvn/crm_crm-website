// --------------------------------------------------------------------------
// <copyright file="CommunicationLogController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>


namespace Fanex.CRM.Communication.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Services.Implements;
    using Fanex.CRM.Communication.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Communication Log Controller.
    /// </summary>
    public class CommunicationLogController : BaseController
    {
        private ICommunicationLogService _serviceInstance;

        public CommunicationLogController()
            : this(null)
        {
        }

        public CommunicationLogController(ICommunicationLogService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new CommunicationLogService();
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult Index(string customerId)
        {
            List<LogInfo> logs = _serviceInstance.GetLogsByCustomerId(new GetLogsByCustomerIdCriteria
            {
                CustomerId = customerId
            });

            return View("~/Areas/Communication/CommunicationLog.cshtml", logs);
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Host)]
        public ActionResult OpenEditView(int logRunningNumber)
        {
            var logInfo = _serviceInstance.GetLogsByRunningNumber(new GetLogByRunningNumberCriteria
            {
                RunningNumber = logRunningNumber
            });

            return View("~/Areas/Communication/EditLog/Index.cshtml", logInfo);
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Host)]
        public ActionResult OpenInsertView()
        {
            return View("~/Areas/Communication/Insert/Index.cshtml");
        }
    }
}
