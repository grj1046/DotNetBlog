using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Dapper;

namespace DotNetBlog.Pages.Account.Manage
{
    public class BasicModel : PageModel
    {
        private readonly IDbConnectionFactory db;

        [BindProperty]
        public InputModel Input { get; set; }

        public BasicModel(IDbConnectionFactory dotNetBlogDb)
        {
            this.db = dotNetBlogDb;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userID = this.User.GetUserID();
            string strSql = "select NickName, Birthday, Gender from users where ID = @UserID limit 1;";
            var user = await this.db.AccountDb.QueryFirstOrDefaultAsync<User>(strSql, new { UserID = userID });
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            this.Input = new InputModel()
            {
                NickName = user.NickName,
                Birthday = user.Birthday?.ToString("yyyy/MM/dd"),
                Gender = user.Gender
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userID = this.User.GetUserID();
            string strSql = "select NickName, Birthday, Gender from users where ID = @UserID limit 1;";
            var user = await this.db.AccountDb.QueryFirstOrDefaultAsync<User>(strSql, new { UserID = userID });
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");
            user.NickName = this.Input.NickName;
            if (string.IsNullOrEmpty(this.Input.Birthday))
                user.Birthday = null;
            else
                user.Birthday = Convert.ToDateTime(this.Input.Birthday);
            user.Gender = this.Input.Gender;
            user.UpdateAt = DateTime.Now;
            strSql = "update users set NickName = @NickName, Birthday = @Birthday, Gender = @Gender, UpdateAt = @UpdateAt where ID = @UserID;";
            await this.db.AccountDb.ExecuteAsync(strSql, new { userID = userID, user.NickName, user.Birthday, user.UpdateAt, user.Gender });

            return RedirectToPage();
        }

        public class InputModel
        {
            public string NickName { get; set; }
            [DataType(DataType.Text)]
            [DisplayFormat(DataFormatString = "yyyy/MM/dd")]
            public string Birthday { get; set; }
            public Gender? Gender { get; set; }
        }
    }
}