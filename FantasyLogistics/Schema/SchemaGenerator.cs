using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace FantasyLogistics.DefaultSchemas;

public class SchemaGenerator
{
    public static void generateSchema(Type t)
    {
        JSchemaGenerator generator = new JSchemaGenerator();

        JSchema schema = generator.Generate(t);
    }
}