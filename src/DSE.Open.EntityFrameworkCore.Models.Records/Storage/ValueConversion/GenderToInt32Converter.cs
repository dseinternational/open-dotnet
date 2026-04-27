// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Records;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;

// Note: https://github.com/dotnet/efcore/issues/13850

/// <summary>
/// EF Core value converter that maps <see cref="Gender"/> values to and from <see cref="int"/>.
/// </summary>
public sealed class GenderToInt32Converter : ValueConverter<Gender, int>
{
    public static readonly GenderToInt32Converter Default = new();

    public GenderToInt32Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    // keep public for EF Core compiled models
    public static int ConvertTo(Gender code)
    {
        return GenderToByteConverter.ConvertTo(code);
    }

    // keep public for EF Core compiled models
    public static Gender ConvertFrom(int code)
    {
        return GenderToByteConverter.ConvertFrom((byte)code);
    }
}
