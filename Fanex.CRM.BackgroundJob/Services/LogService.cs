namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Newtonsoft.Json;

    public interface ICRMLogService
    {
        Task ErrorAsync(Exception exception);

        Task ErrorAsync(CommandResult commandResult, ICriteria criteria);
    }

    public class CRMLogService : ICRMLogService
    {
        private readonly ILogger logger;

        public CRMLogService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task ErrorAsync(Exception exception)
        {
            var logMessage = new LogMessage(exception.Message)
                       .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                       .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                       .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                       .ToString();
            await logger.ErrorAsync(logMessage);
        }

        public async Task ErrorAsync(CommandResult commandResult, ICriteria criteria)
        {
            var logMessage = new LogMessage(string.Format("{0}: {1}", commandResult.Code, commandResult.Message))
                       .AddMessage(JsonConvert.SerializeObject(criteria, Formatting.Indented))
                       .ToString();
            await logger.ErrorAsync(logMessage);
        }
    }
}