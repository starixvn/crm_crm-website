namespace CRM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CRM.Core.Enumerables;
    using CRM.Core.Models;
    using CRM.Models.Auth;
    using CRM.Services;
    using Fanex.Data;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Models;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class AuthController : Controller
    {
        private readonly IUserService userService;
        private readonly INapSession napSession;
        private readonly ISecurityService securityService;
        private readonly IAuthService authService;
        private readonly AppSettings appSettings;
        private readonly ExternalSiteHostUrl externalSiteHostUrl;

        public AuthController(
            IUserService userService,
            INapSession napSession,
            ISecurityService securityService,
            IAuthService authService,
            IOptionsSnapshot<AppSettings> appSettings,
            IOptionsSnapshot<ExternalSiteHostUrl> externalSiteHostUrl)
        {
            this.userService = userService;
            this.napSession = napSession;
            this.securityService = securityService;
            this.authService = authService;
            this.appSettings = appSettings.Value;
            this.externalSiteHostUrl = externalSiteHostUrl.Value;
        }

        public async Task<IActionResult> SignIn(SignIn model)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                return View();
            }

            User user = await userService.GetAsync(model.Username);
            if (user != null && IsValidPassword(user, model))
            {
                int.TryParse(Cryptography.DecryptPassword(model.OnUserId.Replace(" ", "+")), out int onUserId);
                await DoSignInAsync(user, onUserId);
            }

            return new RedirectResult(externalSiteHostUrl.DefaultPageUrl);
        }

        public async Task<IActionResult> SingleSignOn(SingleSignOn model)
        {
            if (IsValidTime(model.Time))
            {
                int.TryParse(Cryptography.DecryptPassword(model.UserId.Replace(" ", "+")), out int userId);
                User user = await userService.GetAsync(userId);
                if (user != null)
                {
                    int.TryParse(Cryptography.DecryptPassword(model.OnUserId.Replace(" ", "+")), out int onUserId);
                    await DoSignInAsync(user, onUserId);
                }
            }

            return new RedirectResult(externalSiteHostUrl.DefaultPageUrl);
        }

        public async Task<IActionResult> SignOut()
        {
            await DoSignOutAsync();

            return new RedirectResult(externalSiteHostUrl.SignInUrl);
        }

        public async Task<IActionResult> ReCheckIn()
        {
            RecheckInStatus recheckInStatus = await authService.GetRecheckInStatusAsync();
            if (recheckInStatus != RecheckInStatus.Success)
            {
                await DoSignOutAsync();
            }
            var commandResult = new CommandResult
            {
                Code = (byte)recheckInStatus,
                Message = string.Empty
            };
            return Json(commandResult);
        }

        private bool IsValidPassword(User user, SignIn signIn)
        {
            var hashPassword = authService.HashText(string.Format("{0}{1}", user.Password, signIn.Token));
            return string.Equals(signIn.Password, hashPassword, StringComparison.CurrentCulture);
        }

        private async Task DoSignInAsync(User user, int onUserId)
        {
            napSession.UserId = user.Id;
            napSession.UserName = user.Username;
            napSession.OnUserId = onUserId;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }

        private bool IsValidTime(string time)
        {
            DateTime.TryParse(Cryptography.DecryptPassword(time.Replace(" ", "+")), out DateTime descryptTime1);
            DateTime.TryParse(Cryptography.DecryptPassword(securityService.GetTime()), out DateTime descryptTime2);
            var expiration = descryptTime2 - descryptTime1;

            return expiration.TotalSeconds <= appSettings.SingleSignOnExpiredTime;
        }

        private async Task DoSignOutAsync()
        {
            ClearCookies();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private void ClearCookies()
        {
            HttpContext.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Response.Headers.Add("Pragma", "no-cache");
            HttpContext.Response.Headers.Add("Expires", "0");

            IEnumerable<string> cookies = HttpContext.Request.Cookies.Keys;
            foreach (string cookie in cookies)
            {
                HttpContext.Response.Cookies.Delete(cookie);
            }
        }
    }
}