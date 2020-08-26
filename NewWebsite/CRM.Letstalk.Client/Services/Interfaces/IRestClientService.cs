namespace CRM.Letstalk.Client.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRestClientService
    {
        T ExecuteGet<T>(string endPoint, IDictionary<string, object> data = null) where T : new();

        T ExecutePost<T>(string endPoint, IDictionary<string, object> data) where T : new();

        T ExecutePut<T>(string endPoint, IDictionary<string, object> data) where T : new();

        T ExecuteDelete<T>(string endPoint, IDictionary<string, object> data) where T : new();

        Task<T> ExecuteGetAsync<T>(string endPoint, IDictionary<string, object> data = null) where T : new();

        Task<T> ExecutePostAsync<T>(string endPoint, IDictionary<string, object> data) where T : new();

        Task<T> ExecutePutAsync<T>(string endPoint, IDictionary<string, object> data) where T : new();

        Task<T> ExecuteDeleteAsync<T>(string endPoint, IDictionary<string, object> data) where T : new();
    }
}