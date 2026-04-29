// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// A value converter that maps an <see cref="IValue{TValue, T}"/> wrapper to and from a
/// distinct store representation, going via the underlying primitive type
/// <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="TValue">The wrapper value type.</typeparam>
/// <typeparam name="T">The underlying primitive type wrapped by <typeparamref name="TValue"/>.</typeparam>
/// <typeparam name="TStore">The store representation.</typeparam>
[SuppressMessage("Design", "CA1005:Avoid excessive parameters on generic types", Justification = "<Pending>")]
public sealed class ValueTypeValueConverter<TValue, T, TStore> : ValueConverter<TValue, TStore>
    where T : IEquatable<T>, IConvertibleTo<T, TStore>, ITryConvertibleFrom<T, TStore>
    where TValue : struct, IValue<TValue, T>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ValueTypeValueConverter{TValue, T, TStore}"/>.
    /// </summary>
    public ValueTypeValueConverter()
        : base(v => ToStore(v), v => FromStore(v), default)
    {
    }

    /// <summary>
    /// Converts a <typeparamref name="TValue"/> to its <typeparamref name="TStore"/> store form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The store representation.</returns>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TStore ToStore(TValue value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return (TStore)(T)value;
    }

    /// <summary>
    /// Converts a <typeparamref name="TStore"/> store value back to a <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="value">The store value to convert.</param>
    /// <returns>The wrapper value.</returns>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TValue FromStore(TStore value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return (TValue)(T)value;
    }
}

/// <summary>
/// A value converter that maps an <see cref="IValue{TValue, T}"/> wrapper to and from its
/// underlying primitive type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="TValue">The wrapper value type.</typeparam>
/// <typeparam name="T">The underlying primitive type wrapped by <typeparamref name="TValue"/>.</typeparam>
public sealed class ValueTypeValueConverter<TValue, T> : ValueConverter<TValue, T>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ValueTypeValueConverter{TValue, T}"/>.
    /// </summary>
    public ValueTypeValueConverter()
        : base(v => ToStore(v), v => FromStore(v), default)
    {
    }

    /// <summary>
    /// Converts a <typeparamref name="TValue"/> to its underlying <typeparamref name="T"/> form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The underlying primitive value.</returns>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static T ToStore(TValue value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return (T)value;
    }

    /// <summary>
    /// Converts a <typeparamref name="T"/> store value back to a <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="value">The store value to convert.</param>
    /// <returns>The wrapper value.</returns>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TValue FromStore(T value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return (TValue)value;
    }
}
