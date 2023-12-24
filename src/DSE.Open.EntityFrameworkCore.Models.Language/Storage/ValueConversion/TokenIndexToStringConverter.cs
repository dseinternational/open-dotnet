// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class TokenIndexToStringConverter : ValueConverter<TokenIndex, string>
{
    public static readonly TokenIndexToStringConverter Default = new();

    public TokenIndexToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static string ConvertTo(TokenIndex value)
    {
        return value.ToString();
    }

    private static TokenIndex ConvertFrom(string value)
    {
        return TokenIndex.ParseInvariant(value);
    }
}
