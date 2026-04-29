// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Utf8String"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class Utf8StringToStringConverter : ValueConverter<Utf8String, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly Utf8StringToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="Utf8StringToStringConverter"/>.
    /// </summary>
    /// <param name="mappingHints">Optional EF Core mapping hints.</param>
    public Utf8StringToStringConverter(ConverterMappingHints? mappingHints = null)
        : base(ToStore(), FromStore(), mappingHints)
    {
    }

    /// <summary>
    /// Returns an expression that converts a <see cref="Utf8String"/> to its <see cref="string"/>
    /// storage form.
    /// </summary>
    /// <returns>An expression that produces the string.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<Utf8String, string>> ToStore()
    {
        return value => value.ToString();
    }

    /// <summary>
    /// Returns an expression that creates a <see cref="Utf8String"/> from its <see cref="string"/>
    /// storage form.
    /// </summary>
    /// <returns>An expression that produces the <see cref="Utf8String"/>.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<string, Utf8String>> FromStore()
    {
        return value => new(value);
    }
}
