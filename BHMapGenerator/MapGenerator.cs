using System.Text.Json.Serialization;
using BHMapGenerator.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace BHMapGenerator;

public class MapGenerator
{
    private JSchema _associatedSchema;

    private World w;
    
    public static void Main()
    {
        MapGenerator mapGenerator = MapGenerator.fromSchema(SchemaValidator.loadSchema());
        String s = "s";
    }

    public static MapGenerator fromSchema(String schema)
    {
        MapGenerator mapGenInstance = new MapGenerator();

        mapGenInstance.w = JsonConvert.DeserializeObject<BHMapGenerator.World>(schema);

        return mapGenInstance;
    }
    
    public static MapGenerator fromSchema(JSchema schema)
    {
        return fromSchema(schema.ToString());
    }
}