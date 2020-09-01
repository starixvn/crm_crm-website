namespace CRM.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;

    public interface IWriteLogService
    {
        Task WriteErrorLogAsync(Exception exception);
    }

    public class WriteLogService : IWriteLogService
    {
        private readonly ILogger logger;

        public WriteLogService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task WriteErrorLogAsync(Exception exception)
        {
            var logMessage = new LogMessage(exception.Message)
                        .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                        .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                        .ToString();
            await logger.ErrorAsync(logMessage);
        }
    }
}