// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// An numeric value that may be 'not a value', missing, or not available, such as <see langword="null"/> or
/// <see cref="INaValue{TSelf, T}.Na"/>.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// <see cref="INaNumber{TSelf, T}"/> implements <see cref="INumber{TSelf}"/> - typically, forwarding to the
/// existing <see cref="INumber{T}"/> implementation of the underlying value type <typeparamref name="T"/>.
/// By implication, <see cref="INaNumber{TSelf, T}"/> therefore also implements <see cref="IEquatable{TSelf}"/>.
/// Implementors should follow the behaviour of <see langword="float"/> (<c>float.NaN == float.NaN</c> returns
/// <see langword="true"/> and <c>float.NaN.Equals(float.NaN)</c> returns <see langword="false"/>) when handling
/// equality tests.
/// <para><see cref="ITernaryEquatable{T}"/> provides additional methods for explicit three-valued logic
/// comparisons.</para>
/// <para>Implementors should default to <see cref="IEqualityOperators{TSelf, TOther, Boolean}"/> as
/// defined for <see cref="INumberBase{TSelf}"/>. See remarks on <see cref="ITernaryEquatable{T}"/> about
/// equality operators returning <see cref="Trilean"/> results.</para>
/// </remarks>
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
