namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Criteria.NotificationList;
    using CRM.Models.NotificationList;

    public interface INotificationListService
    {
        Task<IEnumerable<Notification>> GetNotificationList(GetNotificationListCriteria criteria);
    }
}