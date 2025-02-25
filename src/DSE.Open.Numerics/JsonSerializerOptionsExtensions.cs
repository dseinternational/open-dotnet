// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

#pragma warning disable CA2225 // Operator overloads have named alternates

public static class JsonSerializerOptionsExtensions
{
    public static void AddVectorJsonConverter<T>(this IList<JsonConverter> converters)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(converters);
        converters.Add(new VectorJsonConverter<T>());
    }

    public static void AddDataPointArrayJsonConverter<TX, TY>(this IList<JsonConverter> converters)
        where TX : struct, INumber<TX>
        where TY : struct, INumber<TY>
    {
        ArgumentNullException.ThrowIfNull(converters);
        converters.Add(new DataPointArrayJsonConverter<TX, TY>());
    }

    public static void AddDefaultNumericsJsonConverters(this JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.Converters.AddVectorJsonConverter<byte>();
        options.Converters.AddVectorJsonConverter<short>();
        options.Converters.AddVectorJsonConverter<int>();
        options.Converters.AddVectorJsonConverter<long>();
        options.Converters.AddVectorJsonConverter<ushort>();
        options.Converters.AddVectorJsonConverter<uint>();
        options.Converters.AddVectorJsonConverter<ulong>();
        options.Converters.AddVectorJsonConverter<float>();
        options.Converters.AddVectorJsonConverter<double>();

        options.Converters.AddDataPointArrayJsonConverter<byte, byte>();
        options.Converters.AddDataPointArrayJsonConverter<short, short>();
        options.Converters.AddDataPointArrayJsonConverter<int, int>();
        options.Converters.AddDataPointArrayJsonConverter<long, long>();
        options.Converters.AddDataPointArrayJsonConverter<ushort, ushort>();
        options.Converters.AddDataPointArrayJsonConverter<uint, uint>();
        options.Converters.AddDataPointArrayJsonConverter<ulong, ulong>();
    }
}
