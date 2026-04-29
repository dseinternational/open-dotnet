// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts numeric values between two <see cref="INumberBase{TSelf}"/> types using checked arithmetic.
/// </summary>
/// <typeparam name="TFrom">The source numeric type.</typeparam>
/// <typeparam name="TTo">The destination (storage) numeric type.</typeparam>
public sealed class NumberConverter<TFrom, TTo> : ValueConverter<TFrom, TTo>
    where TFrom : INumberBase<TFrom>
    where TTo : INumberBase<TTo>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly NumberConverter<TFrom, TTo> Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="NumberConverter{TFrom, TTo}"/>.
    /// </summary>
    public NumberConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <typeparamref name="TFrom"/> value to a <typeparamref name="TTo"/> using checked arithmetic.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="OverflowException">Thrown when <paramref name="value"/> is outside the range of <typeparamref name="TTo"/>.</exception>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TTo ConvertTo(TFrom value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return TTo.CreateChecked(value);
    }

    /// <summary>
    /// Converts a <typeparamref name="TTo"/> storage value back to a <typeparamref name="TFrom"/> using checked arithmetic.
    /// </summary>
    /// <param name="value">The stored value to convert.</param>
    /// <returns>The reconstructed value.</returns>
    /// <exception cref="OverflowException">Thrown when <paramref name="value"/> is outside the range of <typeparamref name="TFrom"/>.</exception>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TFrom ConvertFrom(TTo value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return TFrom.CreateChecked(value);
    }
}
