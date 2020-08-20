namespace CRM.Services.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Constants;
    using CRM.Core.Enumerables;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.Common;
    using CRM.Criteria.NotificationList;
    using CRM.Criteria.NotificationType;
    using CRM.Models;
    using CRM.Models.NotificationList;
    using CRM.Models.NotificationType;
    using CRM.Repositories;
    using CRM.Services;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;
    using static Fanex.NAP.Core.Business.Constant;

    public class NotificationListService
    {
        private INotificationListRepository notificationListRepository;
        private IBaseService baseService;
    }
}