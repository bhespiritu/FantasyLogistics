using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace BHMapGenerator.Schema;

public class SchemaValidator
{
    public static JSchema loadSchema()
    {
        using(var reader = new StreamReader("Schema/world.json"))
        {
            String data = reader.ReadToEnd();
            Console.WriteLine(data);
            JSchema schema = JSchema.Parse(data);
            Console.WriteLine(schema.Type);
// Object

            foreach (var property in schema.Properties)
            {
                Console.WriteLine(property.Key + " - " + property.Value.Type);
            }

            return schema;
        }
    }

    public static JSchema parseSchema(String schema)
    {
        return JSchema.Parse(schema);
    }
}