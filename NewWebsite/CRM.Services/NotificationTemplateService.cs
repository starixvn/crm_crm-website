namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationTemplate;
    using CRM.Models.NotificationTemplate;
    using Fanex.NAP.Client.Helpers.Web;

    public interface INotificationTemplateService
    {
        Task<IEnumerable<NotificationTemplate>> GetAsync(GetGridDataParam param);

        Task<NotificationTemplate> GetByIdAsync(int id);

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

        public async Task<IEnumerable<NotificationTemplate>> GetAsync(GetGridDataParam param)
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
                ContentXml = model.Contents.Any() ? ParseContentToXml(model.Contents) : string.Empty
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
                ContentXml = model.Contents.Any() ? ParseContentToXml(model.Contents) : string.Empty,
                UserId = napSession.UserId
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

        private static string ParseContentToXml(IEnumerable<NotificationTemplateContent> contents)
        {
            const string XmlTemplate = "<Content LanguageId='{0}' Value='{1}' />";
            var stringBuilder = new StringBuilder("<Contents>");
            foreach (var content in contents)
            {
                stringBuilder.AppendFormat(XmlTemplate, content.LanguageId, HttpUtility.HtmlEncode("aaa\nbbb\nccc"));
            }

            stringBuilder.Append("</Contents>");
            return stringBuilder.ToString();
        }
    }
}