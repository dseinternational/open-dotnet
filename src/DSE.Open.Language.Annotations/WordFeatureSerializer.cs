// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.CompilerServices;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// Serializes and deserializes sequences of <see cref="WordFeature"/> to and
/// from pipe-delimited (<c>|</c>) strings, as used by the CoNLL-U
/// <c>FEATS</c> field.
/// </summary>
public static class WordFeatureSerializer
{
    /// <summary>
    /// Writes <paramref name="features"/> to <paramref name="destination"/>
    /// as a pipe-delimited sequence.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="features"/>
    /// is <see langword="null"/>.</exception>
    public static bool TrySerialize(
        Span<char> destination,
        IEnumerable<WordFeature> features,
        out int charsWritten)
    {
        ArgumentNullException.ThrowIfNull(features);
        return TrySerialize(destination, [.. features], out charsWritten);
    }

    /// <summary>
    /// Writes <paramref name="features"/> to <paramref name="destination"/>
    /// as a pipe-delimited sequence.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="features"/>
    /// is <see langword="null"/>.</exception>
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

            if (!f.TryFormat(destination[charsWritten..], out var cw, default, default))
            {
                charsWritten = 0;
                return false;
            }

            charsWritten += cw;

            if (i < features.Count - 1)
            {
                if (destination.Length > charsWritten)
                {
                    destination[charsWritten++] = '|';
                }
                else
                {
                    charsWritten = 0;
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Serializes <paramref name="features"/> to a pipe-delimited string.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="features"/>
    /// is <see langword="null"/>.</exception>
    /// <exception cref="FormatException">The collection could not be formatted.</exception>
    [SkipLocalsInit]
    public static string SerializeToString(IEnumerable<WordFeature> features)
    {
        ArgumentNullException.ThrowIfNull(features);

        var featureList = features as IReadOnlyList<WordFeature> ?? [.. features];
        var maxLength = featureList.Count == 0
            ? 0
            : featureList.Sum(f => f.GetCharCount()) + featureList.Count - 1;

        char[]? rented = null;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(maxLength)
            ? stackalloc char[maxLength]
            : (rented = ArrayPool<char>.Shared.Rent(maxLength));

        try
        {
            if (TrySerialize(buffer, featureList, out var charsWritten))
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
    /// <param name="values">The characters to parse.</param>
    /// <param name="features">When the method returns <see langword="true"/>,
    /// the parsed values. An empty sequence when <paramref name="values"/>
    /// is empty.</param>
    /// <returns><see langword="true"/> if parsing succeeded; otherwise
    /// <see langword="false"/>.</returns>
    public static bool TryDeserialize(ReadOnlySpan<char> values, out IEnumerable<WordFeature> features)
    {
        if (values.IsEmpty)
        {
            features = [];
            return true;
        }

        var results = new List<WordFeature>();
        var remaining = values;

        while (true)
        {
            var separatorIndex = remaining.IndexOf('|');
            var v = (separatorIndex < 0 ? remaining : remaining[..separatorIndex]).Trim();

            if (!v.IsEmpty)
            {
                if (WordFeature.TryParse(v, CultureInfo.InvariantCulture, out var value))
                {
                    results.Add(value);
                }
                else
                {
                    features = [];
                    return false;
                }
            }

            if (separatorIndex < 0)
            {
                break;
            }

            remaining = remaining[(separatorIndex + 1)..];
        }

        features = results;
        return true;
    }

    /// <summary>
    /// Parses a collection of <see cref="WordFeature"/>s from a pipe-delimited
    /// string. A <see langword="null"/> value is treated as an empty sequence.
    /// </summary>
    public static bool TryDeserialize(string? values, out IEnumerable<WordFeature> features)
    {
        return TryDeserialize(values.AsSpan(), out features);
    }
}
