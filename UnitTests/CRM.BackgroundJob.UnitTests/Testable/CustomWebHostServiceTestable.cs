namespace CRM.BackgroundJob.UnitTests.Testable
{
    using Fanex.CRM.BackgroundJob.Services;
    using Microsoft.AspNetCore.Hosting;

    public class CustomWebHostServiceTestable : CustomWebHostService
    {
        public CustomWebHostServiceTestable(IWebHost host) : base(host)
        {
        }

        public void Public_OnStarted()
        {
            base.OnStarted();
        }

        public void Public_OnStopping()
        {
            base.OnStopping();
        }

        public void Public_RunHangfireJobs()
        {
            base.RunHangfireJobs();
        }

        public bool Public_IsEnabledJob(string jobName)
        {
            return base.IsEnabledJob(jobName);
        }

        public string Public_GetCronExpression(string jobName)
        {
            return base.GetCronExpression(jobName);
        }
    }
}