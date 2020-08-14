namespace CRM.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Models;
    using CRM.Models.NotificationList;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class NotificationListRepository : INotificationListRepository
    {
        public async Task<IEnumerable<Notification>> GetNotifications(ICriteria criteria)
        {
            using (var objectDB = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (IMultiResultReader multi = await objectDB.QueryMultipleAsync(criteria))
                {
                    var notifications = multi.Read<Notification>();
                    var recipients = multi.Read<Recipient>();
                    var headlines = multi.Read<Headline>();
                    foreach (var notification in notifications)
                    {
                        notification.Recipients = recipients.Where(recipient => notification.NotificationId == recipient.NotificationId);
                        notification.Headlines = headlines.Where(headline => notification.NotificationId == headline.NotificationId);
                    }

                    return notifications;
                }
            }
        }
    }
}