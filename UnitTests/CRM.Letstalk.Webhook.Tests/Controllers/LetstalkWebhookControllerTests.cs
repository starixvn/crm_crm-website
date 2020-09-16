namespace CRM.Letstalk.Webhook.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Models.Results;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.Interfaces;
    using CRM.Letstalk.Webhook.Controllers;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;

    public class LetstalkWebhookControllerTests
    {
        private readonly IWebhookReceiver webhookReceiver;
        private readonly ILetstalkService letstalkService;
        private readonly List<WebhookConfiguration> webhookConfigurations;
        private readonly LetstalkWebhookController controller;

        public LetstalkWebhookControllerTests()
        {
            webhookReceiver = Substitute.For<IWebhookReceiver>();
            var func = Substitute.For<Func<int, ILetstalkService>>();
            letstalkService = Substitute.For<ILetstalkService>();
            func(Arg.Any<int>()).Returns(letstalkService);
            webhookConfigurations = new List<WebhookConfiguration>
            {
                new WebhookConfiguration {BotId = 1, IsAllowedGet = true, IsAllowedSet = true, WebhookBaseUrl = "abc.com/"},
                new WebhookConfiguration {BotId = 2, IsAllowedGet = false, IsAllowedSet = false, WebhookBaseUrl = "test.com/"}
            };

            controller = new LetstalkWebhookController(webhookReceiver, func, webhookConfigurations);
        }

        [Fact]
        public async Task WebhookReceiver_WhenCalledWithAnyParam_CallsExpectedWebhookReciverService()
        {
            await controller.WebhookReceiver(1);
            var request = Substitute.For<HttpRequest>();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            await webhookReceiver.Received().ReceiveAsync(Arg.Any<HttpRequest>(), Arg.Is(1));
        }

        [Fact]
        public async Task SetWebhook_WhenInCorrectBotId_ReturnNotFound()
        {
            var actual = await controller.SetWebhook(10);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task SetWebhook_WhenCorrectBotIdButConfigNotAllowSet_ReturnNotFound()
        {
            var actual = await controller.SetWebhook(2);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task SetWebhook_WhenCorrectBotIdAndConfigIsAllowSet_ReturnNotFound()
        {
            var result = new BaseResult { Status = 1, Message = "Success" };
            letstalkService.SetWebhookAsync(Arg.Any<string>()).Returns(result);

            var actual = await controller.SetWebhook(1);

            await letstalkService.SetWebhookAsync(Arg.Is("test.com/api/webhook/2"));
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(result, okResult.Value);
        }

        [Fact]
        public async Task GetWebhook_WhenInCorrectBotId_ReturnNotFound()
        {
            var actual = await controller.GetWebhook(10);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task GetWebhook_WhenCorrectBotIdButConfigNotAllowSet_ReturnNotFound()
        {
            var actual = await controller.GetWebhook(2);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task GetWebhook_WhenCorrectBotIdAndConfigIsAllowSet_ReturnNotFound()
        {
            var result = "webhook URL";
            letstalkService.GetWebhookAsync().Returns(result);

            var actual = await controller.GetWebhook(1);

            await letstalkService.GetWebhookAsync();
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(result, okResult.Value);
        }
    }
}