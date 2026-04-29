// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="BinaryValue"/> to a Base64-encoded <see cref="string"/> for storage.
/// </summary>
public sealed class BinaryValueToBase64StringConverter : ValueConverter<BinaryValue, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly BinaryValueToBase64StringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="BinaryValueToBase64StringConverter"/>.
    /// </summary>
    public BinaryValueToBase64StringConverter()
        : base(v => ConvertToString(v), v => ConvertFromString(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="BinaryValue"/> to its Base64-encoded <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The Base64 string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(BinaryValue value)
    {
        return value.ToBase64EncodedString();
    }

    /// <summary>
    /// Converts a Base64-encoded <see cref="string"/> storage value back to a <see cref="BinaryValue"/>.
    /// </summary>
    /// <param name="value">The stored Base64 string value.</param>
    /// <returns>The decoded <see cref="BinaryValue"/>.</returns>
    // keep public for EF Core compiled models
    public static BinaryValue ConvertFromString(string value)
    {
        return BinaryValue.FromBase64(value);
    }
}
