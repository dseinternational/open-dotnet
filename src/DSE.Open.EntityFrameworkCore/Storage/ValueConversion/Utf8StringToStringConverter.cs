// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class Utf8StringToStringConverter : ValueConverter<Utf8String, string>
{
    public static readonly Utf8StringToStringConverter Default = new();

    public Utf8StringToStringConverter(ConverterMappingHints? mappingHints = null)
        : base(ToStore(), FromStore(), mappingHints)
    {
    }

    private static Expression<Func<Utf8String, string>> ToStore()
    {
        return value => value.ToString();
    }

    private static Expression<Func<string, Utf8String>> FromStore()
    {
        return value => new(value);
    }
}
