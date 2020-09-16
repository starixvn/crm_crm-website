namespace CRM.Letstalk.Services.Services
{
    using System;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Services.Constants;
    using CRM.Letstalk.Services.Models;
    using CRM.Letstalk.Services.Services.Interfaces;

    public class MonitorService : IMonitorService
    {
        private readonly MonitorSettings monitorSettings;
        private readonly Func<int, ILetstalkService> letstalkService;

        public MonitorService(MonitorSettings monitorSettings, Func<int, ILetstalkService> letstalkService)
        {
            this.monitorSettings = monitorSettings;
            this.letstalkService = letstalkService;
        }

        public async Task MonitorLetstalkService()
        {
            if (monitorSettings.Mode.Equals(MonitorMode.GetWebhook, StringComparison.InvariantCultureIgnoreCase))
            {
                await letstalkService(monitorSettings.BotId).GetWebhookAsync();
            }
            else
            {
                var listLetstalk = monitorSettings.LetstalkIds.Split(',');
                await letstalkService(monitorSettings.BotId).PushAsync(listLetstalk, monitorSettings.Message);
            }
        }
    }
}