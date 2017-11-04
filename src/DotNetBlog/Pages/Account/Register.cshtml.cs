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

namespace DotNetBlog.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public DotNetBlogDbContext DbContext { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public RegisterModel(DotNetBlogDbContext dotNetBlog)
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
                RSA rsa = RSAExtensions.CreateRsaFromPrivateKey(RSAConstants.PrivateKey);
                var cipherBytes = System.Convert.FromBase64String(Input.Password);
                var plainTextBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
                //var planText = Encoding.UTF8.GetString(plainTextBytes);

                string strRandomStr = "0123456789AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
                StringBuilder sb = new StringBuilder();
                Random rnd = new Random();
                for (int i = 0; i < 5; i++)
                {
                    sb.Append(strRandomStr[rnd.Next(strRandomStr.Length)]);
                }
                string strSalt = sb.ToString();
                var hashedTextBytes = Encoding.UTF8.GetBytes(strSalt).Concat(plainTextBytes).ToArray();

                MD5 md5 = MD5.Create();
                var byteMd5Pwd = md5.ComputeHash(hashedTextBytes);
                var strMd5Pwd = BitConverter.ToString(byteMd5Pwd).Replace("-", "");

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