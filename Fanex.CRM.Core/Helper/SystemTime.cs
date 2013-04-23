
namespace Fanex.CRM.Infrastructure.Helper
{
    using System;

    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.UtcNow;
    }
}