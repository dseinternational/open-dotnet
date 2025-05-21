// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open;

/// <summary>
/// A common interface for types that can be compared using a ternary comparison (<see cref="TernaryEquals(T)"/>),
/// returning a <see cref="Trilean"/> result indicating <see langword="true"/>,  <see langword="false"/>
/// or <c>NA</c> (unknown, missing, <see langword="null"/> or NaN).
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// <see cref="ITernaryEquatable{T}"/> does not extend <see cref="IEquatable{T}"/>. However, implementors
/// should consider implementing <see cref="IEquatable{T}"/> to disambiguate equality comparisons.
/// <para>Implementors should avoid implementing default equality operators returning <see cref="Trilean"/>
/// in preference to <see langword="bool"/> (<see cref="IEqualityOperators{TSelf, TOther, Trilean}"/>)
/// as this may be unexpected for many .NET users. If essential, implement
/// <see cref="IEqualityOperators{TSelf, TOther, Trilean}"/> explcitly and rely on users wanting that
/// behaviour from operators to cast appropriately.</para>
/// </remarks>
public interface ITernaryEquatable<T>
{
    /// <summary>
    /// Returns a ternary comparison of the two values.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>
    /// A <see cref="Trilean"/> giving the result of the comparison: if the two values are
    /// <b>known to be equal</b>, returns <see cref="Open.Trilean.True"/>, if the two values are
    /// <b>known to be not equal</b>, returns <see cref="Open.Trilean.False"/>, <b>otherwise</b>
    /// returns , returns <see cref="Open.Trilean.Na"/> (either or both values are 'unknown').
    /// </returns>
    Trilean TernaryEquals(T other);

    /// <summary>
    /// Returns <see langword="true"/> if the two values are known to be equal and neither is
    /// <c>NA</c> (such as <see langword="null"/> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    virtual bool EqualAndNotNa(T other)
    {
        return (TernaryEquals(other) == Trilean.True).IsTrue;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the two values are known to be equal or <b>both</b> are
    /// <c>NA</c> (such as <see langword="null"/> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    bool EqualOrBothNa(T other);

    /// <summary>
    /// Returns <see langword="true"/> if the two values are known to be equal or <b>either</b> are
    /// <c>NA</c> (such as <see langword="null"/> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    bool EqualOrEitherNa(T other);
}
