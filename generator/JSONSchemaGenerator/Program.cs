using FrooxEngine.Headless;
using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.DataAnnotations;
using JSONSchemaGenerator.Refiners;
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
            // Up 5
            //TODO: this is a mess
            String projectDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "..", "..")); ;

            String outputPath = Path.Combine(projectDirectory, "schemas");
            Directory.CreateDirectory(outputPath);

            GenerateSchemas(outputPath);
        }

        private static void GenerateSchemas(String outputPath)
        {
            SchemaGeneratorConfiguration config = new()
            {
                Nullability = Nullability.AllowForAllTypes,
                Refiners = {
                    new ObsoleteEnumRefiner(),
                    new TitleRefiner()
                }//,
                //PropertyNameResolver = PropertyNameResolvers.CamelCase
            };

            JsonSerializerOptions jsonSerializerConfig = new()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            DataAnnotationsSupport.AddDataAnnotations();

            foreach (Type type in TypesToGenerate)
            {
                // https://localcoder.net/how-to-call-a-generic-method-using-a-type-variable-in-c
                // Although this website looks like it just copies answers from Stack Overflow *shrugs*
                MethodInfo? genericMethod = typeof(Program).GetMethod(GenerateSchemaForName);
                MethodInfo? constructedMethod = genericMethod?.MakeGenericMethod(type);
                JsonSchema? schema = (JsonSchema?)(constructedMethod?.Invoke(null, [jsonSerializerConfig, config]));
                if (schema == null)
                    continue;

                File.WriteAllText($"{outputPath}/{type.Name}.schema.json", JsonSerializer.Serialize(schema, jsonSerializerConfig));
            }
        }

        public const String GenerateSchemaForName = "GenerateSchemaFor";
        public static JsonSchema GenerateSchemaFor<T>(JsonSerializerOptions serializerOptions, SchemaGeneratorConfiguration schemaGeneratiorConfig)
        {
            JsonSchemaBuilder builder = new();
            schemaGeneratiorConfig.RegisterXmlCommentFile<T>();
            return builder.FromType<T>(schemaGeneratiorConfig).Schema(Json.Schema.MetaSchemas.DraftNextId)
                .Build();
        }
    }
}
