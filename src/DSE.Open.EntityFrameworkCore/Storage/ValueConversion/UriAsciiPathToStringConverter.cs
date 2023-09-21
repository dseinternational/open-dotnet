// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class UriAsciiPathToStringConverter : ValueConverter<UriAsciiPath, string>
{
    public static readonly UriAsciiPathToStringConverter Default = new();

    public UriAsciiPathToStringConverter(ConverterMappingHints? mappingHints = null)
        : base(ToStore(), FromStore(), mappingHints)
    {
    }

    private static Expression<Func<UriAsciiPath, string>> ToStore() => value => value.ToString();

    private static Expression<Func<string, UriAsciiPath>> FromStore() => value => new(value);
}
