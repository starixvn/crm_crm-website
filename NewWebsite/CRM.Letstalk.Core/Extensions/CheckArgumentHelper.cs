namespace CRM.Letstalk.Core.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using CRM.Letstalk.Client.Configurations;

    public static class CheckArgumentHelper
    {
        public static void CheckValidBot(List<BotConfig> botConfigs, int botId)
        {
            if (!botConfigs.Any(config => config.BotAccountId == botId))
            {
                throw new KeyNotFoundException($"Not found the bot ID {botId} in configurations!");
            }
        }
    }
}