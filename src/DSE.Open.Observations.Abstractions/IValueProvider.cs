// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

/// <summary>
/// Provides the underlying value from an observation value.
/// </summary>
public interface IValueProvider
{
    public const ulong MinCount = 0u;
    public const ulong MaxCount = 9007199254740991u;

    public const decimal MinAmount = 0.0000000000000000m;
    public const decimal MaxAmount = 999999999999.9999999999999999m;

    public const decimal MinRatio = -1.000000000000000000m;
    public const decimal MaxRatio = 1.000000000000000000m;

    public const decimal MinFrequency = 0.0000000000000000m;
    public const decimal MaxFrequency = 100.0000000000000000m;

    /// <summary>
    /// Indicates the type of underlying value provided.
    /// </summary>
    ValueType ValueType { get; }

    /// <summary>
    /// Gets a binary value.
    /// </summary>
    /// <returns>A <see cref="Boolean"/> value.</returns>
    /// <exception cref="ValueTypeMismatchException"><see cref="ValueType"/> is not
    /// <see cref="ValueType.Binary"/>.</exception>
    bool GetBinary();

    /// <summary>
    /// Gets an ordinal value.
    /// </summary>
    /// <returns>
    /// A <see cref="Byte"/> value representing a value on an ordinal scale.
    /// </returns>
    /// <exception cref="ValueTypeMismatchException"><see cref="ValueType"/> is not
    /// <see cref="ValueType.Ordinal"/>.</exception>
    byte GetOrdinal();

    /// <summary>
    /// Gets a count value.
    /// </summary>
    /// <returns>
    /// A <see cref="UInt64"/> value in the inclusive range <see cref="MinCount"/> to
    /// <see cref="MaxCount"/>.
    /// </returns>
    /// <exception cref="ValueTypeMismatchException"><see cref="ValueType"/> is not
    /// <see cref="ValueType.Count"/>.</exception>
    ulong GetCount();

    /// <summary>
    /// Gets an amount value.
    /// </summary>
    /// <returns>
    /// A <see cref="Decimal"/> value in the inclusive range <see cref="MinAmount"/> to
    /// <see cref="MaxAmount"/>.
    /// </returns>
    /// <exception cref="ValueTypeMismatchException"><see cref="ValueType"/> is not
    /// <see cref="ValueType.Amount"/>.</exception>
    decimal GetAmount();

    /// <summary>
    /// Gets a ratio value.
    /// </summary>
    /// <returns>
    /// A <see cref="Decimal"/> value in the inclusive range <see cref="MinRatio"/> to
    /// <see cref="MaxRatio"/>.
    /// </returns>
    /// <exception cref="ValueTypeMismatchException"><see cref="ValueType"/> is not
    /// <see cref="ValueType.Ratio"/>.</exception>
    decimal GetRatio();

    /// <summary>
    /// Gets a frequency value.
    /// </summary>
    /// <returns>
    /// A <see cref="Decimal"/> value in the inclusive range <see cref="MinFrequency"/> to
    /// <see cref="MaxFrequency"/>.
    /// </returns>
    /// <exception cref="ValueTypeMismatchException"><see cref="ValueType"/> is not
    /// <see cref="ValueType.Frequency"/>.</exception>
    decimal GetFrequency();

    [DoesNotReturn]
    public static T ThrowValueMismatchException<T>()
    {
        throw new ValueTypeMismatchException(
            "The value provider does not provide the requested value type.");
    }
}
