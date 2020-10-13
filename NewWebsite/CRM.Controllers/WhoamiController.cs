namespace CRM.Controllers
{
    using System;
    using System.Threading.Tasks;
    using CRM.Core.Extensions;
    using CRM.Letstalk.Client.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class WhoamiController : Controller
    {
        private readonly Func<int, ILetstalkService> letstalkService;

        public WhoamiController(Func<int, ILetstalkService> letstalkService)
        {
            this.letstalkService = letstalkService;
        }

        public async Task<IActionResult> Index()
        {
            await letstalkService(100021).PushAsync("t381023", "Hi Oliver, I am Production bot");
            return Content(WhoamiHelper.GetContent(HttpContext));
        }
    }
}