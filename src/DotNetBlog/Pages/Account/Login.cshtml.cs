using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using DotNetBlog.Models;

namespace DotNetBlog.Pages.Account
{
    public class LoginModel : PageModel
    {
        public DotNetBlogDbContext DbContext { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public LoginModel(DotNetBlogDbContext dotNetBlog)
        {
            this.DbContext = dotNetBlog;
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);

            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await Task.FromResult(0);
            //this.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemeAsync().ToList());
            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                //This doesn't count login failures towards account lockout
                //To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                //"guorenjun@outlook.com".Equals(Input.Email, StringComparison.OrdinalIgnoreCase) && Input.Password == "123456"
                if (this.DbContext.Accounts.Any(a => a.Email == Input.Email && a.Password == Input.Password))
                {
                    //_logger.LogInformation("User Logged in.");
                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                await Task.FromResult(0);
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe })
                //}
                //if (result.IsLockedOut)
                //{
                //    _logger.LogWarning("User account locket out.");
                //    return RedirectToPage("./Lockout");
                //}
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
            //if we got this far, something failed, redisplay form
            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}