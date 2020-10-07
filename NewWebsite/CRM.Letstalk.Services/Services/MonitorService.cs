namespace CRM.Letstalk.Services.Services
{
    using System;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Services.Constants;
    using CRM.Letstalk.Services.Models;
    using CRM.Letstalk.Services.Services.Interfaces;
    using Newtonsoft.Json;

    public class MonitorService : IMonitorService
    {
        private readonly MonitorSettings monitorSettings;
        private readonly ICRMLogService cRMLogService;
        private readonly Func<int, ILetstalkService> letstalkService;

        public MonitorService(MonitorSettings monitorSettings, Func<int, ILetstalkService> letstalkService, ICRMLogService cRMLogService)
        {
            this.monitorSettings = monitorSettings;
            this.letstalkService = letstalkService;
            this.cRMLogService = cRMLogService;
        }

        public async Task MonitorLetstalkService()
        {
            cRMLogService.LogInformation("Service run");
            if (monitorSettings.Mode.Equals(MonitorMode.GetWebhook, StringComparison.InvariantCultureIgnoreCase))
            {
                var result = await letstalkService(monitorSettings.BotId).GetWebhookAsync();
                cRMLogService.LogInformation(result);
            }
            else
            {
                var listLetstalk = monitorSettings.LetstalkIds.Split(',');
                var results = await letstalkService(monitorSettings.BotId).PushAsync(listLetstalk, monitorSettings.Message);
                cRMLogService.LogInformation(JsonConvert.SerializeObject(results));
            }
        }
    }
}