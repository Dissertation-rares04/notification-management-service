using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificationManagementService.Business.Interface;
using NotificationManagementService.Core;
using NotificationManagementService.Core.Enum;
using NotificationManagementService.Core.Model;

namespace NotificationManagementService.Business.Implementation
{
    public class NotificationTasksMessageHandler : IMessageHandler
    {
        private readonly ILogger<NotificationTasksMessageHandler> _logger;
        private readonly IMailService _mailService;
        private readonly IAuth0HttpClient _auth0HttpClient;

        public NotificationTasksMessageHandler(ILogger<NotificationTasksMessageHandler> logger, IMailService mailService, IAuth0HttpClient auth0HttpClient)
        {
            _logger = logger;
            _mailService = mailService;
            _auth0HttpClient = auth0HttpClient;
        }

        public async Task HandleMessage(Message message)
        {
            try
            {
                switch (message.ActionType)
                {
                    case ActionType.POST_CREATED:
                        throw new NotImplementedException();
                    case ActionType.POST_UPDATED:
                        throw new NotImplementedException();
                    case ActionType.POST_DELETED:
                        throw new NotImplementedException();
                    case ActionType.COMMENT_CREATED:
                        var commentReceived = JsonConvert.DeserializeObject<CommentReceived>(message.Value);
                        await HandleCommentCreatedActionType(commentReceived);
                        break;
                    case ActionType.COMMENT_UPDATED:
                        throw new NotImplementedException();
                    case ActionType.COMMENT_DELETED:
                        throw new NotImplementedException();
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task HandleCommentCreatedActionType(CommentReceived commentReceived) 
        {
            var commentAuthorRoute = string.Format(Routes.UserById, commentReceived.CommentAuthorUserId);
            var commentAuthor = await _auth0HttpClient.GetAsync<User>(commentAuthorRoute);

            var postAuthorRoute = string.Format(Routes.UserById, commentReceived.PostAuthorUserId);
            var postAuthor = await _auth0HttpClient.GetAsync<User>(postAuthorRoute);

            await _mailService.SendCommentCreatedMail(commentAuthor, postAuthor, commentReceived);
        }
    }
}
