// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="AgeInMonths"/> to a <see cref="short"/> for storage.
/// </summary>
public sealed class AgeInMonthsToInt16Converter : ValueConverter<AgeInMonths, short>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly AgeInMonthsToInt16Converter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="AgeInMonthsToInt16Converter"/>.
    /// </summary>
    public AgeInMonthsToInt16Converter()
        : base(c => ConvertToInt16(c), s => ConvertFromInt16(s))
    {
    }

    /// <summary>
    /// Converts an <see cref="AgeInMonths"/> to its <see cref="short"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The total months as a <see cref="short"/>.</returns>
    // keep public for EF Core compiled models
    public static short ConvertToInt16(AgeInMonths code)
    {
        return (short)code.TotalMonths;
    }

    /// <summary>
    /// Converts a <see cref="short"/> storage value back to an <see cref="AgeInMonths"/>.
    /// </summary>
    /// <param name="code">The stored months value.</param>
    /// <returns>The reconstructed <see cref="AgeInMonths"/>.</returns>
    // keep public for EF Core compiled models
    public static AgeInMonths ConvertFromInt16(short code)
    {
        return new(code);
    }
}
