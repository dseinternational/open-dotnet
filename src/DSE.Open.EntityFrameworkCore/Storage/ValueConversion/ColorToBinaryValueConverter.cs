// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using DSE.Open.Drawing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Color"/> to a <see cref="byte"/> array for storage using its RGBA representation.
/// </summary>
public sealed class ColorToBinaryValueConverter : ValueConverter<Color, byte[]>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly ColorToBinaryValueConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="ColorToBinaryValueConverter"/>.
    /// </summary>
    /// <param name="mappingHints">Optional EF Core mapping hints.</param>
    public ColorToBinaryValueConverter(ConverterMappingHints? mappingHints = null)
        : base(ToBytes(), FromBytes(), mappingHints)
    {
    }

    /// <summary>
    /// Returns an expression that converts a <see cref="Color"/> to its RGBA byte representation.
    /// </summary>
    /// <returns>An expression that produces the byte array.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<Color, byte[]>> ToBytes()
    {
        return value => value.AsRrgbaBytes();
    }

    /// <summary>
    /// Returns an expression that creates a <see cref="Color"/> from its RGBA byte representation.
    /// </summary>
    /// <returns>An expression that produces the color.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<byte[], Color>> FromBytes()
    {
        return value => Color.FromRgba(value);
    }
}
