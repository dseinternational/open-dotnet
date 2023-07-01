// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using DSE.Open.Drawing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public class ColorToBinaryValueConverter : ValueConverter<Color, byte[]>
{
    public static readonly ColorToBinaryValueConverter Default = new();

    public ColorToBinaryValueConverter(ConverterMappingHints? mappingHints = null)
        : base(ToBytes(), FromBytes(), mappingHints)
    {
    }

    private static Expression<Func<Color, byte[]>> ToBytes() => value => value.AsArgbBytes();

    private static Expression<Func<byte[], Color>> FromBytes() => value => Color.FromArgb(value);
}
