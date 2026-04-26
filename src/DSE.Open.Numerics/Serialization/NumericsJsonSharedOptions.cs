// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// Pre-configured <see cref="JsonSerializerOptions"/> sets for Numerics JSON
/// serialization. Two flavours: a reflection-based one that uses runtime
/// converters, and a source-generated one (<see cref="SourceGenerated"/>) that
/// is AOT-safe.
/// </summary>
public static class NumericsJsonSharedOptions
{
    private static readonly Lazy<JsonSerializerOptions> s_reflected = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping)
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        options.AddDefaultNumericsJsonConverters();
        return options;
    });

    private static readonly Lazy<JsonSerializerOptions> s_sourceGenerated = new(() =>
    {
        var options = new JsonSerializerOptions(s_reflected.Value)
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        options.TypeInfoResolverChain.Add(NumericsJsonSerializationContext.Default);
        options.MakeReadOnly();
        return options;
    });

    /// <summary>Reflection-based options. Not AOT-safe but supports any user converter that can be added at runtime.</summary>
    public static JsonSerializerOptions Reflected => s_reflected.Value;

    /// <summary>Source-generated options. AOT-safe; the recommended default for production code.</summary>
    public static JsonSerializerOptions SourceGenerated => s_sourceGenerated.Value;

    /// <summary>Alias for <see cref="SourceGenerated"/>.</summary>
    public static JsonSerializerOptions Default => SourceGenerated;
}
