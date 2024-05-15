using NotificationManagementService.Core.Enum;

namespace NotificationManagementService.Core.Model
{
    public class Message
    {
        public ActionType ActionType { get; set; }

        public string Value { get; set; }
    }
}
