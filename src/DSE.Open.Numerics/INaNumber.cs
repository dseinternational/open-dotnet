// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public interface INaNumber<TSelf, T>
    : INumber<TSelf>,
      IMinMaxValue<TSelf>,
      INaValue<TSelf, T>,
      ITernaryEquatable<TSelf>
    where T : struct, INumber<T>, IMinMaxValue<T>
    where TSelf : struct, INaNumber<TSelf, T>
{
    static abstract TSelf FromValue(T value);
}
