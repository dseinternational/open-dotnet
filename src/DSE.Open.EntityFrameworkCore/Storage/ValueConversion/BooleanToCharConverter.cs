// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="bool"/> to a <see cref="char"/> for storage, mapping <see langword="true"/>
/// to <c>'Y'</c> and <see langword="false"/> to <c>'N'</c>.
/// </summary>
public sealed class BooleanToCharConverter : ValueConverter<bool, char>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly BooleanToCharConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="BooleanToCharConverter"/>.
    /// </summary>
    public BooleanToCharConverter()
        : base(v => ConvertToChar(v), v => ConvertToBoolean(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="bool"/> to its <see cref="char"/> storage form
    /// (<c>'Y'</c> for <see langword="true"/>, otherwise <c>'N'</c>).
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The corresponding <see cref="char"/>.</returns>
    // keep public for EF Core compiled models
    public static char ConvertToChar(bool value)
    {
        return value ? 'Y' : 'N';
    }

    /// <summary>
    /// Converts a <see cref="char"/> storage value back to a <see cref="bool"/>
    /// (<see langword="true"/> when the char is <c>'Y'</c>).
    /// </summary>
    /// <param name="value">The stored character.</param>
    /// <returns>The decoded boolean.</returns>
    // keep public for EF Core compiled models
    public static bool ConvertToBoolean(char value)
    {
        return value == 'Y';
    }
}
