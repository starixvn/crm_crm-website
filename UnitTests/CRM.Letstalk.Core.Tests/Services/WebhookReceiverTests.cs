namespace CRM.Letstalk.Core.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Services;
    using CRM.Letstalk.Core.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using NSubstitute;
    using Xunit;

    public class WebhookReceiverTests
    {
        private readonly IWebhookReceiver webhookReceiver;
        private readonly IJwtService jwtService;
        private readonly IEventHandler eventHandler;
        private readonly List<BotConfig> botConfigs;

        public WebhookReceiverTests()
        {
            jwtService = Substitute.For<IJwtService>();
            eventHandler = Substitute.For<IEventHandler>();
            var func = Substitute.For<Func<string, IEventHandler>>();
            func(Arg.Any<string>()).Returns(eventHandler);

            botConfigs = new List<BotConfig>
            {
                new BotConfig { BotAccountId = 1000, SecureKey = "SecureKey" }
            };
            webhookReceiver = new WebhookReceiver(jwtService, func, botConfigs);
        }

        [Fact]
        public void ReceiveAsync_WhenCallWithInValidBotId_ThrowsKeyNotFoundException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => webhookReceiver.ReceiveAsync(Arg.Any<HttpRequest>(), 1001));
        }
    }
}