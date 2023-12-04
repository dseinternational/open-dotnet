﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;

namespace DSE.Open.Language.Annotations;

public static class WordAttributeSerializer
{
    public static bool TrySerialize(
        Span<char> destination,
        IEnumerable<WordAttribute> features,
        out int charsWritten)
    {
        Guard.IsNotNull(features);
        return TrySerialize(destination, features.ToArray(), out charsWritten);
    }

    public static bool TrySerialize(
        Span<char> destination,
        IReadOnlyList<WordAttribute> features,
        out int charsWritten)
    {
        Guard.IsNotNull(features);

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

    public static string SerializeToString(IEnumerable<WordAttribute> features)
    {
        Guard.IsNotNull(features);

        var maxLength = features.Count() * 16;

        char[]? rented = null;

        Span<char> buffer = maxLength > 128
            ? (rented = ArrayPool<char>.Shared.Rent(maxLength))
            : stackalloc char[maxLength];

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
    /// Parses a collection of <see cref="WordAttribute"/>s from a pipe-delimited sequence of characters.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="features"></param>
    /// <returns></returns>
    public static bool TryDeserialize(ReadOnlySpan<char> values, out IEnumerable<WordAttribute> features)
    {
        if (values.IsEmpty)
        {
            features = Enumerable.Empty<WordAttribute>();
            return true;
        }

        Span<Range> ranges = stackalloc Range[32];

        var l = values.Split(ranges, '|',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var results = new WordAttribute[l];

        for (var r = 0; r < l; r++)
        {
            var v = values[ranges[r].Start.Value..ranges[r].End.Value];

            if (WordAttribute.TryParse(v, CultureInfo.InvariantCulture, out var value))
            {
                results[r] = value;
            }
            else
            {
                features = Enumerable.Empty<WordAttribute>();
                return false;
            }
        }

        features = results;
        return true;
    }

    public static bool TryDeserialize(string? values, out IEnumerable<WordAttribute> features)
    {
        return TryDeserialize(values.AsSpan(), out features);
    }
}
