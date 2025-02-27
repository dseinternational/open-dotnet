// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

#pragma warning disable CA2225 // Operator overloads have named alternates

public static class JsonSerializerOptionsExtensions
{
    public static void AddVectorJsonConverter<T>(this IList<JsonConverter> converters)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(converters);
        Vector.EnsureNotKnownNumericType(typeof(T));
        converters.Add(new VectorJsonConverter<T>());
    }

    public static void AddNumericVectorJsonConverter<T>(this IList<JsonConverter> converters)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(converters);
        Vector.EnsureKnownNumericType(typeof(T));
        converters.Add(new NumericVectorJsonConverter<T>());
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

        options.Converters.AddVectorJsonConverter<char>();
        options.Converters.AddVectorJsonConverter<string>();
        options.Converters.AddVectorJsonConverter<DateTime>();
        options.Converters.AddVectorJsonConverter<DateTimeOffset>();
        options.Converters.AddVectorJsonConverter<DateOnly>();
        options.Converters.AddVectorJsonConverter<TimeOnly>();
        options.Converters.AddVectorJsonConverter<Guid>();

        options.Converters.AddNumericVectorJsonConverter<byte>();
        options.Converters.AddNumericVectorJsonConverter<DateTime64>();
        options.Converters.AddNumericVectorJsonConverter<decimal>();
        options.Converters.AddNumericVectorJsonConverter<double>();
        options.Converters.AddNumericVectorJsonConverter<float>();
        options.Converters.AddNumericVectorJsonConverter<int>();
        options.Converters.AddNumericVectorJsonConverter<Int128>();
        options.Converters.AddNumericVectorJsonConverter<long>();
        options.Converters.AddNumericVectorJsonConverter<sbyte>();
        options.Converters.AddNumericVectorJsonConverter<short>();
        options.Converters.AddNumericVectorJsonConverter<uint>();
        options.Converters.AddNumericVectorJsonConverter<UInt128>();
        options.Converters.AddNumericVectorJsonConverter<ulong>();
        options.Converters.AddNumericVectorJsonConverter<ushort>();

        options.Converters.AddDataPointArrayJsonConverter<byte, byte>();
        options.Converters.AddDataPointArrayJsonConverter<short, short>();
        options.Converters.AddDataPointArrayJsonConverter<int, int>();
        options.Converters.AddDataPointArrayJsonConverter<long, long>();
        options.Converters.AddDataPointArrayJsonConverter<ushort, ushort>();
        options.Converters.AddDataPointArrayJsonConverter<uint, uint>();
        options.Converters.AddDataPointArrayJsonConverter<ulong, ulong>();
        options.Converters.AddDataPointArrayJsonConverter<float, float>();
        options.Converters.AddDataPointArrayJsonConverter<double, double>();
    }
}
