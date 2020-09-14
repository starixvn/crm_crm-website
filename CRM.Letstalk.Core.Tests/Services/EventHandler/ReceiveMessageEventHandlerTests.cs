namespace CRM.Letstalk.Core.Tests.Services.EventHandler
{
    using System;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.EventHandler;
    using NSubstitute;
    using Xunit;

    public class ReceiveMessageEventHandlerTests
    {
        private readonly ReceiveMessageEventHandler receiveMessageEventHandler;
        private readonly ILetstalkService letstalkService;

        public ReceiveMessageEventHandlerTests()
        {
            var func = Substitute.For<Func<int, ILetstalkService>>();
            letstalkService = Substitute.For<ILetstalkService>();
            func(Arg.Any<int>()).Returns(letstalkService);
            receiveMessageEventHandler = new ReceiveMessageEventHandler(func);
        }

        [Fact]
        public async Task EventHandleAsync_WhenCalled_WillCallsExpectedServiceWithExpectedArgurment()
        {
            var hookEvent = new HookEvent
            {
                Source = new EventSource
                {
                    UserId = "letstalk"
                }
            };

            var botConfig = new BotConfig
            {
                BotAccountId = 10000,
            };

            await receiveMessageEventHandler.EventHandleAsync(hookEvent, botConfig);

            await letstalkService.Received().PushAsync(Arg.Is(hookEvent.Source.UserId), Arg.Is("Hello, I am Bot."));
        }
    }
}