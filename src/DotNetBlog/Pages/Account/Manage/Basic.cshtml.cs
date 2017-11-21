using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNetBlog.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DotNetBlog.Pages.Account.Manage
{
    public class BasicModel : PageModel
    {

        public GuorjAccountDbContext DbAccount { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public BasicModel(GuorjAccountDbContext dbAccount)
        {
            this.DbAccount = dbAccount;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userID = this.User.GetUserID();
            var user = await this.DbAccount.Users.FirstOrDefaultAsync(a => a.UserID == userID);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            this.Input = new InputModel()
            {
                NickName = user.NickName,
                Birthday = user.Birthday,
                Gender = user.Gender
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userID = this.User.GetUserID();
            var user = await this.DbAccount.Users.FirstOrDefaultAsync(a => a.UserID == userID);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userID}'.");
            user.NickName = this.Input.NickName;
            user.Birthday = this.Input.Birthday;
            user.Gender = this.Input.Gender;
            user.UpdateAt = DateTime.Now;
            await this.DbAccount.SaveChangesAsync();

            return RedirectToPage();
        }

        public class InputModel
        {
            //public Guid UserID { get; set; }
            public string NickName { get; set; }
            [DataType(DataType.Date)]
            public DateTime? Birthday { get; set; }
            public Gender? Gender { get; set; }
        }
    }
}