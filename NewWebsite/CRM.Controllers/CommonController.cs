namespace CRM.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Filters;
    using CRM.Models.NotificationType;
    using CRM.Services;
    using Microsoft.AspNetCore.Mvc;

    [CrmAuthorize]
    public class CommonController : Controller
    {
        private readonly INotificationTypeService notificationTypeService;

        public CommonController(INotificationTypeService notificationTypeService)
        {
            this.notificationTypeService = notificationTypeService;
        }

        public async Task<IActionResult> GetNotificationTypes()
        {
            IEnumerable<NotificationType> notificationTypes = await notificationTypeService.GetAsync(new GetGridDataParam());
            return Json(notificationTypes);
        }
    }
}