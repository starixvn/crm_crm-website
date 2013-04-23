
namespace Fanex.CRM.Infrastructure.Extension
{
    using System;
    using System.Web;

    public class ResourceExtension
    {
        public enum Resources
        {
            ErrorMsg, Language
        }
        public static string GetResourceValue(Resources resource, string key)
        {
            try
            {
                return HttpContext.GetGlobalResourceObject(resource.ToString(), key).ToString();
            }
            catch (Exception)
            {
                
                throw new Exception("Key: " + key + " does not exist in resource: " + resource.ToString());
            }
            
        }
    }
}
