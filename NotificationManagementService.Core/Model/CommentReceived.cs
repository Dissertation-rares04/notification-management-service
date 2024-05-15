using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationManagementService.Core.Model
{
    public class CommentReceived
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostAuthorUserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CommentAuthorUserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }

        public string CommentContent { get; set; }
    }
}
