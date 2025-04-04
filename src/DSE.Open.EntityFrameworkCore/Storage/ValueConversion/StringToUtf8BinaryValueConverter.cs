// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class StringToBinaryValueConverter : ValueConverter<string, byte[]>
{
    public static readonly StringToBinaryValueConverter Default = new();

    public StringToBinaryValueConverter(ConverterMappingHints? mappingHints = null)
        : base(ToBytes(), FromBytes(), mappingHints)
    {
    }

    // keep public for EF Core compiled models
    public static Expression<Func<string, byte[]>> ToBytes()
    {
        return value => Encoding.UTF8.GetBytes(value);
    }

    // keep public for EF Core compiled models
    public static Expression<Func<byte[], string>> FromBytes()
    {
        return value => Encoding.UTF8.GetString(value);
    }
}
