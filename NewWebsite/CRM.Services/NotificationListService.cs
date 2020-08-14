using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Criteria.NotificationList;
using CRM.Models.NotificationList;
using CRM.Repositories;

namespace CRM.Services
{
    public class NotificationListService : INotificationListService
    {
        private readonly INotificationListRepository notificationListRepository;

        public NotificationListService(INotificationListRepository notificationListRepository)
        {
            this.notificationListRepository = notificationListRepository;
        }

        public async Task<IEnumerable<Notification>> GetNotificationList(GetNotificationListCriteria criteria)
        {
            var result = await notificationListRepository.GetNotifications(criteria);
            return result;
        }
    }
}