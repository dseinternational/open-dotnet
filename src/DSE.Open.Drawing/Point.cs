// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Drawing;

public readonly record struct Point(double X, double Y)
{
    public static Point Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (TryParse(value, out var point))
        {
            return point;
        }

        throw new FormatException($"Cannot parse '{value}' to Point");
    }

    public static bool TryParse([NotNullWhen(true)] string? value, out Point point)
        => TryParse(value, null, out point);

    public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider, out Point point)
    {
        if (value is null)
        {
            point = default;
            return false;
        }

        return TryParse(value.AsSpan(), out point);
    }

    public static bool TryParse(ReadOnlySpan<char> value, out Point point) => TryParse(value, null, out point);

    public static bool TryParse(ReadOnlySpan<char> value, IFormatProvider? provider, out Point point)
    {
        var i0 = value.IndexOf(',');

        if (i0 < 1)
        {
            goto Fail;
        }

        var p0 = value[..i0];

        var p1 = value[(i0 + 1)..];

        if (!double.TryParse(p0, NumberStyles.Float, CultureInfo.InvariantCulture, out var x)
            || !double.TryParse(p1, NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
        {
            goto Fail;
        }

        point = new Point(x, y);
        return true;

    Fail:
        point = default;
        return false;
    }

    public static bool TryParseCollection(string? spaceSeparatedValues, out IList<Point> points)
    {
        if (string.IsNullOrWhiteSpace(spaceSeparatedValues))
        {
            points = Array.Empty<Point>();
            return true;
        }

        var collection = spaceSeparatedValues.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        if (collection.Length == 0)
        {
            points = Array.Empty<Point>();
            return true;
        }

        return TryParseCollection(collection, out points);
    }

    public static bool TryParseCollection(IEnumerable<string> values, out IList<Point> points)
    {
        ArgumentNullException.ThrowIfNull(values);

        var result = new List<Point>();

        foreach (var p in values)
        {
            if (TryParse(p, out var point))
            {
                result.Add(point);
            }
            else
            {
                points = Array.Empty<Point>();
                return false;
            }
        }

        points = result;
        return true;
    }
}
