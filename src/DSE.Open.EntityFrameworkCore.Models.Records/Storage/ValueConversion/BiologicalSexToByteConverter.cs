// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Records;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="BiologicalSex"/> values to and from <see cref="byte"/>.
/// </summary>
public sealed class BiologicalSexToByteConverter : ValueConverter<BiologicalSex, byte>
{
    public static readonly BiologicalSexToByteConverter Default = new();

    /// <summary>The byte value used to represent <see cref="BiologicalSex.Female"/>.</summary>
    public const byte Female = 1;

    /// <summary>The byte value used to represent <see cref="BiologicalSex.Male"/>.</summary>
    public const byte Male = 2;

    public BiologicalSexToByteConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    // keep public for EF Core compiled models
    public static byte ConvertTo(BiologicalSex code)
    {
        if (code == BiologicalSex.Female)
        {
            return Female;
        }

        if (code == BiologicalSex.Male)
        {
            return Male;
        }

        ValueConversionException.Throw($"Could not convert BiologicalSex '{code}' to {nameof(Byte)}", code, null);
        return default; // unreachable
    }

    // keep public for EF Core compiled models
    public static BiologicalSex ConvertFrom(byte code)
    {
        if (code == Female)
        {
            return BiologicalSex.Female;
        }

        if (code == Male)
        {
            return BiologicalSex.Male;
        }

        ValueConversionException.Throw($"Could not convert byte '{code}' to {nameof(BiologicalSex)}", code, null);
        return default; // unreachable
    }
}
