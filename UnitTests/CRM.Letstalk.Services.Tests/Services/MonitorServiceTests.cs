namespace CRM.Letstalk.Services.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Services.Constants;
    using CRM.Letstalk.Services.Models;
    using CRM.Letstalk.Services.Services;
    using NSubstitute;
    using Xunit;

    public class MonitorServiceTests
    {
        private readonly MonitorService monitorService;
        private readonly MonitorSettings monitorSettings;
        private readonly ILetstalkService letstalkService;
        private readonly ICRMLogService cRMLogService;

        public MonitorServiceTests()
        {
            var func = Substitute.For<Func<int, ILetstalkService>>();
            letstalkService = Substitute.For<ILetstalkService>();
            cRMLogService = Substitute.For<ICRMLogService>();
            func(Arg.Any<int>()).Returns(letstalkService);

            monitorSettings = new MonitorSettings { Mode = MonitorMode.GetWebhook, BotId = 10, LetstalkIds = "abc,def", Message = "Hi" };

            monitorService = new MonitorService(monitorSettings, func, cRMLogService);
        }

        [Fact]
        public async Task MonitorLetstalkService_WhenModeIsGetWebhook_CallsGetWebhookApi()
        {
            await monitorService.MonitorLetstalkService();

            await letstalkService.Received().GetWebhookAsync();
        }

        [Fact]
        public async Task MonitorLetstalkService_WhenModeIsPushMessage_CallsPushApi()
        {
            monitorSettings.Mode = MonitorMode.PushMessage;
            await monitorService.MonitorLetstalkService();

            await letstalkService.Received().PushAsync(Arg.Any<string[]>(), Arg.Is(monitorSettings.Message));
        }
    }
}