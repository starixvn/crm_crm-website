namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Xml.Linq;
    using CRM.Core.Constants;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationTemplate;
    using CRM.Models.NotificationTemplate;
    using CRM.Models.Setting;
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
        private readonly ISettingService settingService;

        public NotificationTemplateService(
            INapSession napSession,
            IBaseService baseService,
            ISettingService settingService)
        {
            this.napSession = napSession;
            this.baseService = baseService;
            this.settingService = settingService;
        }

        public async Task<IEnumerable<NotificationTemplate>> GetAsync(GetGridDataParam param)
        {
            var criteria = new GetNotificationTemplateCriteria
            {
                NotificationTypeId = param.NotificationTypeId,
                TemplateName = param.TemplateName,
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
            var template = await baseService.GetDataEntityAsync<NotificationTemplate>(criteria);
            IEnumerable<Setting> languages = await GetLanguageAsync();
            ParseContentXml(template, languages);
            return template;
        }

        public async Task<CommandResult> CreateAsync(CreateNotificationTemplate model)
        {
            var command = new CreateNotificationTemplateCommand
            {
                NotificationTypeId = model.NotificationTypeId,
                Name = model.Name?.Trim(),
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
                Name = model.Name?.Trim(),
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

        private async Task<IEnumerable<Setting>> GetLanguageAsync()
        {
            var param = new GetSettingParam
            {
                SettingName = SettingName.ChatBotLanguage,
                IsActive = 1
            };
            return await settingService.GetAsync(param);
        }

        private void ParseContentXml(NotificationTemplate template, IEnumerable<Setting> languages)
        {
            template.Contents = languages.Select(x => new NotificationTemplateContent
            {
                LanguageId = x.SettingId,
                Language = x.SettingValue,
                Content = string.Empty
            }).ToList();
            if (!string.IsNullOrWhiteSpace(template.ContentXml))
            {
                var element = XElement.Parse(template.ContentXml);
                var contents = element.Elements("Content")
                    .Select(x => new NotificationTemplateContent
                    {
                        LanguageId = int.Parse(x.Attribute("LanguageId").Value),
                        Content = x.Attribute("Value").Value
                    }).ToList();
                foreach (var content in template.Contents)
                {
                    var item = contents.FirstOrDefault(x => x.LanguageId == content.LanguageId);
                    if (item != null)
                    {
                        content.Content = HttpUtility.HtmlDecode(item.Content);
                    }
                }
            }
        }

        private static string ParseContentToXml(IEnumerable<NotificationTemplateContent> contents)
        {
            const string XmlTemplate = "<Content LanguageId='{0}' Value='{1}' />";
            var stringBuilder = new StringBuilder("<Contents>");
            foreach (var content in contents)
            {
                stringBuilder.AppendFormat(XmlTemplate, content.LanguageId, HttpUtility.HtmlEncode(content.Content));
            }

            stringBuilder.Append("</Contents>");
            return stringBuilder.ToString();
        }
    }
}