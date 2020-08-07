namespace CRM.Services
{
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationType;
    using CRM.Models.NotificationType;
    using Fanex.NAP.Client.Helpers.Web;

    public interface INotificationTypeService
    {
        Task<CommandResult> CreateAsync(CreateNotificationType model);

        Task<CommandResult> EditAsync(EditNotificationType model);

        Task<CommandResult> DeleteAsync(int id);
    }

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
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }

        public async Task<CommandResult> EditAsync(EditNotificationType model)
        {
            var command = new EditNotificationTypeCommand
            {
                NotificationTypeId = model.NotificationTypeId
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }

        public async Task<CommandResult> DeleteAsync(int id)
        {
            var command = new DeleteNotificationTypeCommand
            {
                NotificationTypeId = id
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }
    }
}