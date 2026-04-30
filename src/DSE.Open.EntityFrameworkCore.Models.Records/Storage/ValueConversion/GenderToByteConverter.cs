// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Records;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="Gender"/> values to and from <see cref="byte"/>.
/// </summary>
public sealed class GenderToByteConverter : ValueConverter<Gender, byte>
{
    /// <summary>
    /// The default shared instance of the converter.
    /// </summary>
    public static readonly GenderToByteConverter Default = new();

    /// <summary>The byte value used to represent an unknown gender.</summary>
    public const byte Unknown = 0;

    /// <summary>The byte value used to represent <see cref="Gender.Female"/>.</summary>
    public const byte Female = 1;

    /// <summary>The byte value used to represent <see cref="Gender.Male"/>.</summary>
    public const byte Male = 2;

    /// <summary>The byte value used to represent <see cref="Gender.Other"/>.</summary>
    public const byte Other = 3;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenderToByteConverter"/> class.
    /// </summary>
    public GenderToByteConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="Gender"/> value to its <see cref="byte"/> representation.
    /// </summary>
    /// <param name="code">The <see cref="Gender"/> value to convert.</param>
    /// <returns>The <see cref="byte"/> value representing <paramref name="code"/>.</returns>
    // keep public for EF Core compiled models
    public static byte ConvertTo(Gender code)
    {
        if (code == Gender.Female)
        {
            return Female;
        }

        if (code == Gender.Male)
        {
            return Male;
        }

        if (code == Gender.Other)
        {
            return Other;
        }

        ValueConversionException.Throw($"Could not convert gender '{code}' to {nameof(Byte)}", code, null);
        return default; // unreachable
    }

    /// <summary>
    /// Converts a <see cref="byte"/> value to its <see cref="Gender"/> representation.
    /// </summary>
    /// <param name="code">The <see cref="byte"/> value to convert.</param>
    /// <returns>The <see cref="Gender"/> value represented by <paramref name="code"/>.</returns>
    // keep public for EF Core compiled models
    public static Gender ConvertFrom(byte code)
    {
        return code switch
        {
            Unknown => Gender.Other,
            Female => Gender.Female,
            Male => Gender.Male,
            Other => Gender.Other,
            _ => ThrowConvertFrom(code),
        };
    }

    private static Gender ThrowConvertFrom(byte code)
    {
        ValueConversionException.Throw($"Could not convert byte '{code}' to {nameof(Gender)}", code, null);
        return default; // unreachable
    }
}
