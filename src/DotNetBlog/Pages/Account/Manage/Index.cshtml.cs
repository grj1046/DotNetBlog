using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using DotNetBlog.Services;
using System.ComponentModel.DataAnnotations;
using DotNetBlog.Identity;
using System.Security.Claims;
using Dapper;

namespace DotNetBlog.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly IDbConnectionFactory db;
        public string NickName { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public IEmailSender EmailSender { get; set; }

        public IndexModel(IEmailSender emailSender, IDbConnectionFactory dotNetBlogDb)
        {
            this.EmailSender = emailSender;
            this.db = dotNetBlogDb;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userID = this.User.GetUserID();
            string strSql = @"
select UserName, Email, PhoneNumber from accounts where UserID = @UserID;
select NickName, Birthday, Gender from users where ID = @UserID;";
            var multiResult = await this.db.AccountDb.QueryMultipleAsync(strSql, new { UserID = userID });
            var account = await multiResult.ReadFirstOrDefaultAsync<Models.Account>();
            var user = await multiResult.ReadFirstOrDefaultAsync<Models.User>();
            if (account == null || user == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            this.NickName = user.NickName;
            this.Input = new InputModel()
            {
                Email = account.Email,
                PhoneNumber = account.PhoneNumber
            };

            //TODO: Email
            IsEmailConfirmed = false;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            var userID = this.User.GetUserID();

            string strSql = "select AccountID, UserName, Email, PhoneNumber from accounts where UserID = @UserID;";
            var account = await this.db.AccountDb.QueryFirstOrDefaultAsync<Models.Account>(strSql, new { UserID = userID });
            if (account == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            strSql = "update accounts set Email = @Email, PhoneNumber = @PhoneNumber where AccountID = @AccountID;";
            try
            {
                int result = await this.db.AccountDb.ExecuteAsync(strSql, new { this.Input.Email, this.Input.PhoneNumber, account.AccountID });
                if (result == 0)
                    throw new Exception("update failed");
            }
            catch (Exception)
            {
                throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{userID}'.");
            }

            this.StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userID = this.User.GetUserID();

            string strSql = "select UserName, Email, PhoneNumber from accounts where UserID = @UserID;";
            var account = await this.db.AccountDb.QueryFirstOrDefaultAsync<Models.Account>(strSql, new { UserID = userID });
            if (account == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");


            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            //await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
        }
    }
}