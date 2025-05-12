// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics.Data;

[JsonConverter(typeof(SeriesJsonConverter))]
public abstract class Series<T, TVector> : Series, ISeries<T, TVector>
    where TVector : Vector<T>
{
    protected internal Series(string? name, TVector data, IDictionary<string, Variant>? annotations)
        : base(name, data, annotations)
    {
        ArgumentNullException.ThrowIfNull(data);
        Data = data;
    }

    public T this[int index] => Data[index];

    public new TVector Data { get; }

    TVector IReadOnlySeries<T, TVector>.Data => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan()
    {
        return Data.AsSpan();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> Slice(int start)
    {
        return AsSpan()[start..];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> Slice(int start, int length)
    {
        return AsSpan().Slice(start, length);
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    ReadOnlySpan<T> IReadOnlySeries<T, TVector>.AsReadOnlySpan()
#pragma warning restore CA1033 // Interface methods should be callable by child types
    {
        return AsSpan();
    }

    ReadOnlySpan<T> IReadOnlySeries<T, TVector>.Slice(int start, int length)
    {
        return Slice(start, length);
    }
}

[JsonConverter(typeof(SeriesJsonConverter))]
public sealed class Series<T>
    : Series<T, Vector<T>>,
      ISeries<T, Vector<T>>,
      IReadOnlySeries<T, Vector<T>>
{
    public Series(
        string? name,
        Vector<T> data,
        IDictionary<string, Variant>? annotations)
        : base(name, data, annotations)
    {
    }
}
