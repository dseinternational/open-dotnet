// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class UriAsciiPathToLowerStringConverter : ValueConverter<UriAsciiPath, string>
{
    public static readonly UriAsciiPathToLowerStringConverter Default = new();

    public UriAsciiPathToLowerStringConverter(ConverterMappingHints? mappingHints = null)
        : base(ToStore(), FromStore(), mappingHints)
    {
    }

    private static Expression<Func<UriAsciiPath, string>> ToStore()
    {
        return value => value.ToStringLower();
    }

    private static Expression<Func<string, UriAsciiPath>> FromStore()
    {
        return value => new(value);
    }
}
