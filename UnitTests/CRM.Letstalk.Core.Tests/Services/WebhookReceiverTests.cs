namespace CRM.Letstalk.Core.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Constants;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services;
    using CRM.Letstalk.Core.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
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

        [Fact]
        public async Task ReceiveAsync_WhenCallWithValidBotId_WillCallsExcpectedServiceWithExpectArguments()
        {
            var data = "TestingToken";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(data));
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = stream;

            var hookEvent = new HookEvent
            {
                Type = EventType.JoinGroup,
                PayloadObject = new Payload(),
                Source = new EventSource { Type = "group", GroupId = "Group1" }
            };

            jwtService.Decode(Arg.Any<string>(), Arg.Any<string>()).Returns(JsonConvert.SerializeObject(hookEvent));

            await webhookReceiver.ReceiveAsync(httpContext.Request, 1000);

            jwtService.Received().Decode(Arg.Is(botConfigs.FirstOrDefault().SecureKey), Arg.Is(data));
        }
    }
}