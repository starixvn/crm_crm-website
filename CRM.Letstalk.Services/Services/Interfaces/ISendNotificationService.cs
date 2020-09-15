namespace CRM.Letstalk.Services.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface ISendNotificationService
    {
        Task SendNotificationToLetstalk();
    }
}