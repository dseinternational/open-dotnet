// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics.Data;

[JsonConverter(typeof(SeriesJsonConverter))]
public abstract class ReadOnlySeries<T, TVector>
    : ReadOnlySeries,
      IReadOnlySeries<T, TVector>
    where TVector : ReadOnlyVector<T>
{
    protected ReadOnlySeries(string? name, TVector data, IReadOnlyDictionary<string, Variant>? annotations)
        : base(name, data, annotations)
    {
        ArgumentNullException.ThrowIfNull(data);
        Data = data;
    }

    public T this[int index] => Data[index];

    public new TVector Data { get; }
}

[JsonConverter(typeof(SeriesJsonConverter))]
public sealed class ReadOnlySeries<T> : ReadOnlySeries<T, ReadOnlyVector<T>>
{
    public ReadOnlySeries(
        string? name,
        ReadOnlyVector<T> data,
        IReadOnlyDictionary<string, Variant>? annotations)
        : base(name, data, annotations)
    {
    }
}

