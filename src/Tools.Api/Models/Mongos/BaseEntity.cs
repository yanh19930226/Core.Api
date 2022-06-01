using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Api.Models.Mongos
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement(nameof(CreateDateTime))]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }
        [BsonElement(nameof(UpdateTime))]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdateTime { get; set; }
        public bool IsDelete { get; set; }
        public BaseEntity()
        {
            CreateDateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            IsDelete = false;
        }
    }
}
