// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using DSE.Open.Drawing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class ColorToBinaryValueConverter : ValueConverter<Color, byte[]>
{
    public static readonly ColorToBinaryValueConverter Default = new();

    public ColorToBinaryValueConverter(ConverterMappingHints? mappingHints = null)
        : base(ToBytes(), FromBytes(), mappingHints)
    {
    }

    // keep public for EF Core compiled models
    public static Expression<Func<Color, byte[]>> ToBytes()
    {
        return value => value.AsRrgbaBytes();
    }

    // keep public for EF Core compiled models
    public static Expression<Func<byte[], Color>> FromBytes()
    {
        return value => Color.FromRgba(value);
    }
}
