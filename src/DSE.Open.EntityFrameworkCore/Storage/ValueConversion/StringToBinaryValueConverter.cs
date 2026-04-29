// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="string"/> to a <see cref="byte"/> array for storage using UTF-8 encoding.
/// </summary>
public sealed class StringToBinaryValueConverter : ValueConverter<string, byte[]>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly StringToBinaryValueConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="StringToBinaryValueConverter"/>.
    /// </summary>
    /// <param name="mappingHints">Optional EF Core mapping hints.</param>
    public StringToBinaryValueConverter(ConverterMappingHints? mappingHints = null)
        : base(ToBytes(), FromBytes(), mappingHints)
    {
    }

    /// <summary>
    /// Returns an expression that UTF-8 encodes a <see cref="string"/> to a byte array.
    /// </summary>
    /// <returns>An expression that produces the byte array.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<string, byte[]>> ToBytes()
    {
        return value => Encoding.UTF8.GetBytes(value);
    }

    /// <summary>
    /// Returns an expression that UTF-8 decodes a byte array back to a <see cref="string"/>.
    /// </summary>
    /// <returns>An expression that produces the string.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<byte[], string>> FromBytes()
    {
        return value => Encoding.UTF8.GetString(value);
    }
}
