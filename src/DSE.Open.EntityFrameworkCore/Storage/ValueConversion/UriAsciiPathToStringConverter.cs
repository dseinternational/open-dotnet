// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="UriAsciiPath"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class UriAsciiPathToStringConverter : ValueConverter<UriAsciiPath, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly UriAsciiPathToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="UriAsciiPathToStringConverter"/>.
    /// </summary>
    /// <param name="mappingHints">Optional EF Core mapping hints.</param>
    public UriAsciiPathToStringConverter(ConverterMappingHints? mappingHints = null)
        : base(ToStore(), FromStore(), mappingHints)
    {
    }

    /// <summary>
    /// Returns an expression that converts a <see cref="UriAsciiPath"/> to its <see cref="string"/>
    /// storage form.
    /// </summary>
    /// <returns>An expression that produces the string.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<UriAsciiPath, string>> ToStore()
    {
        return value => value.ToString();
    }

    /// <summary>
    /// Returns an expression that creates a <see cref="UriAsciiPath"/> from its <see cref="string"/>
    /// storage form.
    /// </summary>
    /// <returns>An expression that produces the path.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<string, UriAsciiPath>> FromStore()
    {
        return value => new(value);
    }
}
