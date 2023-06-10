// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using DSE.Open.Numerics;

namespace DSE.Open.Values;

[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface INominalValue<TSelf, T>
    : IValue<TSelf, T>,
      INominal<TSelf, T>
    where T
    : IEquatable<T>,
      IEqualityOperators<T, T, bool>,
      ISpanFormattable,
      ISpanParsable<T>
    where TSelf : struct, INominalValue<TSelf, T>
{
}
