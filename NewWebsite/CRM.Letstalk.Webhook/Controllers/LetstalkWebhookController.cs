namespace CRM.Letstalk.Webhook.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class LetstalkWebhookController : ControllerBase
    {
        private readonly IWebhookReceiver webhookReceiver;
        private readonly ICRMLogService cRMLogService;
        private readonly Func<int, ILetstalkService> letstalkService;
        private readonly List<WebhookConfiguration> webhookConfigurations;

        public LetstalkWebhookController(IWebhookReceiver webhookReceiver,
            Func<int, ILetstalkService> letstalkService,
            List<WebhookConfiguration> webhookConfigurations,
            ICRMLogService cRMLogService)
        {
            this.webhookReceiver = webhookReceiver;
            this.letstalkService = letstalkService;
            this.webhookConfigurations = webhookConfigurations;
            this.cRMLogService = cRMLogService;
        }

        [Route("api/webhook/{id}")]
        public async Task WebhookReceiver(int id)
        {
            cRMLogService.LogInformation(JsonConvert.SerializeObject("The request is coming"));
            await webhookReceiver.ReceiveAsync(Request, id);
        }

        [Route("api/setwebhook/{id}")]
        public async Task<IActionResult> SetWebhook(int id)
        {
            var botConfigs = webhookConfigurations.FirstOrDefault(config => config.BotId == id);
            if (botConfigs != null && botConfigs.IsAllowedSet)
            {
                var webhookUrl = $"{botConfigs.WebhookBaseUrl}api/webhook/{id}";
                var result = await letstalkService(id).SetWebhookAsync(webhookUrl);
                return Ok(result);
            }

            return NotFound();
        }

        [Route("api/getwebhook/{id}")]
        public async Task<IActionResult> GetWebhook(int id)
        {
            var botConfigs = webhookConfigurations.FirstOrDefault(config => config.BotId == id);
            if (botConfigs != null && botConfigs.IsAllowedGet)
            {
                var result = await letstalkService(id).GetWebhookAsync();
                cRMLogService.LogInformation(JsonConvert.SerializeObject(result));
                return Ok(result);
            }

            return NotFound();
        }
    }
}