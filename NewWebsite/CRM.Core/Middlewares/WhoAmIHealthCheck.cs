namespace CRM.Core.Middlewares
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public class WhoAmIHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
           HealthCheckContext context,
           CancellationToken cancellationToken = default(CancellationToken))
        {
            var ip = await Dns.GetHostAddressesAsync(Dns.GetHostName());
            IDictionary<string, object> result = new Dictionary<string, object>();
            for (var i = 1; i < ip.Length; i++)
            {
                if (!ip[i].Equals("::1"))
                {
                    result.Add($"IP{i}", ip[i].ToString());
                }
            }
            return HealthCheckResult.Healthy("Server Info", new ReadOnlyDictionary<string, object>(result));
        }
    }
}