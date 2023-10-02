// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class IdentifierToStringConverter : ValueConverter<Identifier, string>
{
    public static readonly IdentifierToStringConverter Default = new();

    public IdentifierToStringConverter()
        : base(v => ConvertToString(v), v => ConvertFromString(v))
    {
    }

    public static string ConvertToString(Identifier value) => value.ToString();

    public static Identifier ConvertFromString(string value) => Identifier.Parse(value);
}
