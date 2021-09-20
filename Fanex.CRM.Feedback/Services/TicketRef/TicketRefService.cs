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

    public interface ITicketRefService
    {
        Task<TicketInfo> GetTicketByRefNoAsync(long refno);

        DateTime GetBalanceUpday();
    }

    public class TicketRefService: ITicketRefService
    {
        private readonly IBaseService baseService;
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
            try
            {
                CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
                HttpClient client = HttpClientFactory.Create(customDelegatingHandler);
                var response = await client.GetAsync(NapUrl + "BetListApi/GetBetListByRefNo?refno=" + refno);
                LogService.Info("URL: " + response);
                var jsonString = await response.Content.ReadAsStringAsync();
                LogService.Info("jsonString: " + jsonString);
                if (jsonString != null)
                {
                    return JsonConvert.DeserializeObject<TicketInfo>(jsonString);
                }
                else
                {
                    return await Task.FromResult<TicketInfo>(null);
                }
            }
            catch(Exception e)
            {
                LogService.Error("GetTicketInfoByRefNoAsync Error: " + e.Message, e.StackTrace);
                throw;
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
