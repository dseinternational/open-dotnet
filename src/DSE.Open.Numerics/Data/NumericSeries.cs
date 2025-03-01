// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using DSE.Open.Numerics.Serialization;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Data;

[JsonConverter(typeof(SeriesJsonConverter))]
public sealed class NumericSeries<T> : Series<T, NumericVector<T>>
   where T : struct, INumber<T>
{
    public NumericSeries(
        string? name,
        NumericVector<T> values,
        IDictionary<string, Variant>? annotations)
        : base(name, values, annotations)
    {
    }
}
