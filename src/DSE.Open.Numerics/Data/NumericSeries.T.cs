// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics.Data;

[JsonConverter(typeof(SeriesJsonConverter))]
public sealed class NumericSeries<T>
    : Series<T, NumericVector<T>>,
      INumericSeries<T, NumericVector<T>>,
      IReadOnlyNumericSeries<T, NumericVector<T>>,
      IAdditionOperators<NumericSeries<T>, NumericSeries<T>, NumericSeries<T>>,
      IAdditionOperators<NumericSeries<T>, T, NumericSeries<T>>,
      ISubtractionOperators<NumericSeries<T>, NumericSeries<T>, NumericSeries<T>>,
      ISubtractionOperators<NumericSeries<T>, T, NumericSeries<T>>
   where T : struct, INumber<T>
{
    public NumericSeries(
        string? name,
        NumericVector<T> values,
        IDictionary<string, Variant>? annotations)
        : base(name, values, annotations)
    {
    }

    public NumericSeries<T> Add(NumericSeries<T> series, string? name = default)
    {
        ArgumentNullException.ThrowIfNull(series);
        return Add(series.Data, name);
    }

    public NumericSeries<T> Add(NumericVector<T> vector, string? name = default)
    {
        return CreateNumeric(name, Data + vector);
    }

    public NumericSeries<T> Add(T scalar, string? name = default)
    {
        return CreateNumeric(name, Data + scalar);
    }

    public NumericSeries<T> Subtract(NumericSeries<T> series, string? name = default)
    {
        ArgumentNullException.ThrowIfNull(series);
        return Subtract(series.Data, name);
    }

    public NumericSeries<T> Subtract(NumericVector<T> vector, string? name = default)
    {
        return CreateNumeric(name, Data - vector);
    }

    public NumericSeries<T> Subtract(T scalar, string? name = default)
    {
        return CreateNumeric(name, Data - scalar);
    }

    public static NumericSeries<T> operator +(NumericSeries<T> left, NumericSeries<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Add(right);
    }

    public static NumericSeries<T> operator +(NumericSeries<T> left, T right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Add(right);
    }

    public static NumericSeries<T> operator -(NumericSeries<T> left, NumericSeries<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Subtract(right);
    }

    public static NumericSeries<T> operator -(NumericSeries<T> left, T right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Subtract(right);
    }
}
