using EduQuest_Application.Abstractions.Email;
using Infrastructure.ExternalServices.Email.Setting;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EduQuest_Infrastructure.ExternalServices.Email
{

    public class EmailServices : IEmailService
    {
        private readonly EmailSetting _emailSetting;

        public EmailServices(IOptions<EmailSetting> emailSetting)
        {
            _emailSetting = emailSetting.Value;
        }

        public async Task SendEmailVerifyAsync(string subject, string recipientEmail, string username, string otp, string path, string logoPath)
        {
            // Create a new email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSetting.SenderName, _emailSetting.SenderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = subject;

            // Load HTML content from the file
            string htmlContent = @"
                                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8' />
                    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                    <title>Email Verification</title>
                    <style>
                        body { font-family: Arial, sans-serif; margin: 0; padding: 0; }
                        .email-container { max-width: 400px; margin: 0 auto; background-color: white; border-radius: 20px; overflow: hidden; }
                        .email-header, .email-footer {
                            background: linear-gradient(to bottom, #c67130, #d36a2d, #FF6F3C);
                            height: 70px;
                            text-align: center;
                            position: relative;
                        }
                        /* .email-header { height: 80px; } */
                        .email-body { padding: 20px; text-align: center; }
                        h1 { font-size: 25px; }
                        p { color: #666; font-size: 14px; line-height: 1.6; }
                        .OTP { font-size: 22px; font-weight: bold; color: #FF6F3C; }
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='email-body'>
                            <h1>Verify Your Action</h1>
                            <p>To complete your request securely, please use the verification code below:</p>
                            <h1 class='OTP'>@Model.OTP</h1>
                            <p>This is an automated email. Please do not reply.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            // Replace placeholders with actual values
            var hrml = htmlContent.Replace("@Model.UserName", username)
                                          .Replace("@Model.OTP", otp);

            // Set the email body with HTML content
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = hrml
            };

            //if (File.Exists(logoPath))
            //{
            //    var logo = bodyBuilder.LinkedResources.Add(logoPath);
            //    logo.ContentId = "logo3";
            //}
            message.Body = bodyBuilder.ToMessageBody();

            // Send the email using an SMTP client
            using (var smtpClient = new SmtpClient())
            {
                // SMTP server configuration (replace with your SMTP server details)
                var smtpServer = _emailSetting.SmtpServer;
                var smtpPort = _emailSetting.Port; // or 465 for SSL
                var smtpUsername = _emailSetting.Username;
                var smtpPassword = _emailSetting.Password;

                // Connect to the SMTP server
                await smtpClient.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(smtpUsername, smtpPassword);

                // Send the email
                await smtpClient.SendAsync(message);

                // Disconnect from the SMTP server
                await smtpClient.DisconnectAsync(true);
            }
        }


       
    }
}
