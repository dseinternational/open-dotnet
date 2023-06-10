// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Values;
using EfValueConversion = Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

[SuppressMessage("Design", "CA1005:Avoid excessive parameters on generic types", Justification = "<Pending>")]
public class ValueConverter<TValue, T, TStore> : EfValueConversion.ValueConverter<TValue, TStore>
    where T : IEquatable<T>, IConvertibleTo<T, TStore>, ITryConvertibleFrom<T, TStore>
    where TValue : struct, IValue<TValue, T>
{
    public ValueConverter()
        : base((v) => ConvertToStoreType(v), v => ConvertFromStoreType(v), default)
    {
    }

    private static TStore ConvertToStoreType(TValue value) => (TStore)(T)value;

    private static TValue ConvertFromStoreType(TStore value) => (TValue)(T)value;
}

public class ValueConverter<TValue, T> : EfValueConversion.ValueConverter<TValue, T>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    public ValueConverter()
        : base((v) => ConvertToStoreType(v), v => ConvertFromStoreType(v), default)
    {
    }

    private static T ConvertToStoreType(TValue value) => (T)value;

    private static TValue ConvertFromStoreType(T value) => (TValue)value;
}
