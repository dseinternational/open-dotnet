// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.CompilerServices;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Language.Annotations;

public static class WordFeatureSerializer
{
    public static bool TrySerialize(
        Span<char> destination,
        IEnumerable<WordFeature> features,
        out int charsWritten)
    {
        ArgumentNullException.ThrowIfNull(features);
        return TrySerialize(destination, [.. features], out charsWritten);
    }

    public static bool TrySerialize(
        Span<char> destination,
        IReadOnlyList<WordFeature> features,
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
    public static string SerializeToString(IEnumerable<WordFeature> features)
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
    /// Parses a collection of <see cref="WordFeature"/>s from a pipe-delimited sequence of characters.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="features"></param>
    /// <returns></returns>
    [SkipLocalsInit]
    public static bool TryDeserialize(ReadOnlySpan<char> values, out IEnumerable<WordFeature> features)
    {
        if (values.IsEmpty)
        {
            features = [];
            return true;
        }

        Span<Range> ranges = stackalloc Range[32];

        var l = values.Split(ranges, '|',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var results = new WordFeature[l];

        for (var r = 0; r < l; r++)
        {
            var v = values[ranges[r].Start.Value..ranges[r].End.Value];

            if (WordFeature.TryParse(v, CultureInfo.InvariantCulture, out var value))
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

    public static bool TryDeserialize(string? values, out IEnumerable<WordFeature> features)
    {
        return TryDeserialize(values.AsSpan(), out features);
    }
}
