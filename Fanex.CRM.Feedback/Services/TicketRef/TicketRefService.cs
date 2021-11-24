namespace Fanex.CRM.Feedback.Services.TicketRef
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Common.Services;
    using Newtonsoft.Json;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Logging;
    using Fanex.CRM.Common.Helpers;
    using System.Collections.Generic;
    using System.Threading;

    public interface ITicketRefService
    {
        Task<TicketInfo> GetTicketByRefNoAsync(long refno);

        DateTime GetBalanceUpday();
    }

    public class TicketRefService: ITicketRefService
    {
        private readonly IBaseService baseService;
        private const short timeSleep = 2;
        private const short retryCount = 3;
        public TicketRefService(): this(null)
        {
        }

        public TicketRefService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }
        private static string NapUrl => ConfigurationManager.AppSettings["NapUrl"];
        public async Task<TicketInfo> GetTicketByRefNoAsync(long refno)
        {
            return await DoWithRetryAsync(() => GetTicketInfo(refno), TimeSpan.FromSeconds(timeSleep), tryCount: retryCount);
        }

        private static async Task<TicketInfo> GetTicketInfo(long refno)
        {
            CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
            HttpClient client = HttpClientFactory.Create(customDelegatingHandler);
            var url = new Uri(NapUrl + "BetListApi/GetBetListByRefNo?refno=" + refno);
            var response = await client.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            if (jsonString != null)
            {
                return JsonConvert.DeserializeObject<TicketInfo>(jsonString);
            }
            else
            {
                return await Task.FromResult<TicketInfo>(null);
            }
        }

        private static async Task<TicketInfo> DoWithRetryAsync(Func<Task<TicketInfo>> func, TimeSpan sleepPeriod, int tryCount = 3)
        {
            while (true)
            {
                try
                {
                    return await func();
                }
                catch
                {
                    if (tryCount-- == 0)
                    {
                        throw;
                    }
                    await Task.Delay(sleepPeriod);
                }
            }
        }

        public DateTime GetBalanceUpday()
        {
            var criteria = new BalanceUpDayCriteria();
            var balance = baseService.GetDataEntity<BalanceUpday>(criteria);
            if(balance != null)
            {
                return balance.BalanceUpDay;
            }
            return new DateTime();
        }
    }
}
