namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Enumerables;
    using CRM.Core.Extensions;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationType;
    using CRM.Models;
    using CRM.Models.NotificationType;
    using Fanex.NAP.Client.Helpers.Web;

    public class NotificationTypeService : INotificationTypeService
    {
        private readonly INapSession napSession;
        private readonly IBaseService baseService;

        public NotificationTypeService(
            INapSession napSession,
            IBaseService baseService)
        {
            this.napSession = napSession;
            this.baseService = baseService;
        }

        public async Task<CommandResult> CreateAsync(CreateNotificationType model)
        {
            var command = new CreateNotificationTypeCommand
            {
                NotificationTypeName = model.NotificationTypeName,
                ChatTypeIds = model.ChatTypeIds,
                RecipientTypeIds = model.RecipientTypeIds,
                SystemId = model.SystemId,
                UserId = napSession.UserId
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }

        public async Task<CommandResult> EditAsync(EditNotificationType model)
        {
            var command = new EditNotificationTypeCommand
            {
                NotificationTypeId = model.NotificationTypeId,
                NotificationTypeName = model.NotificationTypeName,
                ChatTypeIds = model.ChatTypeIds,
                RecipientTypeIds = model.RecipientTypeIds,
                SystemId = model.SystemId,
                UserId = napSession.UserId
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }

        public async Task<CommandResult> DeleteAsync(int id)
        {
            var command = new DeleteNotificationTypeCommand
            {
                NotificationTypeId = id,
                UserId = napSession.UserId
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }

        public async Task<IEnumerable<NotificationType>> GetNotificationTypeAsync(GetNotificationTypeCriteria criteria)
        {
            var data = await baseService.FetchMultipleAsync<NotificationType, RecipientType, NotificationChatType>(criteria);
            if (data.Item1.Any())
            {
                foreach (var item in data.Item3)
                {
                    item.ChatTypeName = EnumHelper.GetDesciption((ChatType)item.ChatTypeId);
                }
                foreach (var item in data.Item1)
                {
                    item.RecipientTypes = data.Item2.Where(recipientType => recipientType.NotificationTypeId == item.NotificationTypeId);
                    item.ChatTypes = data.Item3.Where(chatType => chatType.NotificationTypeId == item.NotificationTypeId);
                }
            }

            return data.Item1;
        }
    }
}