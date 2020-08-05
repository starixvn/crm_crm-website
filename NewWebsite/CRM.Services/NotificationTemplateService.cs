namespace CRM.Services
{
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationTemplate;
    using CRM.Models.NotificationTemplate;
    using Fanex.NAP.Client.Helpers.Web;

    public interface INotificationTemplateService
    {
        Task<CommandResult> CreateAsync(CreateNotificationTemplate model);
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
                ContentEN = model.ContentEN?.Trim(),
                ContentSC = model.ContentSC?.Trim(),
                ContentENSC = model.ContentENSC?.Trim(),
                IsCombineContentENSC = model.IsCombineContentENSC,
                CreatorId = napSession.UserId
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }
    }
}