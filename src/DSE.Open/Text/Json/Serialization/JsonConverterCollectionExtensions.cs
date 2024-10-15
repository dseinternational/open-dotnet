// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public static class JsonConverterCollectionExtensions
{
    public static void AddDseOpenCoreJsonConverters(this ICollection<JsonConverter> converters)
    {
        ArgumentNullException.ThrowIfNull(converters);

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
    }
}
