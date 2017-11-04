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

        //public string PublicKey { get; private set; }

        //openssl genrsa -out rsa_1024_priv.pem 1024
//        private static readonly string _privateKey = @"MIICXQIBAAKBgQDlOJu6TyygqxfWT7eLtGDwajtNFOb9I5XRb6khyfD1Yt3YiCgQ
//WMNW649887VGJiGr/L5i2osbl8C9+WJTeucF+S76xFxdU6jE0NQ+Z+zEdhUTooNR
//aY5nZiu5PgDB0ED/ZKBUSLKL7eibMxZtMlUDHjm4gwQco1KRMDSmXSMkDwIDAQAB
//AoGAfY9LpnuWK5Bs50UVep5c93SJdUi82u7yMx4iHFMc/Z2hfenfYEzu+57fI4fv
//xTQ//5DbzRR/XKb8ulNv6+CHyPF31xk7YOBfkGI8qjLoq06V+FyBfDSwL8KbLyeH
//m7KUZnLNQbk8yGLzB3iYKkRHlmUanQGaNMIJziWOkN+N9dECQQD0ONYRNZeuM8zd
//8XJTSdcIX4a3gy3GGCJxOzv16XHxD03GW6UNLmfPwenKu+cdrQeaqEixrCejXdAF
//z/7+BSMpAkEA8EaSOeP5Xr3ZrbiKzi6TGMwHMvC7HdJxaBJbVRfApFrE0/mPwmP5
//rN7QwjrMY+0+AbXcm8mRQyQ1+IGEembsdwJBAN6az8Rv7QnD/YBvi52POIlRSSIM
//V7SwWvSK4WSMnGb1ZBbhgdg57DXaspcwHsFV7hByQ5BvMtIduHcT14ECfcECQATe
//aTgjFnqE/lQ22Rk0eGaYO80cc643BXVGafNfd9fcvwBMnk0iGX0XRsOozVt5Azil
//psLBYuApa66NcVHJpCECQQDTjI2AQhFc1yRnCU/YgDnSpJVm1nASoRUnU8Jfm3Oz
//uku7JUXcVpt08DFSceCEX9unCuMcT72rAQlLpdZir876".Replace("\n", "");

        //openssl rsa -pubout -in rsa_1024_priv.pem -out rsa_1024_pub.pem
        //        private static readonly string _publicKey = @"-----BEGIN PUBLIC KEY-----
        //MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDlOJu6TyygqxfWT7eLtGDwajtN
        //FOb9I5XRb6khyfD1Yt3YiCgQWMNW649887VGJiGr/L5i2osbl8C9+WJTeucF+S76
        //xFxdU6jE0NQ+Z+zEdhUTooNRaY5nZiu5PgDB0ED/ZKBUSLKL7eibMxZtMlUDHjm4
        //gwQco1KRMDSmXSMkDwIDAQAB
        //-----END PUBLIC KEY-----";

        public RegisterModel(DotNetBlogDbContext dotNetBlog)
        {
            this.DbContext = dotNetBlog;
            //this.PublicKey = _publicKey;
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
                Models.Account account = new Models.Account();
                account.Email = Input.Email;
                account.Salt = strSalt;
                account.PasswordHash = strMd5Pwd;
                Models.User user = new Models.User();
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