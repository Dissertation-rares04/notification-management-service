using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationManagementService.Core.Model
{
    public class Comment : AuditableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }

        public List<Like> Likes { get; set; }
    }
}
