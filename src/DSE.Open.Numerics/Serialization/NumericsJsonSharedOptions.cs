// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics.Serialization;

public static class NumericsJsonSharedOptions
{
    private static readonly Lazy<JsonSerializerOptions> s_reflected = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping);
        options.AddDefaultNumericsJsonConverters();
        return options;
    });

    private static readonly Lazy<JsonSerializerOptions> s_sourceGenerated = new(() =>
    {
        var options = new JsonSerializerOptions(s_reflected.Value);
        options.TypeInfoResolverChain.Add(NumericsJsonSerializationContext.Default);
        options.MakeReadOnly();
        return options;
    });

    public static JsonSerializerOptions Reflected => s_reflected.Value;

    public static JsonSerializerOptions SourceGenerated => s_sourceGenerated.Value;

    public static JsonSerializerOptions Default => SourceGenerated;
}
