namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationType;
    using CRM.Models.NotificationType;
    using CRM.Repositories;
    using Fanex.NAP.Client.Helpers.Web;

    public class NotificationTypeService : INotificationTypeService
    {
        private readonly INapSession napSession;
        private readonly IBaseService baseService;
        private readonly INotificationTypeRepository notificationTypeRepository;

        public NotificationTypeService(
            INapSession napSession,
            IBaseService baseService,
            INotificationTypeRepository notificationTypeRepository)
        {
            this.napSession = napSession;
            this.baseService = baseService;
            this.notificationTypeRepository = notificationTypeRepository;
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
            var types = await notificationTypeRepository.GetNotificationTypes(criteria);

            return types;
        }
    }
}