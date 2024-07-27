// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class NumberConverter<TFrom, TTo> : ValueConverter<TFrom, TTo>
    where TFrom : INumberBase<TFrom>
    where TTo : INumberBase<TTo>
{
    public static readonly NumberConverter<TFrom, TTo> Default = new();

    public NumberConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static TTo ConvertTo(TFrom value)
    {
        return TTo.CreateChecked(value);
    }

    private static TFrom ConvertFrom(TTo value)
    {
        return TFrom.CreateChecked(value);
    }
}
