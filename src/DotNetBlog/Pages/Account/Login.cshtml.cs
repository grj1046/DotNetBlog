using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Dapper;

namespace DotNetBlog.Pages.Account
{
    public class LoginModel : PageModel
    {

        private readonly IDbConnectionFactory db;

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public LoginModel(IDbConnectionFactory dotNetBlogDb)
        {
            this.db = dotNetBlogDb;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                //maybe it can be redirect to account profile page or account manager page
                return RedirectToPage("/Index");
            }
            if (!string.IsNullOrEmpty(this.ErrorMessage))
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);

            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await Task.FromResult(0);
            //this.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemeAsync().ToList());
            this.ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {

                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
            //This doesn't count login failures towards account lockout
            //To enable password failures to trigger account lockout, set lockoutOnFailure: true
            string strSql = "SELECT * FROM accounts WHERE Email = @Email;";
            var account = await this.db.AccountDb.QueryFirstOrDefaultAsync<Models.Account>(strSql, new { Input.Email });
            if (account == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            RSA rsa = RSAExtensions.CreateRsaFromPrivateKey(RSAConstants.PrivateKey);
            var cipherBytes = System.Convert.FromBase64String(Input.Password);
            var plainTextBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
            var hashedTextBytes = Encoding.UTF8.GetBytes(account.Salt).Concat(plainTextBytes).ToArray();
            MD5 md5 = MD5.Create();
            var byteMd5Pwd = md5.ComputeHash(hashedTextBytes);
            var strMd5Pwd = BitConverter.ToString(byteMd5Pwd).Replace("-", "");

            if (account.PasswordHash != strMd5Pwd)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
            strSql = @"
SELECT * FROM Users WHERE ID = @UserID;
SELECT UserID, UserRoles.RoleID, Name AS RoleName FROM UserRoles INNER JOIN Roles ON UserRoles.RoleID = Roles.RoleID WHERE UserID = @UserID;";
            var multiResult = await this.db.AccountDb.QueryMultipleAsync(strSql, new { account.UserID });
            var user = await multiResult.ReadFirstOrDefaultAsync<Models.User>();
            var listUserRoles = await multiResult.ReadAsync<Models.UserRole>();
            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, "the account of user not found, please contact administrator.");
                return Page();
            }

            //_logger.LogInformation("User Logged in.");
            var id = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypesConstants.Name, ClaimTypesConstants.Role);
            id.AddClaim(new Claim(ClaimTypesConstants.NameIdentifier, user.ID.ToString()));
            id.AddClaim(new Claim(ClaimTypesConstants.Name, user.NickName));
            id.AddClaim(new Claim(ClaimTypesConstants.AuthenticationMethod, "Email"));
            //get roles of this type
            foreach (var userRole in listUserRoles)
            {
                id.AddClaim(new Claim(ClaimTypesConstants.Role, userRole.RoleName));
            }
            var userPrincipal = new ClaimsPrincipal(id);

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal, new AuthenticationProperties() { IsPersistent = Input.RememberMe });

            return LocalRedirect(Url.GetLocalUrl(returnUrl));

            //if (result.IsLockedOut)
            //{
            //    _logger.LogWarning("User account locket out.");
            //    return RedirectToPage("./Lockout");
            //}
            //if we got this far, something failed, redisplay form
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