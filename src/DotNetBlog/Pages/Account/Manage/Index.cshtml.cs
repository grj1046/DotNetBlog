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

namespace DotNetBlog.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        public string UserName { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public IEmailSender EmailSender { get; set; }

        public IndexModel( IEmailSender emailSender)
        {
            this.EmailSender = emailSender;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //var userID = this.User.GetUserID();
            //var user = await this.DbContext.Users.Include(a => a.Account).FirstOrDefaultAsync(a => a.UserID == userID);
            //if (user == null)
            //    throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            //this.UserName = user.Account.UserName;
            //this.Input = new InputModel()
            //{
            //    Email = user.Account.Email,
            //    PhoneNumber = user.Account.PhoneNumber
            //};

            //TODO: Email
            IsEmailConfirmed = false;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            var userID = this.User.GetUserID();

            //var user = await this.DbContext.Users.Include(a => a.Account).FirstOrDefaultAsync();
            //if (user == null)
            //    throw new ApplicationException($"Unable to load user with ID '{userID}'.");
            //if (this.Input.Email != user.Account.Email)
            //{
            //    user.Account.Email = this.Input.Email;
            //    try
            //    {
            //        await this.DbContext.SaveChangesAsync();
            //    }
            //    catch (Exception)
            //    {
            //        throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.UserID}'.");
            //    }
            //}
            //if (this.Input.PhoneNumber != user.Account.PhoneNumber)
            //{
            //    user.Account.PhoneNumber = this.Input.PhoneNumber;
            //    try
            //    {
            //        await this.DbContext.SaveChangesAsync();
            //    }
            //    catch (Exception)
            //    {
            //        throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.UserID}'.");
            //    }
            //}

            //this.StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            //var userID = this.User.GetUserID();

            //var user = await this.DbContext.Users.Include(a => a.Account).FirstOrDefaultAsync();
            //if (user == null)
            //    throw new ApplicationException($"Unable to load user with ID '{userID}'.");

            ////var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            ////var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            ////await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);

            //StatusMessage = "Verification email sent. Please check your email.";
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