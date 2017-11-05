using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using DotNetBlog.Models;
using System.Security.Cryptography;
using System.Text;
using DotNetBlog.Common;
using DotNetBlog.Identity;

namespace DotNetBlog.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public GuorjAccountDbContext DbContext { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public RegisterModel(GuorjAccountDbContext dotNetBlog)
        {
            this.DbContext = dotNetBlog;
        }

        public void OnGet(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            if (this.ModelState.IsValid)
            {
                if (this.DbContext.Accounts.Any(a => a.Email == Input.Email))
                {
                    this.ModelState.AddModelError(string.Empty, "the email address is already registered");
                    return Page();
                }
                string strSalt = UserManager.GetSalt();
                var strMd5Pwd = UserManager.GetPasswordHash(strSalt, Input.Password);

                //create user when pass authentication
                //send confirm email
                var nowDate = DateTime.Now;
                Models.Account account = new Models.Account();
                account.AccountID = Guid.NewGuid();
                account.Email = Input.Email;
                account.Salt = strSalt;
                account.PasswordHash = strMd5Pwd;
                account.CreateAt = nowDate;
                account.UpdateAt = nowDate;
                Models.User user = new Models.User();
                user.UserID = Guid.NewGuid();
                user.CreateAt = nowDate;
                user.UpdateAt = nowDate;
                user.Account = account;
                user.NickName = Input.Email.Substring(0, Input.Email.IndexOf('@'));
                this.DbContext.Users.Add(user);
                await this.DbContext.SaveChangesAsync();
                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }
            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            //[StringLength(30, ErrorMessage = "The {0} must be at lease {2} and at max {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [Display(Name = "Confirm password")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}