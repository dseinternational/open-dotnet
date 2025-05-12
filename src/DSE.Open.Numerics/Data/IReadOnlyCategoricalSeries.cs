// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics.Data;

public interface IReadOnlyCategoricalSeries<T, TVector> : IReadOnlySeries<T, TVector>
    where TVector : IReadOnlyCategoricalVector<T>
    where T : struct,
              IComparable<T>,
              IEquatable<T>,
              IBinaryInteger<T>,
              IMinMaxValue<T>
{
}

