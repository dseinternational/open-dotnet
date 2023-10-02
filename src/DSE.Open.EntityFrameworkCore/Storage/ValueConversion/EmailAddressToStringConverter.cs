// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class EmailAddressToStringConverter : ValueConverter<EmailAddress, string>
{
    public static readonly EmailAddressToStringConverter Default = new();

    public EmailAddressToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    private static string ConvertToString(EmailAddress value) => value.ToString();

    private static EmailAddress ConvertFromString(string value)
    {
        if (EmailAddress.TryParse(value, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Error converting string value '{value}' to EmailAddress.", value, null);
        return default;
    }
}
