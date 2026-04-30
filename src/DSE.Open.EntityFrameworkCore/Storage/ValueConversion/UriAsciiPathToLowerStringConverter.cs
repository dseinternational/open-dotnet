// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="UriAsciiPath"/> to a lower-case <see cref="string"/> for storage.
/// </summary>
public sealed class UriAsciiPathToLowerStringConverter : ValueConverter<UriAsciiPath, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly UriAsciiPathToLowerStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="UriAsciiPathToLowerStringConverter"/>.
    /// </summary>
    /// <param name="mappingHints">Optional EF Core mapping hints.</param>
    public UriAsciiPathToLowerStringConverter(ConverterMappingHints? mappingHints = null)
        : base(ToStore(), FromStore(), mappingHints)
    {
    }

    /// <summary>
    /// Returns an expression that converts a <see cref="UriAsciiPath"/> to its lower-case
    /// <see cref="string"/> storage form.
    /// </summary>
    /// <returns>An expression that produces the lower-case string.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<UriAsciiPath, string>> ToStore()
    {
        return value => value.ToStringLower();
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
