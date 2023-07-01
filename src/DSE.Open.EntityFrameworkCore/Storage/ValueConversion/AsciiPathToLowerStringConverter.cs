// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public class AsciiPathToLowerStringConverter : ValueConverter<AsciiPath, string>
{
    public static readonly AsciiPathToLowerStringConverter Default = new();

    public AsciiPathToLowerStringConverter(ConverterMappingHints? mappingHints = null)
        : base(ToStore(), FromStore(), mappingHints)
    {
    }

    private static Expression<Func<AsciiPath, string>> ToStore() => value => value.ToStringLower();

    private static Expression<Func<string, AsciiPath>> FromStore() => value => new(value);
}
