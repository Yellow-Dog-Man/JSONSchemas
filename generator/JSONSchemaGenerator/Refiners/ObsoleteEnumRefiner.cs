using Json.Schema.Generation;
using Json.Schema.Generation.Intents;
using System;
using System.Linq;

namespace JSONSchemaGenerator.Refiners;

/// <summary>
/// Removes Obsolete Enum values from an EnumIntent.
/// </summary>
/// <remarks>The standard generator will output obsolete values for enums. This makes sense from a "schema" perspective as 
/// these items still exist and are therefore valid for the schema. But for documentation reasons, we should remove them.</remarks>
/// TODO: Would https://json-everything.net/ be interested in using this?
/// Based on work in: https://github.com/DW2MC/DW2ModLoader/blob/main/ModDevToolsMod/Dw2ContentDefinitionSchemaRefiner.cs and 
/// https://github.com/LicoGame/Magus/blob/main/src/Magus.Json/MagusRefiner.cs without which I would have no idea how to do this.
public class ObsoleteEnumRefiner : ISchemaRefiner
{
    public void Run(SchemaGenerationContextBase context)
    {
        Type type = context.Type;

        // Shouldn't happen, but better safe then sorry.
        if (!type.IsEnum)
        {
            return;
        }

        EnumIntent enumIntent = context.Intents.OfType<EnumIntent>().First();

        if (enumIntent == null)
        {
            return;
        }

        enumIntent.Names = enumIntent.Names.Where((name) => !ShouldFilterEnumName(type, name)).ToList();
    }

    public static Boolean IsObsolete(Type type, String key)
    {
        return type.GetField(key)?.GetCustomAttributes(typeof(ObsoleteAttribute), false).Any() ?? false;
    }

    public Boolean ShouldFilterEnumName(Type type, String name)
    {
        return IsObsolete(type, name);
    }

    public Boolean ShouldRun(SchemaGenerationContextBase context)
    {
        // Only run on enums
        return context.Type.IsEnum;
    }
}
