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
        public async Task<IEnumerable<Notification>> GetNotificationsAsync(ICriteria criteria)
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

        public async Task<ValidationNotificationList> ValidateNotificationListAsync(ICriteria criteria)
        {
            using (var objectDB = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (IMultiResultReader multi = await objectDB.QueryMultipleAsync(criteria))
                {
                    var inActiveNotificationIds = multi.Read<int>();
                    var resendDataList = multi.Read<NotificationListResend>();
                    var result = new ValidationNotificationList
                    {
                        InActiveNotificationIds = inActiveNotificationIds,
                        ResendDataList = resendDataList
                    };

                    return result;
                }
            }
        }
    }
}