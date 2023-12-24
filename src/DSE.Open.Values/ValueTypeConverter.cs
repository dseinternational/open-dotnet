// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;

namespace DSE.Open.Values;

public class ValueTypeConverter<TValue, T> : TypeConverter
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(TValue) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is TValue)
        {
            return value;
        }

        return value is T input && TValue.TryFromValue(input, out var result)
            ? result
            : base.ConvertFrom(context, culture, value);
    }
}
