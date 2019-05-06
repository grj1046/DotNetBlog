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
using Dapper;

namespace DotNetBlog.Pages.Account
{
    public class RegisterModel : PageModel
    {

        private readonly IDbConnectionFactory db;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public RegisterModel(IDbConnectionFactory dotNetBlogDb)
        {
            this.db = dotNetBlogDb;
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
                this.db.AccountDb.Open();
                var trans = this.db.AccountDb.BeginTransaction();
                try
                {
                    string strSql = "SELECT * FROM accounts WHERE Email = @Email;";
                    var account = await this.db.AccountDb.QueryFirstOrDefaultAsync<Models.Account>(strSql, new { Email = Input.Email }, trans);
                    if (account != null)
                    {
                        this.ModelState.AddModelError(string.Empty, "the email address is already registered");
                        return Page();
                    }
                    string strSalt = UserManager.GetSalt();
                    var strMd5Pwd = UserManager.GetPasswordHash(strSalt, Input.Password);

                    //create user when pass authentication
                    //send confirm email
                    var nowDate = DateTime.Now;
                    var UserID = Guid.NewGuid();
                    string strNickName = Input.Email.Substring(0, Input.Email.IndexOf('@'));
                    strSql = @"
INSERT Users(ID, NickName, CreateAt, UpdateAt) VALUE(@ID, @NickName, @CreateAt, @UpdateAt);
INSERT accounts(AccountID, Email, Salt, PasswordHash, UserID, CreateAt, UpdateAt) VALUE (@AccountID, @Email, @Salt, @PasswordHash, @UserID, @CreateAt, @UpdateAt);";
                    await this.db.AccountDb.ExecuteAsync(strSql, new
                    {
                        AccountID = Guid.NewGuid(),
                        Input.Email,
                        Salt = strSalt,
                        PasswordHash = strMd5Pwd,
                        UserID,
                        ID = UserID,
                        NickName = strNickName,
                        CreateAt = nowDate,
                        UpdateAt = nowDate
                    }, trans);
                    trans.Commit();
                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
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