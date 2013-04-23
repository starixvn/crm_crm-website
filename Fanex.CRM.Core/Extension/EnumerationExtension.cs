
namespace Fanex.CRM.Infrastructure.Extension
{
    using System;

    public static class EnumerationExtension
    {
        /// <summary>
        /// <![CDATA[http://stackoverflow.com/questions/276585/enumeration-extension-methods]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theEnum"></param>
        /// <param name="strType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse<T>(this Enum theEnum, string strType,  out T result)
        {
            string strTypeFixed = strType.Replace(' ', '_');
            if (Enum.IsDefined(typeof(T), strTypeFixed))
            {
                result = (T)Enum.Parse(typeof(T), strTypeFixed, true);
                return true;
            }
            else
            {
                foreach (string value in Enum.GetNames(typeof(T)))
                {
                    if (value.Equals(strTypeFixed,
                                    StringComparison.OrdinalIgnoreCase))
                    {
                        result = (T)Enum.Parse(typeof(T), value);
                        return true;
                    }
                }
                result = default(T);
                return false;
            }
        }

    }
}
