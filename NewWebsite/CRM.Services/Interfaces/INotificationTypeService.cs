namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Criteria.NotificationType;
    using CRM.Models.NotificationType;

    public interface INotificationTypeService
    {
        Task<IEnumerable<NotificationType>> GetAsync(GetGridDataParam param);

        Task<CommandResult> CreateAsync(CreateNotificationType model);

        Task<CommandResult> EditAsync(EditNotificationType model);

        Task<CommandResult> DeleteAsync(int id);

        Task<IEnumerable<NotificationType>> GetNotificationTypeAsync(GetNotificationTypeCriteria criteria);
    }
}