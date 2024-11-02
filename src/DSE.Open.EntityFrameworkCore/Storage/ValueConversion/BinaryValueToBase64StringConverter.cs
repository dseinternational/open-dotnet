// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class BinaryValueToBase64StringConverter : ValueConverter<BinaryValue, string>
{
    public static readonly BinaryValueToBase64StringConverter Default = new();

    public BinaryValueToBase64StringConverter()
        : base(v => ConvertToString(v), v => ConvertFromString(v))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertToString(BinaryValue value)
    {
        return value.ToBase64EncodedString();
    }

    // keep public for EF Core compiled models
    public static BinaryValue ConvertFromString(string value)
    {
        return BinaryValue.FromBase64EncodedString(value);
    }
}
