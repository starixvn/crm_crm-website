namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationList;
    using CRM.Models.NotificationList;
    using CRM.Repositories;

    public class NotificationListService : INotificationListService
    {
        private readonly INotificationListRepository notificationListRepository;
        private readonly IBaseService baseService;

        public NotificationListService(
            INotificationListRepository notificationListRepository,
            IBaseService baseService)
        {
            this.notificationListRepository = notificationListRepository;
            this.baseService = baseService;
        }

        public async Task<IEnumerable<Notification>> GetNotificationList(GetNotificationListCriteria criteria)
        {
            var result = await notificationListRepository.GetNotifications(criteria);
            return result;
        }

        public async Task<CommandResult> EditNotificationList(EditNotificationListCommand command)
        {
            return await baseService.ExecuteNonQueryAsync(command);
        }
    }
}