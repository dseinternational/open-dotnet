// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics.Data;

public interface ICategoricalSeries<T, TVector> : ISeries<T, TVector>
    where TVector : ICategoricalVector<T>
    where T : struct,
              IComparable<T>,
              IEquatable<T>,
              IBinaryInteger<T>,
              IMinMaxValue<T>
{
}

