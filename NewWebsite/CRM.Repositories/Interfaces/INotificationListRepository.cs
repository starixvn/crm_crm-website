namespace CRM.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Models.NotificationList;
    using Fanex.Data.Repository;

    public interface INotificationListRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsAsync(ICriteria criteria);
    }
}