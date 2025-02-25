// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;

namespace DSE.Open.Numerics;
#pragma warning disable CA2225 // Operator overloads have named alternates

public static class JsonSerializerOptionsExtensions
{
    public static void AddNumericsJsonConverters<T>(this JsonSerializerOptions options)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(options);

        options.Converters.Add(new VectorJsonConverter<byte>());
        options.Converters.Add(new VectorJsonConverter<short>());
        options.Converters.Add(new VectorJsonConverter<int>());
        options.Converters.Add(new VectorJsonConverter<long>());
        options.Converters.Add(new VectorJsonConverter<ushort>());
        options.Converters.Add(new VectorJsonConverter<uint>());
        options.Converters.Add(new VectorJsonConverter<ulong>());
        options.Converters.Add(new VectorJsonConverter<float>());
        options.Converters.Add(new VectorJsonConverter<double>());

        options.Converters.Add(new DataPointArrayJsonConverter<byte, byte>());
        options.Converters.Add(new DataPointArrayJsonConverter<short, short>());
        options.Converters.Add(new DataPointArrayJsonConverter<int, int>());
        options.Converters.Add(new DataPointArrayJsonConverter<long, long>());
        options.Converters.Add(new DataPointArrayJsonConverter<ushort, ushort>());
        options.Converters.Add(new DataPointArrayJsonConverter<uint, uint>());
        options.Converters.Add(new DataPointArrayJsonConverter<ulong, ulong>());
    }
}
