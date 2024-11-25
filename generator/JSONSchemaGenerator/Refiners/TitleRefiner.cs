using Json.Schema.Generation;
using Json.Schema.Generation.Intents;
using System;
using System.Collections;
using System.Linq;

namespace JSONSchemaGenerator.Refiners;

/// <summary>
/// Gives any JSON Objects, a title property. Filters out most non-JSON Objects.
/// </summary>
public class TitleRefiner : ISchemaRefiner
{
    public void Run(SchemaGenerationContextBase context)
    {
        context.Intents.Add(new TitleIntent(context.Type.Name));
    }

    public bool ShouldRun(SchemaGenerationContextBase context)
    {
        if (context.Intents.OfType<TitleIntent>().Any())
            return false;

        return !context.Type.IsPrimitive &&
            context.Type != typeof(string) &&
            context.Type != typeof(String) &&
            context.Type.GetInterfaces().All(f => f != typeof(IEnumerable)) &&
            !context.Type.IsNullableValueType() &&
            !context.Type.IsNullableNumber();
    }
}