namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Criteria.NotificationList;
    using CRM.Models.NotificationList;

    public interface INotificationListService
    {
        Task<IEnumerable<Notification>> GetNotificationListAsync(GetNotificationListCriteria criteria);

        Task<CommandResult> EditNotificationListAsync(EditNotificationListCommand command);

        Task<ValidationNotificationListResult> ValidateNotificationListAsync(ValidateNotificationListCriteria criteria);

        Task<CommandResult> ResendNotificationsAsync(ResendNotificationsCommand command);
    }
}