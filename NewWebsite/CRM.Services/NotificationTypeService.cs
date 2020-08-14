namespace CRM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationType;
    using CRM.Models.NotificationType;
    using Fanex.NAP.Client.Helpers.Web;

    public interface INotificationTypeService
    {
        Task<IEnumerable<NotificationType>> GetAsync(GetGridDataParam param);

        Task<NotificationType> GetByIdAsync(int id);

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

        public async Task<IEnumerable<NotificationType>> GetAsync(GetGridDataParam param)
        {
            var criteria = new GetNotificationTypeCriteria
            {
                RecipientTypeId = param.RecipientTypeId,
                ChatTypeId = param.ChatTypeId,
                SystemId = param.SystemId,
                NotificationTypeName = param.NotificationTypeName,
                PageIndex = param.PageIndex,
                PageSize = param.PageSize
            };
            var types = await baseService.GetDataAsync<NotificationType>(criteria);
            param.TotalRows = criteria.TotalRows;

            return types;
        }

        public async Task<NotificationType> GetByIdAsync(int id)
        {
            var type = new NotificationType() ;
            try
            {
                var criteria = new GetNotificationTypeCriteria
                {
                    NotificationTypeId = id
                };

                type = await baseService.GetDataEntityAsync<NotificationType>(criteria);
            }
            catch { 
                Exception ex; 
            }

            return type;
        }
        
        public async Task<CommandResult> CreateAsync(CreateNotificationType model)
        {
            var command = new CreateNotificationTypeCommand
            {
                NotificationTypeName = model.NotificationTypeName,
                ChatTypeId = model.ChatTypeId,
                RecipientTypeId = model.RecipientTypeId,
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
                ChatTypeId = model.ChatTypeId,
                RecipientTypeId = model.RecipientTypeId,
                SystemId = model.SystemId
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
    }
}