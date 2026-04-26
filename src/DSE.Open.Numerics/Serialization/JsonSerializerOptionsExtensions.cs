// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// Extension methods for registering Numerics-specific converters on a
/// <see cref="JsonSerializerOptions"/>.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    /// <summary>Adds a <see cref="DataPointArrayJsonConverter{T}"/> for the supplied <typeparamref name="T"/>.</summary>
    public static void AddDataPointArrayJsonConverter<T>(this IList<JsonConverter> converters)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(converters);
        converters.Add(new DataPointArrayJsonConverter<T>());
    }

    /// <summary>Adds a <see cref="DataPoint3DArrayJsonConverter{T}"/> for the supplied <typeparamref name="T"/>.</summary>
    public static void AddDataPoint3DArrayJsonConverter<T>(this IList<JsonConverter> converters)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(converters);
        converters.Add(new DataPoint3DArrayJsonConverter<T>());
    }

    /// <summary>
    /// Registers <see cref="DataPointArrayJsonConverter{T}"/> and
    /// <see cref="DataPoint3DArrayJsonConverter{T}"/> instances for every
    /// supported numeric element type. Idempotent for these specific
    /// converters but does not deduplicate against entries that may already
    /// exist on <paramref name="options"/>.
    /// </summary>
    public static void AddDefaultNumericsJsonConverters(this JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.Converters.AddDataPointArrayJsonConverter<byte>();
        options.Converters.AddDataPointArrayJsonConverter<sbyte>();
        options.Converters.AddDataPointArrayJsonConverter<short>();
        options.Converters.AddDataPointArrayJsonConverter<ushort>();
        options.Converters.AddDataPointArrayJsonConverter<int>();
        options.Converters.AddDataPointArrayJsonConverter<uint>();
        options.Converters.AddDataPointArrayJsonConverter<long>();
        options.Converters.AddDataPointArrayJsonConverter<ulong>();
        options.Converters.AddDataPointArrayJsonConverter<Half>();
        options.Converters.AddDataPointArrayJsonConverter<float>();
        options.Converters.AddDataPointArrayJsonConverter<double>();
        options.Converters.AddDataPointArrayJsonConverter<decimal>();
        options.Converters.AddDataPointArrayJsonConverter<DateTime64>();

        options.Converters.AddDataPoint3DArrayJsonConverter<byte>();
        options.Converters.AddDataPoint3DArrayJsonConverter<sbyte>();
        options.Converters.AddDataPoint3DArrayJsonConverter<short>();
        options.Converters.AddDataPoint3DArrayJsonConverter<ushort>();
        options.Converters.AddDataPoint3DArrayJsonConverter<int>();
        options.Converters.AddDataPoint3DArrayJsonConverter<uint>();
        options.Converters.AddDataPoint3DArrayJsonConverter<long>();
        options.Converters.AddDataPoint3DArrayJsonConverter<ulong>();
        options.Converters.AddDataPoint3DArrayJsonConverter<Half>();
        options.Converters.AddDataPoint3DArrayJsonConverter<float>();
        options.Converters.AddDataPoint3DArrayJsonConverter<double>();
        options.Converters.AddDataPoint3DArrayJsonConverter<decimal>();
        options.Converters.AddDataPoint3DArrayJsonConverter<DateTime64>();
    }
}
