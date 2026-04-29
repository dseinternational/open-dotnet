// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Utf8String"/> to a <see cref="byte"/> array for storage.
/// </summary>
public sealed class Utf8StringToBinaryValueConverter : ValueConverter<Utf8String, byte[]>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly Utf8StringToBinaryValueConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="Utf8StringToBinaryValueConverter"/>.
    /// </summary>
    /// <param name="mappingHints">Optional EF Core mapping hints.</param>
    public Utf8StringToBinaryValueConverter(ConverterMappingHints? mappingHints = null)
        : base(ToBytes(), FromBytes(), mappingHints)
    {
    }

    /// <summary>
    /// Returns an expression that converts a <see cref="Utf8String"/> to its byte array storage form.
    /// </summary>
    /// <returns>An expression that produces the byte array.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<Utf8String, byte[]>> ToBytes()
    {
        return value => value.ToByteArray();
    }

    /// <summary>
    /// Returns an expression that creates a <see cref="Utf8String"/> from a byte array storage value.
    /// </summary>
    /// <returns>An expression that produces the <see cref="Utf8String"/>.</returns>
    // keep public for EF Core compiled models
    public static Expression<Func<byte[], Utf8String>> FromBytes()
    {
        return value => new(value);
    }
}
