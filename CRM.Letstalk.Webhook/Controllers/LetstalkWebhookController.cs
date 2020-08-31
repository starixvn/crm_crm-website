namespace CRM.Letstalk.Webhook.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.Interfaces;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Microsoft.AspNetCore.Mvc;

    public class LetstalkWebhookController : ControllerBase
    {
        private readonly IWebhookReceiver webhookReceiver;
        private readonly Func<int, ILetstalkService> letstalkService;
        private readonly ILogger logger;
        private readonly List<WebhookConfiguration> webhookConfigurations;

        public LetstalkWebhookController(IWebhookReceiver webhookReceiver,
            Func<int, ILetstalkService> letstalkService,
            ILogger logger,
            List<WebhookConfiguration> webhookConfigurations)
        {
            this.webhookReceiver = webhookReceiver;
            this.letstalkService = letstalkService;
            this.logger = logger;
            this.webhookConfigurations = webhookConfigurations;
        }

        [Route("api/webhook/{id}")]
        public async Task WebhookReceiver(int id)
        {
            try
            {
                await webhookReceiver.ReceiveAsync(Request, id);
            }
            catch (Exception ex)
            {
                await WriteLog(ex);
            }
        }

        private async Task WriteLog(Exception exception)
        {
            var logMessage = new LogMessage(exception.Message)
                        .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                        .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                        .ToString();
            await logger.ErrorAsync(logMessage);
        }

        [Route("api/setwebhook/{id}")]
        public async Task<IActionResult> SetWebhook(int id)
        {
            var botConfigs = webhookConfigurations.FirstOrDefault(config => config.BotId == id);
            if (botConfigs != null && botConfigs.IsAllowedSet)
            {
                var result = await letstalkService(id).SetWebhookAsync(botConfigs.WebhookUrl);
                return Ok(result);
            }

            return NotFound();
        }
    }
}