// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Globalization;

namespace DSE.Open.Drawing;

public class PointTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string);
    }

    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (Point.TryParse(value?.ToString(), out var p))
        {
            return p;
        }

        throw new InvalidOperationException($"Cannot convert \"{value}\" into {nameof(Point)}");
    }

    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is not Point p)
        {
            throw new NotSupportedException();
        }

        return $"{p.X.ToStringInvariant()},{p.Y.ToStringInvariant()}";
    }
}

