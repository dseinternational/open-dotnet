// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="BinaryValue"/> to a <see cref="byte"/> array for storage.
/// </summary>
public sealed class BinaryValueToByteArrayConverter : ValueConverter<BinaryValue, byte[]>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly BinaryValueToByteArrayConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="BinaryValueToByteArrayConverter"/>.
    /// </summary>
    public BinaryValueToByteArrayConverter()
        : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="BinaryValue"/> to its <see cref="byte"/> array storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The byte array.</returns>
    // keep public for EF Core compiled models
    public static byte[] ConvertTo(BinaryValue value)
    {
        return value.ToArray();
    }

    /// <summary>
    /// Converts a <see cref="byte"/> array storage value back to a <see cref="BinaryValue"/>.
    /// </summary>
    /// <param name="value">The stored byte array.</param>
    /// <returns>The reconstructed <see cref="BinaryValue"/>.</returns>
    // keep public for EF Core compiled models
    public static BinaryValue ConvertFrom(byte[] value)
    {
        return new(value);
    }
}
