// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.CompilerServices;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Language.Annotations;

public static class AttributeValueSerializer
{
    public static bool TrySerialize(
        Span<char> destination,
        IEnumerable<AttributeValue> features,
        out int charsWritten)
    {
        ArgumentNullException.ThrowIfNull(features);
        return TrySerialize(destination, [.. features], out charsWritten);
    }

    public static bool TrySerialize(
        Span<char> destination,
        IReadOnlyList<AttributeValue> features,
        out int charsWritten)
    {
        ArgumentNullException.ThrowIfNull(features);

        charsWritten = 0;

        for (var i = 0; i < features.Count; i++)
        {
            var f = features[i];

            if (f.TryFormat(destination[charsWritten..], out var cw, default, default))
            {
                charsWritten += cw;

                if (i < features.Count - 1)
                {
                    if (destination.Length > charsWritten)
                    {
                        destination[charsWritten++] = '|';
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    [SkipLocalsInit]
    public static string SerializeToString(IEnumerable<AttributeValue> features)
    {
        ArgumentNullException.ThrowIfNull(features);

        var maxLength = features.Count() * 16;

        char[]? rented = null;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(maxLength)
            ? stackalloc char[maxLength]
            : (rented = ArrayPool<char>.Shared.Rent(maxLength));

        try
        {
            if (TrySerialize(buffer, features, out var charsWritten))
            {
                return buffer[..charsWritten].ToString();
            }

            return ThrowHelper.ThrowFormatException<string>(
                "Could not format features collection to string.");
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    /// <summary>
    /// Parses a collection of <see cref="AttributeValue"/>s from a pipe-delimited sequence of characters.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="features"></param>
    /// <returns></returns>
    public static bool TryDeserialize(ReadOnlySpan<char> values, out IEnumerable<AttributeValue> features)
    {
        if (values.IsEmpty)
        {
            features = [];
            return true;
        }

        Span<Range> ranges = stackalloc Range[32];

        var l = values.Split(ranges, '|',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var results = new AttributeValue[l];

        for (var r = 0; r < l; r++)
        {
            var v = values[ranges[r].Start.Value..ranges[r].End.Value];

            if (AttributeValue.TryParse(v, CultureInfo.InvariantCulture, out var value))
            {
                results[r] = value;
            }
            else
            {
                features = [];
                return false;
            }
        }

        features = results;
        return true;
    }

    public static bool TryDeserialize(string? values, out IEnumerable<AttributeValue> features)
    {
        return TryDeserialize(values.AsSpan(), out features);
    }
}
