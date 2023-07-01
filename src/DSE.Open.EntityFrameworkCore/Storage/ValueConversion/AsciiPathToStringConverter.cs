// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public class AsciiPathToStringConverter : ValueConverter<AsciiPath, string>
{
    public static readonly AsciiPathToStringConverter Default = new();

    public AsciiPathToStringConverter(ConverterMappingHints? mappingHints = null)
        : base(ToStore(), FromStore(), mappingHints)
    {
    }

    private static Expression<Func<AsciiPath, string>> ToStore() => value => value.ToString();

    private static Expression<Func<string, AsciiPath>> FromStore() => value => new(value);
}
