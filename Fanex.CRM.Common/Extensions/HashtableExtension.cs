namespace Fanex.CRM.Common.Extensions
{
    using System.Collections;
    using System.Diagnostics;

    public static class HashtableExtension
    {
        [DebuggerStepThrough]
        public static void AddIf(this Hashtable target, bool condition, string key, object value)
        {
            if (condition)
            {
                target.Add(key, value);
            }
        }
    }
}