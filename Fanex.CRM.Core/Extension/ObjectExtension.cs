
namespace Fanex.CRM.Infrastructure.Extension
{
    using System.Diagnostics;

    public static class ObjectExtension
    {
        [DebuggerStepThrough()]
        public static bool Is<T>(this object item) where T : class
        {
            return item is T;
        }

        [DebuggerStepThrough()]
        public static bool IsNot<T>(this object item) where T : class
        {
            return !(item.Is<T>());
        }

        [DebuggerStepThrough()]
        public static T As<T>(this object item) where T : class
        {
            return item as T;
        }

    }
}
