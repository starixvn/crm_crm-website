namespace CRM.Letstalk.Core.Services.Interfaces
{
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Core.Models;

    public interface IEventHandler
    {
        Task EventHandleAsync(HookEvent hookEvent, BotConfig botConfig);
    }
}