// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

public static class JsonConverterCollectionExtensions
{
    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Calls AddDseOpenValuesJsonConverters with includeJsonValueObjectConverter = false")]
    public static void AddDseOpenValuesJsonConverters(this ICollection<JsonConverter> converters)
        => converters.AddDseOpenValuesJsonConverters(false);

    [RequiresUnreferencedCode("Including the JsonValueObjectConverter might require types " +
        "that cannot be statically analyzed.")]
    public static void AddDseOpenValuesJsonConverters(
        this ICollection<JsonConverter> converters,
        bool includeJsonValueObjectConverter)
    {
        ArgumentNullException.ThrowIfNull(converters);

        converters.AddDseOpenValuesJsonConverters(includeJsonValueObjectConverter);

    }
}
