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

    private static string ConvertToString(BinaryValue value) => value.ToBase64EncodedString();

    private static BinaryValue ConvertFromString(string value) => BinaryValue.FromBase64EncodedString(value);
}
