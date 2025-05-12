// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics.Data;

[JsonConverter(typeof(SeriesJsonConverter))]
public sealed class CategoricalSeries<T, TVector> : Series<T, TVector>
    where TVector : ICategoricalVector<T>
    where T : struct,
              IComparable<T>,
              IEquatable<T>,
              IBinaryInteger<T>,
              IMinMaxValue<T>
{
    public CategoricalSeries(string? name, TVector data, IDictionary<string, Variant>? annotations)
        : base(name, data, annotations)
    {
    }
}
