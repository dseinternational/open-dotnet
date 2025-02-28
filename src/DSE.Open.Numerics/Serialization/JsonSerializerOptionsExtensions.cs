// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

#pragma warning disable CA2225 // Operator overloads have named alternates

public static class JsonSerializerOptionsExtensions
{
    public static void AddDataPointArrayJsonConverter<T>(this IList<JsonConverter> converters)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(converters);
        converters.Add(new DataPointArrayJsonConverter<T>());
    }

    public static void AddDefaultNumericsJsonConverters(this JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.Converters.AddDataPointArrayJsonConverter<byte>();
        options.Converters.AddDataPointArrayJsonConverter<short>();
        options.Converters.AddDataPointArrayJsonConverter<int>();
        options.Converters.AddDataPointArrayJsonConverter<long>();
        options.Converters.AddDataPointArrayJsonConverter<ushort>();
        options.Converters.AddDataPointArrayJsonConverter<uint>();
        options.Converters.AddDataPointArrayJsonConverter<ulong>();
        options.Converters.AddDataPointArrayJsonConverter<float>();
        options.Converters.AddDataPointArrayJsonConverter<double>();
    }
}
