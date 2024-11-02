// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class BinaryValueToByteArrayConverter : ValueConverter<BinaryValue, byte[]>
{
    public static readonly BinaryValueToByteArrayConverter Default = new();

    public BinaryValueToByteArrayConverter()
        : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    // keep public for EF Core compiled models
    public static byte[] ConvertTo(BinaryValue value)
    {
        return value.ToArray();
    }

    // keep public for EF Core compiled models
    public static BinaryValue ConvertFrom(byte[] value)
    {
        return new(value);
    }
}
