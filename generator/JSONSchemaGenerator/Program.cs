using FrooxEngine.Headless;
using Json.Schema;
using Json.Schema.Generation;
using NJsonSchema.Generation;
using SkyFrost.Base;
using System;
using System.IO;
using System.Text.Json;

namespace JSONSchemaGenerator
{
    internal class Program
    {
        private static void Main(String[] args)
        {
            String projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            String outputPath = Path.Combine(projectDirectory, "output");
            _=Directory.CreateDirectory(outputPath);
            GenerateNJsonSchemas(outputPath);

            GeneratJsonSchemaNet(outputPath);
        }

        private static void GeneratJsonSchemaNet(String outputPath)
        {
            JsonSchemaBuilder builder = new JsonSchemaBuilder();
            _ = new SchemaGeneratorConfiguration();

            // Get XML file from The Assemblies we need.

            // MyModel is any type from the assembly.  A single registration will
            // cover the entire assembly.
            // https://docs.json-everything.net/schema/schemagen/schema-generation/#xml-comment-support
            //config.RegisterXmlCommentFile<AppConfig>("MyAssembly.xml");

            JsonSchema schema = builder.FromType<AppConfig>().Build();
            JsonSerializerOptions jsonSerializerConfig = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            File.WriteAllText($"{outputPath}/AppConfig.new.schema.json", JsonSerializer.Serialize(schema, jsonSerializerConfig));
        }

        private static void GenerateNJsonSchemas(String outputPath)
        {
            Type[] systemJsonTypes = { typeof(AppConfig), typeof(HeadlessConfig) };
            SystemTextJsonSchemaGeneratorSettings systemJsonSettings = new SystemTextJsonSchemaGeneratorSettings
            {
                SerializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                },
                // VSCode hates me?
                FlattenInheritanceHierarchy = true
            };

            JsonSchemaGenerator systemGenerator = new JsonSchemaGenerator(systemJsonSettings);

            foreach (Type type in systemJsonTypes)
            {
                File.WriteAllText($"{outputPath}{type.Name}.schema.json", systemGenerator.Generate(type).ToJson());
            }
        }
    }
}
