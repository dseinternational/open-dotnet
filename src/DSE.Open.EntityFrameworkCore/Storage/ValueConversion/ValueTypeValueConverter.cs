// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

[SuppressMessage("Design", "CA1005:Avoid excessive parameters on generic types", Justification = "<Pending>")]
public sealed class ValueTypeValueConverter<TValue, T, TStore> : ValueConverter<TValue, TStore>
    where T : IEquatable<T>, IConvertibleTo<T, TStore>, ITryConvertibleFrom<T, TStore>
    where TValue : struct, IValue<TValue, T>
{
    public ValueTypeValueConverter()
        : base(v => ToStore(v), v => FromStore(v), default)
    {
    }

    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TStore ToStore(TValue value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return (TStore)(T)value;
    }

    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TValue FromStore(TStore value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return (TValue)(T)value;
    }
}

public sealed class ValueTypeValueConverter<TValue, T> : ValueConverter<TValue, T>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    public ValueTypeValueConverter()
        : base(v => ToStore(v), v => FromStore(v), default)
    {
    }

    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static T ToStore(TValue value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return (T)value;
    }

    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TValue FromStore(T value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return (TValue)value;
    }
}
