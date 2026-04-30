// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Timestamp"/> to a <see cref="byte"/> array for storage.
/// </summary>
public sealed class TimestampToByteArrayConverter : ValueConverter<Timestamp, byte[]>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimestampToByteArrayConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimestampToByteArrayConverter"/>.
    /// </summary>
    public TimestampToByteArrayConverter()
        : base(v => ConvertToByteArray(v), v => ConvertFromByteArray(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="Timestamp"/> to its <see cref="byte"/> array storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The byte array representation.</returns>
    // keep public for EF Core compiled models
    public static byte[] ConvertToByteArray(Timestamp value)
    {
        return value.GetBytes();
    }

    /// <summary>
    /// Converts a <see cref="byte"/> array storage value back to a <see cref="Timestamp"/>.
    /// </summary>
    /// <param name="value">The stored byte array.</param>
    /// <returns>The reconstructed <see cref="Timestamp"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="value"/> cannot be converted.</exception>
    // keep public for EF Core compiled models
    public static Timestamp ConvertFromByteArray(byte[] value)
    {
        if (Timestamp.TryCreate(value, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert byte array '{value}' to {nameof(Timestamp)}");
        return default; // unreachable
    }
}
