using FrooxEngine.Headless;
using Json.Schema;
using Json.Schema.Generation;
using NJsonSchema.Generation;
using SkyFrost.Base;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace JSONSchemaGenerator
{
    public class Program
    {
        public static Type[] TypesToGenerate = { typeof(AppConfig), typeof(HeadlessConfig) };
        private static void Main(String[] args)
        {
            // Up 4
            String projectDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "..", "..", ".."));;

            String outputPath = Path.Combine(projectDirectory, "output");
            Directory.CreateDirectory(outputPath);

            GenerateNJsonSchemas(outputPath);

            GeneratJsonSchemaNet(outputPath);
        }

        private static void GeneratJsonSchemaNet(String outputPath)
        {
            var config = new SchemaGeneratorConfiguration();
            

            JsonSerializerOptions jsonSerializerConfig = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            foreach (Type type in TypesToGenerate)
            {
                // https://localcoder.net/how-to-call-a-generic-method-using-a-type-variable-in-c
                // Although this website looks like it just copies answers from Stack Overflow *shrugs*
                MethodInfo genericMethod = typeof(Program).GetMethod(GenerateSchemaForName);
                MethodInfo constructedMethod = genericMethod.MakeGenericMethod(type);
                var schema = constructedMethod.Invoke(null, [jsonSerializerConfig, config]);

                File.WriteAllText($"{outputPath}/{type.Name}.schema.json", JsonSerializer.Serialize(schema, jsonSerializerConfig));
            }
        }

        public const string GenerateSchemaForName = "GenerateSchemaFor";
        public static JsonSchema GenerateSchemaFor<T>(JsonSerializerOptions serializerOptions, SchemaGeneratorConfiguration schemaGeneratiorConfig) {
            JsonSchemaBuilder builder = new JsonSchemaBuilder();
            schemaGeneratiorConfig.RegisterXmlCommentFile<T>();
            return builder.FromType<T>(schemaGeneratiorConfig).Build();
        }

        private static void GenerateNJsonSchemas(String outputPath)
        {
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

            foreach (Type type in TypesToGenerate)
            {
                File.WriteAllText($"{outputPath}/{type.Name}.NJSON.schema.json", systemGenerator.Generate(type).ToJson());
            }
        }
    }
}
