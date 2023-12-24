// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class TokenTextToStringConverter : ValueConverter<TokenText, string>
{
    public static readonly TokenTextToStringConverter Default = new();

    public TokenTextToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static string ConvertTo(TokenText value)
    {
        return value.ToString();
    }

    private static TokenText ConvertFrom(string value)
    {
        return new TokenText(value);
    }
}
