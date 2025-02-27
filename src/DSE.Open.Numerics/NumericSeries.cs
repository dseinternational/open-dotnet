// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

public sealed class NumericSeries<T> : Series<T, NumericVector<T>>
   where T : struct, INumber<T>
{
    [JsonConstructor]
    public NumericSeries(
        string? name,
        NumericVector<T> values,
        IDictionary<T, Variant>? references)
        : base(name, values, references)
    {
    }
}
