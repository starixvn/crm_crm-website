namespace CRM.Core.UnitTests.Services
{
    using System;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using NSubstitute;
    using Xunit;

    public class CrmLogServiceTests
    {
        private readonly CRMLogService service;
        private readonly ILogger<CRMLogService> filelogger;
        private readonly Fanex.Logging.ILogger logger;

        public CrmLogServiceTests()
        {
            filelogger = Substitute.For<ILogger<CRMLogService>>();
            logger = Substitute.For<Fanex.Logging.ILogger>();
            service = new CRMLogService(logger, filelogger);
        }

        [Fact]
        public async Task ErrorAsync_WithException_WillCallsFanexLogError()
        {
            var exception = new Exception("This is message");
            await service.ErrorAsync(exception);

            await logger.Received(1).ErrorAsync(Arg.Any<string>());
        }

        [Fact]
        public async Task ErrorAsync_Command_WillCallsFanexLogErrorWithExpectedMessage()
        {
            var command = new CommandResult { Code = 1, Message = "This is error messgae" };
            var criteria = Substitute.For<ICriteria>();
            var logMessage = new LogMessage(string.Format("{0}: {1}", command.Code, command.Message))
                       .AddMessage(JsonConvert.SerializeObject(criteria, Formatting.Indented))
                       .ToString();
            await service.ErrorAsync(command, criteria);

            await logger.Received(1).ErrorAsync(Arg.Is(logMessage));
        }

        [Fact]
        public void LogInformation_WithMessage_WillCallsFileLogInformat()
        {
            service.LogInformation("This is error message");
            filelogger.ReceivedCalls();
        }
    }
}