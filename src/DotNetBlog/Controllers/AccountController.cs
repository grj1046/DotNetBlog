using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetBlog.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;

namespace DotNetBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger Logger;

        public AccountController( ILogger<AccountController> logger)
        {
            this.Logger = logger;
        }

        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            this.Logger.LogInformation("User Logged out.");
            return RedirectToPage("/Index");
        }
    }
}