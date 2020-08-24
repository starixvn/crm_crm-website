namespace CRM.Controllers
{
    using System.Threading.Tasks;
    using CRM.Core.Filters;
    using CRM.Criteria.NotificationType;
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
            var notificationTypes = await notificationTypeService.GetNotificationTypeAsync(new GetNotificationTypeCriteria());
            return Json(notificationTypes);
        }
    }
}