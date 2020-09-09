namespace CRM.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface ISendNotificationService
    {
        Task SendNotificationToLetstalk();
    }
}