namespace CRM.Controllers
{
    using System.Threading.Tasks;
    using CRM.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class TestController : Controller
    {
        private readonly ISendNotificationService sendNotificationService;

        public TestController(ISendNotificationService sendNotificationService)
        {
            this.sendNotificationService = sendNotificationService;
        }

        [Route("/api/test")]
        public async Task<IActionResult> TestJob()
        {
            await sendNotificationService.SendNotificationToLetstalk();

            return Ok();
        }
    }
}