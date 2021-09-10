namespace Fanex.CRM.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITicketRefService
    {
        Task<string> GetTicketByRefNo(int refno); 
    }

    public class TicketRefService: ITicketRefService
    {
        private static string NapUrl => ConfigurationManager.AppSettings["NapUrl"];
        private static string APIKey => ConfigurationManager.AppSettings["APIKey"];
        public async Task<string> GetTicketByRefNo(int refno)
        {
             
            CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
            HttpClient client = HttpClientFactory.Create(customDelegatingHandler);
            var response = await client.GetStringAsync(NapUrl + "BetListApi/GetBetListByRefNo?refno=" + 1000);
            return response;
        }
    }
}
