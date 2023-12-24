// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;

namespace DSE.Open.Drawing;

public class ColorTypeConverter : TypeConverter
{
    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return Color.Parse(value?.ToString()!, culture);
        // Parse will throw on null
    }

    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value is not Color color
            ? ThrowHelper.ThrowInvalidOperationException<object>($"Cannot convert {value?.GetType().Name ?? "null"}")
            : color.ToString();
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
    {
        return new StandardValuesCollection(new[]
        {
            nameof(Colors.AliceBlue),
            nameof(Colors.AntiqueWhite),
            nameof(Colors.Aqua),
            nameof(Colors.Aquamarine),
            nameof(Colors.Azure),
            nameof(Colors.Beige),
            nameof(Colors.Bisque),
            nameof(Colors.Black),
            nameof(Colors.BlanchedAlmond),
            nameof(Colors.Blue),
            nameof(Colors.BlueViolet),
            nameof(Colors.Brown),
            nameof(Colors.BurlyWood),
            nameof(Colors.CadetBlue),
            nameof(Colors.Chartreuse),
            nameof(Colors.Chocolate),
            nameof(Colors.Coral),
            nameof(Colors.CornflowerBlue),
            nameof(Colors.Cornsilk),
            nameof(Colors.Crimson),
            nameof(Colors.Cyan),
            nameof(Colors.DarkBlue),
            nameof(Colors.DarkCyan),
            nameof(Colors.DarkGoldenrod),
            nameof(Colors.DarkGray),
            nameof(Colors.DarkGreen),
            nameof(Colors.DarkGrey),
            nameof(Colors.DarkKhaki),
            nameof(Colors.DarkMagenta),
            nameof(Colors.DarkOliveGreen),
            nameof(Colors.DarkOrange),
            nameof(Colors.DarkOrchid),
            nameof(Colors.DarkRed),
            nameof(Colors.DarkSalmon),
            nameof(Colors.DarkSeaGreen),
            nameof(Colors.DarkSlateBlue),
            nameof(Colors.DarkSlateGray),
            nameof(Colors.DarkSlateGrey),
            nameof(Colors.DarkTurquoise),
            nameof(Colors.DarkViolet),
            nameof(Colors.DeepPink),
            nameof(Colors.DeepSkyBlue),
            nameof(Colors.DimGray),
            nameof(Colors.DimGrey),
            nameof(Colors.DodgerBlue),
            nameof(Colors.Firebrick),
            nameof(Colors.FloralWhite),
            nameof(Colors.ForestGreen),
            nameof(Colors.Fuchsia),
            nameof(Colors.Gainsboro),
            nameof(Colors.GhostWhite),
            nameof(Colors.Gold),
            nameof(Colors.Goldenrod),
            nameof(Colors.Gray),
            nameof(Colors.Green),
            nameof(Colors.GreenYellow),
            nameof(Colors.Grey),
            nameof(Colors.Honeydew),
            nameof(Colors.HotPink),
            nameof(Colors.IndianRed),
            nameof(Colors.Indigo),
            nameof(Colors.Ivory),
            nameof(Colors.Khaki),
            nameof(Colors.Lavender),
            nameof(Colors.LavenderBlush),
            nameof(Colors.LawnGreen),
            nameof(Colors.LemonChiffon),
            nameof(Colors.LightBlue),
            nameof(Colors.LightCoral),
            nameof(Colors.LightCyan),
            nameof(Colors.LightGoldenrodYellow),
            nameof(Colors.LightGray),
            nameof(Colors.LightGreen),
            nameof(Colors.LightGrey),
            nameof(Colors.LightPink),
            nameof(Colors.LightSalmon),
            nameof(Colors.LightSeaGreen),
            nameof(Colors.LightSkyBlue),
            nameof(Colors.LightSlateGray),
            nameof(Colors.LightSlateGrey),
            nameof(Colors.LightSteelBlue),
            nameof(Colors.LightYellow),
            nameof(Colors.Lime),
            nameof(Colors.LimeGreen),
            nameof(Colors.Linen),
            nameof(Colors.Magenta),
            nameof(Colors.Maroon),
            nameof(Colors.MediumAquamarine),
            nameof(Colors.MediumBlue),
            nameof(Colors.MediumOrchid),
            nameof(Colors.MediumPurple),
            nameof(Colors.MediumSeaGreen),
            nameof(Colors.MediumSlateBlue),
            nameof(Colors.MediumSpringGreen),
            nameof(Colors.MediumTurquoise),
            nameof(Colors.MediumVioletRed),
            nameof(Colors.MidnightBlue),
            nameof(Colors.MintCream),
            nameof(Colors.MistyRose),
            nameof(Colors.Moccasin),
            nameof(Colors.NavajoWhite),
            nameof(Colors.Navy),
            nameof(Colors.OldLace),
            nameof(Colors.Olive),
            nameof(Colors.OliveDrab),
            nameof(Colors.Orange),
            nameof(Colors.OrangeRed),
            nameof(Colors.Orchid),
            nameof(Colors.PaleGoldenrod),
            nameof(Colors.PaleGreen),
            nameof(Colors.PaleTurquoise),
            nameof(Colors.PaleVioletRed),
            nameof(Colors.PapayaWhip),
            nameof(Colors.PeachPuff),
            nameof(Colors.Peru),
            nameof(Colors.Pink),
            nameof(Colors.Plum),
            nameof(Colors.PowderBlue),
            nameof(Colors.Purple),
            nameof(Colors.Red),
            nameof(Colors.RosyBrown),
            nameof(Colors.RoyalBlue),
            nameof(Colors.SaddleBrown),
            nameof(Colors.Salmon),
            nameof(Colors.SandyBrown),
            nameof(Colors.SeaGreen),
            nameof(Colors.SeaShell),
            nameof(Colors.Sienna),
            nameof(Colors.Silver),
            nameof(Colors.SkyBlue),
            nameof(Colors.SlateBlue),
            nameof(Colors.SlateGray),
            nameof(Colors.SlateGrey),
            nameof(Colors.Snow),
            nameof(Colors.SpringGreen),
            nameof(Colors.SteelBlue),
            nameof(Colors.Tan),
            nameof(Colors.Teal),
            nameof(Colors.Thistle),
            nameof(Colors.Tomato),
            nameof(Colors.Transparent),
            nameof(Colors.Turquoise),
            nameof(Colors.Violet),
            nameof(Colors.Wheat),
            nameof(Colors.White),
            nameof(Colors.WhiteSmoke),
            nameof(Colors.Yellow),
            nameof(Colors.YellowGreen),
        });
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context)
    {
        return false;
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext? context)
    {
        return true;
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string);
    }
}
