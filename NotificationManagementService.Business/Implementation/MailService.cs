using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using NotificationManagementService.Business.Interface;
using NotificationManagementService.Core.AppSettings;
using NotificationManagementService.Core.Model;

namespace NotificationManagementService.Business.Implementation
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;

        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions, ILogger<MailService> logger)
        {
            _mailSettings = mailSettingsOptions.Value;
            _logger = logger;
        }

        public async Task<bool> SendCommentCreatedMail(User commentAuthor, User postAuthor, CommentReceived commentReceived)
        {
            try
            {
                using MimeMessage emailMessage = new();

                MailboxAddress emailFrom = new(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new(postAuthor.Name, postAuthor.Email);
                emailMessage.To.Add(emailTo);

                // you can add the CCs and BCCs here.
                //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                emailMessage.Subject = $"{commentAuthor.Name} left a new comment on your post {commentReceived.PostId}";

                BodyBuilder emailBodyBuilder = new()
                {
                    TextBody = JsonConvert.SerializeObject(commentReceived.CommentContent)
                };
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                using SmtpClient mailClient = new();
                await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                await mailClient.SendAsync(emailMessage);
                await mailClient.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
