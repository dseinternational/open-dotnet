// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class LabelToStringConverter : ValueConverter<Label, string>
{
    public static readonly LabelToStringConverter Default = new();

    public LabelToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static string ConvertTo(Label value)
    {
        return value.ToString();
    }

    private static Label ConvertFrom(string value)
    {
        return new(value);
    }
}
