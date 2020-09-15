namespace Fanex.CRM.BackgroundJob.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly HangfireAuthentication auth;

        public AuthController(IOptionsSnapshot<HangfireAuthentication> auth)
        {
            this.auth = auth.Value;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var isValid = IsValidUser(username, password);

            if (isValid)
            {
                await DoCheckIn(username);
                return Redirect("~/hangfire");
            }

            ViewBag.IsValid = isValid;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        public IActionResult Error()
        {
            return View();
        }

        private async Task DoCheckIn(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private bool IsValidUser(string username, string password)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var encryptedPassword = Cryptography.EncryptPassword(password);
                isValid = username.Equals(auth.Username, StringComparison.OrdinalIgnoreCase)
                    && encryptedPassword.Equals(auth.Password);
            }

            return isValid;
        }
    }
}