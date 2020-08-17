namespace CRM.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Enumerables;
    using CRM.Models;
    using CRM.Models.NotificationType;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class NotificationTypeRepository : INotificationTypeRepository
    {
        public async Task<IEnumerable<NotificationType>> GetNotificationTypes(ICriteria criteria)
        {
            using (var objectDB = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (IMultiResultReader multi = await objectDB.QueryMultipleAsync(criteria))
                {
                    var notificationTypes = multi.Read<NotificationType>();
                    var recipientTypes = multi.Read<RecipientType>();
                    var chatTypes = multi.Read<ChatTypes>();
                    foreach (var chatType in chatTypes)
                    {
                        chatType.ChatTypeName = chatType.ChatTypeId == (int)ChatType.Group ? ChatType.Group.ToString() : ChatType.Personal.ToString();
                    }
                    foreach (var notificationType in notificationTypes)
                    {
                        notificationType.RecipientTypes = recipientTypes.Where(recipientType => notificationType.NotificationTypeId == recipientType.NotificationTypeId);
                        notificationType.ChatTypes = chatTypes.Where(chatType => notificationType.NotificationTypeId == chatType.NotificationTypeId);
                    }

                    return notificationTypes;
                }
            }
        }
    }
}