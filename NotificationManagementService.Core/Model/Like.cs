using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationManagementService.Core.Model
{
    public class Like
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } // User ID of the user who liked the post/comment

        public DateTime CreatedAt { get; set; } // Timestamp of the like
    }
}
