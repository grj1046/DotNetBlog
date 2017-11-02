using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using DotNetBlog.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using DotNetBlog.Common;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

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

                var objSalt = this.DbContext.Accounts
                    .Select(a => new { AccountID = a.AccountID, Email = a.Email, Salt = a.Salt })
                    .FirstOrDefault(a => a.Email == Input.Email);
                if (objSalt == null)
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                RSA rsa = RSAExtensions.CreateRsaFromPrivateKey(RSAConstants.PrivateKey);
                var cipherBytes = System.Convert.FromBase64String(Input.Password);
                var plainTextBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
                var hashedTextBytes = Encoding.UTF8.GetBytes(objSalt.Salt).Concat(plainTextBytes).ToArray();
                MD5 md5 = MD5.Create();
                var byteMd5Pwd = md5.ComputeHash(hashedTextBytes);
                var strMd5Pwd = BitConverter.ToString(byteMd5Pwd).Replace("-", "");

                var account = this.DbContext.Accounts.Include(a => a.User)
                    .FirstOrDefault(a => a.AccountID == objSalt.AccountID && a.PasswordHash == strMd5Pwd);
                if (account != null)
                {
                    //_logger.LogInformation("User Logged in.");
                    var id = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    id.AddClaim(new Claim(ClaimTypes.NameIdentifier, account.User.UserID.ToString()));
                    id.AddClaim(new Claim(ClaimTypes.Name, account.User.NickName));
                    id.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, "Email"));
                    //get roles of this type
                    var roles = new[] { "Manager" };//await this.DbContext.GetRolesAsync(user);
                    foreach (var roleName in roles)
                    {
                        id.AddClaim(new Claim(ClaimTypes.Role, roleName));
                    }
                    var userPrincipal = new ClaimsPrincipal(id);
                    await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal, new AuthenticationProperties());

                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                await Task.FromResult(0);

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