namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationTemplate;
    using CRM.Models.NotificationTemplate;
    using Fanex.NAP.Client.Helpers.Web;

    public interface INotificationTemplateService
    {
        Task<IEnumerable<NotificationTemplate>> GetAsync(GetNotificationTemplateParam param);

        Task<NotificationTemplate> GetByIdAsync(int id);

        Task<CommandResult> CreateAsync(CreateNotificationTemplate model);

        Task<CommandResult> EditAsync(EditNotificationTemplate model);

        Task<CommandResult> DeleteAsync(DeleteNotificationTemplate model);
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

        public async Task<IEnumerable<NotificationTemplate>> GetAsync(GetNotificationTemplateParam param)
        {
            var criteria = new GetNotificationTemplateCriteria
            {
                NotificationTypeId = param.NotificationTypeId,
                NotificationTemplateName = param.NotificationTemplateName,
                PageIndex = param.PageIndex,
                PageSize = param.PageSize
            };
            var templates = await baseService.GetDataAsync<NotificationTemplate>(criteria);
            param.TotalRows = criteria.TotalRows;

            return templates;
        }

        public async Task<NotificationTemplate> GetByIdAsync(int id)
        {
            var criteria = new GetNotificationTemplateCriteria
            {
                NotificationTemplateId = id
            };
            var tuple = await baseService.FetchMultipleAsync<NotificationTemplate, NotificationTemplateContent>(criteria);
            var template = tuple.Item1.FirstOrDefault();
            if (template != null)
            {
                template.Contents = tuple.Item2.Select(x => new NotificationTemplateContent
                {
                    LanguageId = x.LanguageId,
                    Language = x.Language,
                    Content = HttpUtility.HtmlDecode(x.Content)
                });
            }

            return template;
        }

        public async Task<CommandResult> CreateAsync(CreateNotificationTemplate model)
        {
            var command = new CreateNotificationTemplateCommand
            {
                NotificationTypeId = model.NotificationTypeId,
                NotificationTemplateName = model.NotificationTemplateName?.Trim(),
                Title = model.Title?.Trim(),
                UserId = napSession.UserId,
                ContentXml = model.Contents.Any() ? model.Contents.ToXML("Contents").ToString() : string.Empty
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }

        public async Task<CommandResult> EditAsync(EditNotificationTemplate model)
        {
            var command = new EditNotificationTemplateCommand
            {
                NotificationTemplateId = model.NotificationTemplateId,
                NotificationTypeId = model.NotificationTypeId,
                NotificationTemplateName = model.NotificationTemplateName?.Trim(),
                Title = model.Title?.Trim(),
                ContentXml = model.Contents.Any() ? model.Contents.ToXML("Contents").ToString() : string.Empty,
                UserId = napSession.UserId
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }

        public async Task<CommandResult> DeleteAsync(DeleteNotificationTemplate model)
        {
            var command = new DeleteNotificationTemplateCommand
            {
                NotificationTemplateId = model.NotificationTemplateId,
                NotificationTemplateName = model.NotificationTemplateName,
                UserId = napSession.UserId
            };

            return await baseService.ExecuteNonQueryAsync(command);
        }
    }
}