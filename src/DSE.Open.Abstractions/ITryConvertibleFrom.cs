// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace DSE.Open;

/// <summary>
/// Represents a type that may be converted from another type.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="TFrom"></typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Cannot name from type parameter")]
public interface ITryConvertibleFrom<TSelf, TFrom>
    where TSelf : ITryConvertibleFrom<TSelf, TFrom>
{
    /// <summary>
    /// Attempts to convert a value of type <typeparamref name="TFrom"/> to a value of type <typeparamref name="TSelf"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was converted to a
    /// valid <typeparamref name="TSelf"/> value contained in <paramref name="result"/>, otherwise
    /// <see langword="false"/>.</returns>
    static abstract bool TryFromValue(TFrom value, out TSelf result);

    /// <summary>
    /// Converts a value of type <typeparamref name="TFrom"/> to a value of type <typeparamref name="TSelf"/>.
    /// </summary>
    /// <param name="value">A valid value of type <typeparamref name="TSelf"/> that was represented
    /// by <paramref name="value"/>.</param>
    static abstract explicit operator TSelf(TFrom value);

    static virtual TSelf FromValue(TFrom value)
    {
        return !TSelf.TryFromValue(value, out var result)
            ? throw new InvalidCastException($"Cannot convert {value} to {typeof(TSelf)}")
            : result;
    }
}
