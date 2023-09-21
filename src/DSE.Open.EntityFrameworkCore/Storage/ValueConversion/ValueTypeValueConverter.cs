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
        : base((v) => ToStore(v), v => FromStore(v), default)
    {
    }

    private static TStore ToStore(TValue value) => (TStore)(T)value;

    private static TValue FromStore(TStore value) => (TValue)(T)value;
}

public sealed class ValueTypeValueConverter<TValue, T> : ValueConverter<TValue, T>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    public ValueTypeValueConverter()
        : base((v) => ToStore(v), v => FromStore(v), default)
    {
    }

    private static T ToStore(TValue value) => (T)value;

    private static TValue FromStore(T value) => (TValue)value;
}
