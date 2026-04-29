// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Drawing;

/// <summary>
/// A point in 2D space represented by <see cref="double"/> X and Y coordinates.
/// </summary>
public readonly record struct Point(double X, double Y)
{
    /// <summary>
    /// Parses a comma-separated <c>X,Y</c> string into a <see cref="Point"/>.
    /// </summary>
    /// <exception cref="FormatException">The value is not in a valid format.</exception>
    public static Point Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (TryParse(value, out var point))
        {
            return point;
        }

        throw new FormatException($"Cannot parse '{value}' to Point");
    }

    /// <summary>
    /// Attempts to parse a comma-separated <c>X,Y</c> string into a <see cref="Point"/>.
    /// </summary>
    public static bool TryParse([NotNullWhen(true)] string? value, out Point point)
    {
        return TryParse(value, null, out point);
    }

    /// <summary>
    /// Attempts to parse a comma-separated <c>X,Y</c> string into a <see cref="Point"/>.
    /// </summary>
    public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider, out Point point)
    {
        if (value is null)
        {
            point = default;
            return false;
        }

        return TryParse(value.AsSpan(), out point);
    }

    /// <summary>
    /// Attempts to parse a comma-separated <c>X,Y</c> span into a <see cref="Point"/>.
    /// </summary>
    public static bool TryParse(ReadOnlySpan<char> value, out Point point)
    {
        return TryParse(value, null, out point);
    }

    /// <summary>
    /// Attempts to parse a comma-separated <c>X,Y</c> span into a <see cref="Point"/>.
    /// </summary>
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

        point = new(x, y);
        return true;

    Fail:
        point = default;
        return false;
    }

    /// <summary>
    /// Attempts to parse a whitespace-separated sequence of comma-separated <c>X,Y</c> values
    /// into a list of <see cref="Point"/> values.
    /// </summary>
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

    /// <summary>
    /// Attempts to parse each element of <paramref name="values"/> as a comma-separated
    /// <c>X,Y</c> value into a list of <see cref="Point"/> values.
    /// </summary>
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
