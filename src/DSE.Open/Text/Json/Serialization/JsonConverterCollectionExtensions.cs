// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public static class JsonConverterCollectionExtensions
{
    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Calls AddDseOpenCoreJsonConverters with includeJsonValueObjectConverter = false")]
    [UnconditionalSuppressMessage("Trimming", "IL3050",
        Justification = "Calls AddDseOpenCoreJsonConverters with includeJsonValueObjectConverter = false")]
    public static void AddDseOpenCoreJsonConverters(this ICollection<JsonConverter> converters)
        => converters.AddDseOpenCoreJsonConverters(false);

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static void AddDseOpenCoreJsonConverters(
        this ICollection<JsonConverter> converters,
        bool includeJsonValueObjectConverter)
    {
        Guard.IsNotNull(converters);

        converters.Add(JsonStringBinaryValueBase64Converter.Default);
        converters.Add(JsonStringDiagnosticCodeConverter.Default);
        converters.Add(JsonStringInt128Converter.Default);

        converters.Add(JsonStringRangeConverter<byte>.Default);
        converters.Add(JsonStringRangeConverter<short>.Default);
        converters.Add(JsonStringRangeConverter<int>.Default);
        converters.Add(JsonStringRangeConverter<long>.Default);
        converters.Add(JsonStringRangeConverter<sbyte>.Default);
        converters.Add(JsonStringRangeConverter<ushort>.Default);
        converters.Add(JsonStringRangeConverter<uint>.Default);
        converters.Add(JsonStringRangeConverter<ulong>.Default);

        converters.Add(JsonStringSecureTokenConverter.Default);
        converters.Add(JsonStringTimePeriodConverter.Default);
        converters.Add(JsonStringTimestampConverter.Default);
        converters.Add(JsonStringUInt128Converter.Default);

        if (includeJsonValueObjectConverter)
        {
            converters.Add(JsonValueObjectConverter.Default);
        }
    }
}
