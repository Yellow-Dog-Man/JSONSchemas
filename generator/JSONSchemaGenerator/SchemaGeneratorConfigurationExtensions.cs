using Json.Schema.Generation;

namespace JSONSchemaGenerator;

public static class SchemaGeneratorConfigurationExtensions
{
    // From: https://docs.json-everything.net/schema/schemagen/schema-generation/#xml-comment-support
    public static void RegisterXmlCommentFile<T>(this SchemaGeneratorConfiguration configuration)
    {
        configuration.RegisterXmlCommentFile<T>(typeof(T).Assembly.Location.Replace(".dll", ".xml"));
    }
}
