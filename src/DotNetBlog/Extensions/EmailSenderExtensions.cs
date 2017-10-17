using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DotNetBlog.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            string strSubject = "Confirm your email";
            string strMessage = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>.";
            return emailSender.SendEmailAsync(email, strSubject, strMessage);
        }
        public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string link)
        {
            string strSubject = "Reset Password";
            string strMessage = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>.";
            return emailSender.SendEmailAsync(email, strSubject, strMessage);
        }
    }
}
