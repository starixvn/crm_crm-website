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
    using Fanex.CRM.Setting.Models;

    public interface ITicketRefService
    {
        Task<TicketInfo> GetTicketByRefNoAsync(SiteDbGroup siteDb, RefTicketParams param);

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
        public async Task<TicketInfo> GetTicketByRefNoAsync(SiteDbGroup siteDb, RefTicketParams param)
        {
            CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
            HttpClient client = HttpClientFactory.Create(customDelegatingHandler);
            var url = new Uri(string.Format(NapUrl + "BetListApi/GetBetListByRefNo?RefNo={0}&Site={1}&SiteDBGroupId={2}", param.Refno, siteDb.Site, siteDb.SiteDBGroupID));
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
