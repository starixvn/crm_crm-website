
namespace Fanex.CRM.Infrastructure.Helper
{
    using System;
    using System.Web;

    public class QueryString
    {
        /// <summary>
        /// QueryString
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static T Value<T>(string parameterName) where T : IConvertible
        {
            
            if (HttpContext.Current == null)
                throw new InvalidOperationException("Cannot call QueryString<T> when HttpContext.Current is null");

            if (string.IsNullOrEmpty(HttpContext.Current.Request[parameterName]))
                throw new InvalidOperationException(string.Format("Expected query string parameter named '{0}' to exist", parameterName));

            return Value<T>(parameterName, default(T));
        }


        public static T Value<T>(string parameterName, T defaultValue) where T : IConvertible
        {
            if (HttpContext.Current == null)
                throw new InvalidOperationException("Cannot call QueryString<T> when HttpContext.Current is null");

            HttpRequest request = HttpContext.Current.Request;

            string input;
            if (request.QueryString[parameterName] != null
                && !string.IsNullOrEmpty(request.QueryString[parameterName]))
                input = request.QueryString[parameterName];
            else
                return defaultValue;

            T value;
            try
            {
                value = (T)Convert.ChangeType(input, typeof(T));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Unable to convert query string parameter named '{0}' with value of '{1}' to type {2}", parameterName, input, typeof(T).FullName), ex);
            }
            return value;
        }
    }
}