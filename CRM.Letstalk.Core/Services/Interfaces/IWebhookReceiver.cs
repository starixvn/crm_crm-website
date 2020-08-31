namespace CRM.Letstalk.Core.Services.Interfaces
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IWebhookReceiver
    {
        Task ReceiveAsync(HttpRequest request, int botId);
    }
}