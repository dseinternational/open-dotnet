// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Records;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="Gender"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class GenderToStringConverter : ValueConverter<Gender, string>
{
    public static readonly GenderToStringConverter Default = new();

    public GenderToStringConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertTo(Gender code)
    {
        return code.ToStringInvariant();
    }

    // keep public for EF Core compiled models
    public static Gender ConvertFrom(string code)
    {
        if (Gender.TryParseInvariant(code, out var gender))
        {
            return gender;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(Gender)}", code, null);
        return default; // unreachable
    }
}
