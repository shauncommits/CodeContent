using MongoDB.Bson;

namespace CodeAPI.Models;
using MongoDB.Bson.Serialization;
using System.Text.Json;

public class JsonElementSerializer :  IBsonSerializer<JsonElement>
{
    public JsonElement Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Array)
        {
            // Deserialize the BSON array to a string array
            var jsonArray = BsonSerializer.Deserialize<BsonArray>(context.Reader);
            var json = jsonArray.ToString();

            // Parse the string to a JsonElement
            return JsonSerializer.Deserialize<JsonElement>(json);
        }
        else if (context.Reader.CurrentBsonType == BsonType.String)
        {
            // Deserialize the BSON value to a string
            var json = context.Reader.ReadString();

            // Parse the string to a JsonElement
            /*
            if (string.IsNullOrEmpty(json))
            {
                // Create an empty JSON array
                var emptyArray = JsonDocument.Parse("[]").RootElement;
                return emptyArray;
            }
            */

            return JsonSerializer.Deserialize<JsonElement>(json);
        }
        else
        {
            throw new InvalidOperationException("Invalid BSON type encountered.");
        }
    }


 public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
 {
     if (value is not JsonElement jsonElement)
     {
         throw new ArgumentException("The value must be of type JsonElement.", nameof(value));
     }

     // Serialize the JsonElement to a BSON string value
     var json = jsonElement.ToString();
     context.Writer.WriteString(json);
 }

 public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JsonElement value)
 {
     // Serialize the JsonElement to a BSON string value
     var json = value.ToString();
     context.Writer.WriteString(json);
 }

 object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
 {
     return Deserialize(context, args);
 }

 public Type ValueType => typeof(JsonElement);
}


