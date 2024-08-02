using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeAPI.Models;

public class CodeContent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    [BsonSerializer(typeof(JsonElementSerializer))]
    public JsonElement Picture { get; set; }
    [BsonSerializer(typeof(JsonElementSerializer))]
    public JsonElement SubCodeContent { get; set; }
}