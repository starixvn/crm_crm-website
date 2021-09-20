namespace Fanex.CRM.Feedback.Services.TicketRef
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    public class CustomDelegatingHandler : DelegatingHandler
    {
        //private static string APIKey => ConfigurationManager.AppSettings["APIKey"];

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var APIKey = "kJCrpsMovfqAWrlY1E5sV1+ZpP5/vndJ9gE/QcSbYac=";
                string nonce = Guid.NewGuid().ToString("N");
                string signatureRawData = APIKey + ":" + nonce;
                using (HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(APIKey)))
                {
                    string requestSignatureBase64String = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureRawData)));
                    request.Headers.Authorization = new AuthenticationHeaderValue("amx", string.Format("{0}:{1}", requestSignatureBase64String, nonce));
                }

                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

                return response;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
