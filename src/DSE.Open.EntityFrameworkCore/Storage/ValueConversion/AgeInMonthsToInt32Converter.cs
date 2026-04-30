// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="AgeInMonths"/> to an <see cref="int"/> for storage.
/// </summary>
public sealed class AgeInMonthsToInt32Converter : ValueConverter<AgeInMonths, int>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly AgeInMonthsToInt32Converter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="AgeInMonthsToInt32Converter"/>.
    /// </summary>
    public AgeInMonthsToInt32Converter()
        : base(c => ConvertToInt32(c), s => ConvertFromInt32(s))
    {
    }

    /// <summary>
    /// Converts an <see cref="AgeInMonths"/> to its <see cref="int"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The total months as an <see cref="int"/>.</returns>
    // keep public for EF Core compiled models
    public static int ConvertToInt32(AgeInMonths code)
    {
        return code.TotalMonths;
    }

    /// <summary>
    /// Converts an <see cref="int"/> storage value back to an <see cref="AgeInMonths"/>.
    /// </summary>
    /// <param name="code">The stored months value.</param>
    /// <returns>The reconstructed <see cref="AgeInMonths"/>.</returns>
    // keep public for EF Core compiled models
    public static AgeInMonths ConvertFromInt32(int code)
    {
        return new(code);
    }
}
