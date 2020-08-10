namespace CRM.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationTemplate;
    using CRM.Models.NotificationTemplate;
    using Fanex.NAP.Client.Helpers.Web;

    public interface INotificationTemplateService
    {
        Task<CommandResult> CreateAsync(CreateNotificationTemplate model);

        Task<CommandResult> EditAsync(EditNotificationTemplate model);

        Task<CommandResult> DeleteAsync(int id);
    }

    public class NotificationTemplateService : INotificationTemplateService
    {
        private readonly INapSession napSession;
        private readonly IBaseService baseService;

        public NotificationTemplateService(
            INapSession napSession,
            IBaseService baseService)
        {
            this.napSession = napSession;
            this.baseService = baseService;
        }

        public async Task<CommandResult> CreateAsync(CreateNotificationTemplate model)
        {
            var command = new CreateNotificationTemplateCommand
            {
                NotificationTypeId = model.NotificationTypeId,
                Name = model.Name?.Trim(),
                Title = model.Title?.Trim(),
                CreatorId = napSession.UserId,
                ContentXml = model.Contents.Any() ? model.Contents.ToXML().ToString() : string.Empty
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }

        public async Task<CommandResult> EditAsync(EditNotificationTemplate model)
        {
            var command = new EditNotificationTemplateCommand
            {
                NotificationTemplateId = model.NotificationTemplateId,
                NotificationTypeId = model.NotificationTypeId,
                Name = model.Name?.Trim(),
                Title = model.Title?.Trim(),
                ContentXml = model.Contents.Any() ? model.Contents.ToXML().ToString() : string.Empty,
                ModifierId = napSession.UserId
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }

        public async Task<CommandResult> DeleteAsync(int id)
        {
            var command = new DeleteNotificationTemplateCommand
            {
                NotificationTemplateId = id,
                UserId = napSession.UserId
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }
    }
}