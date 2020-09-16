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

    public class AddContactEventHandlerTests
    {
        private readonly AddContactEventHandler addContactEventHandler;
        private readonly ILetstalkService letstalkService;

        public AddContactEventHandlerTests()
        {
            var func = Substitute.For<Func<int, ILetstalkService>>();
            letstalkService = Substitute.For<ILetstalkService>();
            func(Arg.Any<int>()).Returns(letstalkService);
            addContactEventHandler = new AddContactEventHandler(func);
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
                BotUserId = "@ow_local"
            };

            await addContactEventHandler.EventHandleAsync(hookEvent, botConfig);

            await letstalkService.Received().PushAsync(Arg.Is(hookEvent.Source.UserId), Arg.Is(string.Format("Hello, I am {0}!", botConfig.BotUserId)));
        }
    }
}