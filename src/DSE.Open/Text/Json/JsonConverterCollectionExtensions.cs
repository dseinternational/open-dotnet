// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json;

public static class JsonConverterCollectionExtensions
{
    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Calls AddDseOpenCoreJsonConverters with includeJsonValueObjectConverter = false")]
    public static void AddDseOpenCoreJsonConverters(this ICollection<JsonConverter> converters)
        => converters.AddDseOpenCoreJsonConverters(false);

    [RequiresUnreferencedCode("Including the JsonValueObjectConverter might require types " +
        "that cannot be statically analyzed.")]
    public static void AddDseOpenCoreJsonConverters(
        this ICollection<JsonConverter> converters,
        bool includeJsonValueObjectConverter)
    {
        Guard.IsNotNull(converters);

        converters.Add(JsonStringDiagnosticCodeConverter.Default);
        converters.Add(JsonStringInt128Converter.Default);
        converters.Add(JsonStringTimestampConverter.Default);
        converters.Add(JsonStringUInt128Converter.Default);

        if (includeJsonValueObjectConverter)
        {
            converters.Add(JsonValueObjectConverter.Default);
        }
    }
}
