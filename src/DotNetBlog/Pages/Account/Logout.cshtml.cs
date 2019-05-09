using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DotNetBlog.Pages.Account
{
    public class LogoutModel : PageModel
    {
        [BindProperty]
        public string method { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await this.HttpContext.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}