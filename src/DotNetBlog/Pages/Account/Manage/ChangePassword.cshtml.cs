using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DotNetBlog.Identity;
using Dapper;

namespace DotNetBlog.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IDbConnectionFactory db;
        public ILogger<ChangePasswordModel> Logger { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string StatusMessage { get; set; }

        public ChangePasswordModel(IDbConnectionFactory dotNetBlogDb, ILogger<ChangePasswordModel> logger)
        {
            this.db = dotNetBlogDb;
            this.Logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userID = this.User.GetUserID();
            string strSql = @"
select AccountID, PasswordHash from accounts where UserID = @UserID limit 1;
select ID from users where ID = @UserID limit 1;";
            var multiResult = await this.db.AccountDb.QueryMultipleAsync(strSql, new { UserID = userID });
            var account = await multiResult.ReadFirstOrDefaultAsync<Models.Account>();
            var user = await multiResult.ReadFirstOrDefaultAsync<Models.User>();
            if (account == null || user == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");
            if (string.IsNullOrEmpty(account.PasswordHash))
                return RedirectToPage("./SetPassword");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userID = this.User.GetUserID();
            string strSql = @"
select AccountID, Salt, PasswordHash from accounts where UserID = @UserID limit 1;
select ID from users where ID = @UserID limit 1;";
            var multiResult = await this.db.AccountDb.QueryMultipleAsync(strSql, new { UserID = userID });
            var account = await multiResult.ReadFirstOrDefaultAsync<Models.Account>();
            var user = await multiResult.ReadFirstOrDefaultAsync<Models.User>();
            if (account == null || user == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            string strOldSalt = account.Salt;
            string strInputOldPasswordHashWithOldSalt = UserManager.GetPasswordHash(strOldSalt, Input.OldPassword);

            if (strInputOldPasswordHashWithOldSalt != account.PasswordHash)
            {
                this.ModelState.AddModelError(string.Empty, "The current password is incorrect.");
                return Page();
            }
            //check old password whether equals new password
            string strInputNewPasswordHashWithOldSalt = UserManager.GetPasswordHash(strOldSalt, Input.NewPassword);
            if (strInputOldPasswordHashWithOldSalt == strInputNewPasswordHashWithOldSalt)
            {
                this.ModelState.AddModelError(string.Empty, "The current password can't equals to new password.");
                return Page();
            }

            //TODO: maybe can move old passwordHash to backup db.
            string strNewSalt = UserManager.GetSalt();
            var strMd5Pwd = UserManager.GetPasswordHash(strNewSalt, Input.NewPassword);

            account.Salt = strNewSalt;
            account.PasswordHash = strMd5Pwd;
            account.UpdateAt = DateTime.Now;
            strSql = "update accounts set Salt = @Salt, PasswordHash = @PasswordHash, UpdateAt = @UpdateAt where AccountID = @AccountID;";
            await this.db.AccountDb.ExecuteAsync(strSql, new { account.AccountID, account.Salt, account.PasswordHash, account.UpdateAt });

            ////call SignIn function again

            //TODO: when updated password, maybe update login cookie yet?
            this.Logger.LogInformation("User changed their password successfully.");
            this.StatusMessage = "Your password has been changed.";
            return Page();
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string OldPassword { get; set; }
            [Required]
            //[StringLength(30, ErrorMessage = "The {0} must be at lease {2} and at max {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string NewPassword { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new Password")]
            [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}