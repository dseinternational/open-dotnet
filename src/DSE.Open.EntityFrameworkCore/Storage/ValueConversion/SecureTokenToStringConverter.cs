// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Security;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class SecureTokenToStringConverter : ValueConverter<SecureToken, string>
{
    public static readonly SecureTokenToStringConverter Default = new();

    public SecureTokenToStringConverter()
        : base(v => ConvertToString(v), v => ConvertFromString(v))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertToString(SecureToken value)
    {
        return value.ToString();
    }

    // keep public for EF Core compiled models
    public static SecureToken ConvertFromString(string value)
    {
        return SecureToken.Parse(value, CultureInfo.InvariantCulture);
    }
}
