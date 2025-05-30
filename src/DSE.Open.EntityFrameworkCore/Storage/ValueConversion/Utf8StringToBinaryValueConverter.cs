// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class Utf8StringToBinaryValueConverter : ValueConverter<Utf8String, byte[]>
{
    public static readonly Utf8StringToBinaryValueConverter Default = new();

    public Utf8StringToBinaryValueConverter(ConverterMappingHints? mappingHints = null)
        : base(ToBytes(), FromBytes(), mappingHints)
    {
    }

    // keep public for EF Core compiled models
    public static Expression<Func<Utf8String, byte[]>> ToBytes()
    {
        return value => value.ToByteArray();
    }

    // keep public for EF Core compiled models
    public static Expression<Func<byte[], Utf8String>> FromBytes()
    {
        return value => new(value);
    }
}
