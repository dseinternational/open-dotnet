// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open;

/// <summary>
/// Represents a type that can be converted to another type.
/// </summary>
/// <typeparam name="TFrom">The type that can be converted.</typeparam>
/// <typeparam name="TTo">The type that <typeparamref name="TFrom"/> can be converted to.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Cannot name from type parameter")]
public interface IConvertibleTo<TFrom, TTo>
    where TFrom : IConvertibleTo<TFrom, TTo>
{
    /// <summary>
    /// Gets a value of type <typeparamref name="TTo"/> from a <typeparamref name="TFrom"/> value.
    /// </summary>
    /// <param name="value"></param>
    static virtual implicit operator TTo(TFrom value) => TFrom.ConvertTo(value);

    static abstract TTo ConvertTo(TFrom value);
}
