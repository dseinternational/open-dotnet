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

    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TTo ConvertTo(TFrom value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return TTo.CreateChecked(value);
    }

    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TFrom ConvertFrom(TTo value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return TFrom.CreateChecked(value);
    }
}
