// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class PosTagToStringConverter : ValueConverter<PosTag, string>
{
    public static readonly PosTagToStringConverter Default = new();

    public PosTagToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static string ConvertTo(PosTag value)
    {
        return value.ToString();
    }

    private static PosTag ConvertFrom(string value)
    {
        return new(value);
    }
}
