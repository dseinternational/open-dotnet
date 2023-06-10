// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Values;

[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface IMinMaxValue<TSelf, T> : IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>
    where T : IEquatable<T>, IMinMaxValue<T>
    where TSelf : struct, IMinMaxValue<TSelf, T>
{
    static abstract TSelf MinValue { get; }

    static abstract TSelf MaxValue { get; }
}