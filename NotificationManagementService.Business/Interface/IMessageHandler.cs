using NotificationManagementService.Core.Model;

namespace NotificationManagementService.Business.Interface
{
    public interface IMessageHandler
    {
        Task HandleMessage(Message message);
    }
}
