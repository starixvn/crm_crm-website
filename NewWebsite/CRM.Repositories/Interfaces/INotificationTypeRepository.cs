namespace CRM.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Models.NotificationType;
    using Fanex.Data.Repository;

    public interface INotificationTypeRepository
    {
        Task<IEnumerable<NotificationType>> GetNotificationTypes(ICriteria criteria);
    }
}