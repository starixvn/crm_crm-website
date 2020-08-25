namespace CRM.Controllers
{
    using System.Threading.Tasks;
    using CRM.Core.Filters;
    using CRM.Criteria.Common;
    using CRM.Criteria.NotificationType;
    using CRM.Services;
    using Microsoft.AspNetCore.Mvc;

    [CrmAuthorize]
    public class CommonController : Controller
    {
        private readonly INotificationTypeService notificationTypeService;
        private readonly ICommonService commonService;

        public CommonController(
            INotificationTypeService notificationTypeService,
            ICommonService commonService)
        {
            this.notificationTypeService = notificationTypeService;
            this.commonService = commonService;
        }

        public async Task<IActionResult> GetNotificationTypes()
        {
            var notificationTypes = await notificationTypeService.GetNotificationTypeAsync(new GetNotificationTypeCriteria());
            return Json(notificationTypes);
        }

        [HttpPost]
        public async Task<IActionResult> GetHeadlines([FromBody]GetHeadlinesCriteria criteria)
        {
            var headlines = await commonService.GetHeadlines(criteria);
            return Json(headlines);
        }

        [HttpPost]
        public async Task<IActionResult> GetRecipients([FromBody]GetRecipientsCriteria criteria)
        {
            var headlines = await commonService.GetRecipients(criteria);
            return Json(headlines);
        }
    }
}