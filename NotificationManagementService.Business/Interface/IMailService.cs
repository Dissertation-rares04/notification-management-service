using NotificationManagementService.Core.Model;

namespace NotificationManagementService.Business.Interface
{
    public interface IMailService
    {
        Task<bool> SendCommentCreatedMail(User commentAuthor, User postAuthor, CommentReceived commentReceived);
    }
}
