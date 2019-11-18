using KC_Bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using static KC_Bugtracker.EmailService;

namespace KC_Bugtracker.Helpers
{
    public class EmailHelper
    {
        private static string ConfiguredEmail = WebConfigurationManager.AppSettings["emailto"];

        public static async Task ComposeEmailAsync(EmailModel email)
        {
            try
            {
                var senderEmail = $"{email.FromEmail}<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, ConfiguredEmail)
                {
                    Subject = email.Subject,
                    Body = $"<strong>{email.FromName} has sent you the following message: </strong> <hr/> {email.Body}",
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }

        public static async Task ComposeEmailAsync(RegisterViewModel model, string callbackUrl)
        {
            try
            {
                var senderEmail = $"Bugtracker Admin<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, model.Email)
                {
                    Subject = "Confirm your accout",
                    Body = $"Please confirm your account by clicking <a href=\"{callbackUrl}\">here</a>",
                    IsBodyHtml = true
                };

                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }

        public static async Task ComposeEmailAsync(ForgotPasswordViewModel model, string callbackUrl)
        {
            try
            {
                var senderEmail = $"Bugtracker Admin<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, model.Email)
                {
                    Subject = "Reset Password",
                    Body = $"Please reset your password by clicking this <a href=\"{callbackUrl}\">link</a>",
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }

    }
}