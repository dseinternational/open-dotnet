// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace DSE.Open.Text;

public static partial class StringHelper
{
    /// <summary>
    /// Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type <see cref="string"/>,
    /// using the specified <paramref name="separator"/> between each member before the penultimate, and the
    /// specified <paramref name="finalSeparator"/> between the penultimate and final members.
    /// </summary>
    /// <param name="separator">The string to use as a separator. It is included in the returned string only if
    ///     <paramref name="values"/> has more than one element.</param>
    /// <param name="finalSeparator">The string to use as the final separator. It is included in the returned string
    ///     only if <paramref name="values"/> has more than one element.</param>
    /// <param name="values">A collection that contains the strings to concatenate.</param>
    /// <returns>A string that consists of the elements of <paramref name="values"/> delimited by the
    /// <paramref name="separator"/> string between each member before the penultimate, and by the
    /// <paramref name="finalSeparator"/> between the penultimate and final members; or, <see cref="string.Empty"/>
    /// if <paramref name="values"/> has zero elements.</returns>
    public static string Join(string? separator, string? finalSeparator, IEnumerable<string?> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (string.IsNullOrEmpty(finalSeparator))
        {
            // A `null` or `string.Empty` final separator is treated as asking for
            // `string.Join(separator, values)` behaviour
            return string.Join(separator, values);
        }

        if (values is List<string?> valuesList)
        {
            return JoinSpan(separator.AsSpan(), CollectionsMarshal.AsSpan(valuesList), finalSeparator.AsSpan());
        }

        if (values is string?[] valuesArray)
        {
            return JoinSpan(separator.AsSpan(), new ReadOnlySpan<string?>(valuesArray), finalSeparator.AsSpan());
        }

        if (values is ICollection<string?> collection)
        {
            return JoinCollection(separator, collection, finalSeparator);
        }

        return JoinEnumerable(separator, values, finalSeparator);
    }

    private static string JoinEnumerable(
        string? separator,
        IEnumerable<string?> values,
        string finalSeparator)
    {
        var sb = new StringBuilder(64);

        using (var en = values.GetEnumerator())
        {
            if (!en.MoveNext())
            {
                return string.Empty;
            }

            var firstValue = en.Current;

            if (!en.MoveNext())
            {
                // Only one value available
                return firstValue ?? string.Empty;
            }

            _ = sb.Append(firstValue);

            var isLast = false;

            while (!isLast)
            {
                var current = en.Current;

                isLast = !en.MoveNext();

                _ = sb.Append(isLast ? finalSeparator : separator);

                _ = sb.Append(current);
            }
        }

        return sb.ToString();
    }

    private static string JoinCollection(string? separator, ICollection<string?> collection, string finalSeparator)
    {
        switch (collection.Count)
        {
            case 0:
                return string.Empty;
            case 1:
                return collection.FirstOrDefault() ?? string.Empty;
        }

        var charCount = collection.Sum(s => s is not null ? (long)s.Length : 0) +
            ((collection.Count - 2) * separator?.Length ?? 0) +
            finalSeparator.Length;

        if (charCount > int.MaxValue)
        {
            ThrowHelper.ThrowInvalidOperationException("Resulting string would be > Int32.MaxValue characters.");
        }

        var length = (int)charCount;

        if (length == 0)
        {
            return string.Empty;
        }

        return string.Create(length,
            (collection, separator, finalSeparator),
            (chars, state) =>
            {
                var written = Format(chars,
                    state.separator,
                    state.collection,
                    state.finalSeparator,
                    state.collection.Count);
                Debug.Assert(written == chars.Length);
            });
    }

    private static int Format(
        Span<char> chars,
        ReadOnlySpan<char> separator,
        ICollection<string?> collection,
        ReadOnlySpan<char> finalSeparator)
    {
        return Format(chars, separator, collection, finalSeparator, collection.Count);
    }

    /// <returns>Number of characters written to the span</returns>
    private static int Format(
        Span<char> chars,
        ReadOnlySpan<char> separator,
        IEnumerable<string?> collection,
        ReadOnlySpan<char> finalSeparator,
        int count)
    {
        var charsWritten = 0;
        var wordIndex = 0;

        foreach (var str in collection)
        {
            var strSpan = str.AsSpan();

            strSpan.CopyTo(chars[charsWritten..]);
            charsWritten += strSpan.Length;
            wordIndex++;

            if (wordIndex < count - 1)
            {
                separator!.CopyTo(chars[charsWritten..]);
                charsWritten += separator.Length;
            }
            else if (wordIndex == count - 1)
            {
                finalSeparator.CopyTo(chars[charsWritten..]);
                charsWritten += finalSeparator.Length;
            }
        }

        return charsWritten;
    }

    /// <summary>
    /// Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type <see cref="string"/>,
    /// using the specified <paramref name="separator"/> between each member before the penultimate, and the
    /// specified <paramref name="finalSeparator"/> between the penultimate and final members.
    /// </summary>
    /// <param name="separator">The string to use as a separator. It is included in the returned string only if
    ///     <paramref name="values"/> has more than one element.</param>
    /// <param name="finalSeparator">The string to use as the final separator. It is included in the returned string
    ///     only if <paramref name="values"/> has more than one element.</param>
    /// <param name="values">A collection that contains the strings to concatenate.</param>
    /// <returns>A string that consists of the elements of <paramref name="values"/> delimited by the
    /// <paramref name="separator"/> string between each member before the penultimate, and by the
    /// <paramref name="finalSeparator"/> between the penultimate and final members; or, <see cref="string.Empty"/>
    /// if <paramref name="values"/> has zero elements.</returns>
    public static string Join(
        ReadOnlySpan<char> separator,
        ReadOnlySpan<char> finalSeparator,
        IEnumerable<string?> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (finalSeparator.IsEmpty)
        {
            // `Empty` final separator is treated as asking for `string.Join(separator, values)` behaviour
            finalSeparator = separator;
        }

        if (values is List<string?> valuesList)
        {
            return JoinSpan(separator, CollectionsMarshal.AsSpan(valuesList), finalSeparator);
        }

        if (values is string?[] valuesArray)
        {
            return JoinSpan(separator, new ReadOnlySpan<string?>(valuesArray), finalSeparator);
        }

        if (values is ICollection<string?> collection)
        {
            var count = collection.Count;

            if (count == 0)
            {
                return string.Empty;
            }

            var charCount = collection.Sum(s => s is not null ? (long)s.Length : 0) +
                ((count - 2) * separator.Length) +
                finalSeparator.Length;

            if (charCount > int.MaxValue)
            {
                ThrowHelper.ThrowInvalidOperationException(
                    "Resulting string would be > Int32.MaxValue characters.");
            }

            return charCount switch
            {
                0 => string.Empty,
                // Cannot use string.Create here because ReadOnlySpan<char> values cannot be
                // passed to lambdas/anonymous functions
                _ => JoinPreAllocated(separator, collection, finalSeparator, (int)charCount)
            };
        }

        var sb = new StringBuilder(64);

        using (var en = values.GetEnumerator())
        {
            if (!en.MoveNext())
            {
                return string.Empty;
            }

            var firstValue = en.Current;

            if (!en.MoveNext())
            {
                // Only one value available
                return firstValue ?? string.Empty;
            }

            _ = sb.Append(firstValue);

            var isLast = false;

            while (!isLast)
            {
                var current = en.Current;

                isLast = !en.MoveNext();

                _ = isLast ? sb.Append(finalSeparator) : sb.Append(separator);

                _ = sb.Append(current);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Concatenates the members of a collection, using the specified <paramref name="separator"/> between each member
    /// before the penultimate, and the specified <paramref name="finalSeparator"/> between the penultimate and final
    /// members, optionally applying a specified <paramref name="format"/> to each member using a specified
    /// <paramref name="provider"/>.
    /// members.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="separator">The string to use as a separator. It is included in the returned string only if
    ///     <paramref name="values"/> has more than one element.</param>
    /// <param name="finalSeparator">The string to use as the final separator. It is included in the returned string
    ///     only if <paramref name="values"/> has more than one element.</param>
    /// <param name="values">A collection that contains the strings to concatenate.</param>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static string Join<T>(
        ReadOnlySpan<char> separator,
        ReadOnlySpan<char> finalSeparator,
        IEnumerable<T> values,
        string? format = default,
        IFormatProvider? provider = default)
    {
        ArgumentNullException.ThrowIfNull(values);

        finalSeparator = finalSeparator.IsEmpty
            ? separator
            : finalSeparator;

        if (typeof(T) == typeof(string))
        {
            if (values is List<string?> valuesList)
            {
                return JoinSpan(separator, CollectionsMarshal.AsSpan(valuesList), finalSeparator);
            }

            if (values is string?[] valuesArray)
            {
                return JoinSpan(separator, new ReadOnlySpan<string?>(valuesArray), finalSeparator);
            }

            if (values is IEnumerable<string> valuesEnumerable)
            {
                return Join(separator, finalSeparator, valuesEnumerable);
            }
        }

        using (var e = values.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                return string.Empty;
            }

            // TODO: confirm if this adds anything for chars

            if (typeof(T) == typeof(char))
            {
                var en = Unsafe.As<IEnumerator<char>>(e);

                var firstValue = en.Current;

                if (!en.MoveNext())
                {
                    return firstValue.ToString();
                }

                var sb = new StringBuilder(64);

                _ = sb.Append(firstValue);

                var isLast = false;

                while (!isLast)
                {
                    var current = en.Current;

                    isLast = !en.MoveNext();

                    _ = isLast ? sb.Append(finalSeparator) : sb.Append(separator);

                    _ = sb.Append(current);
                }

                return sb.ToString();
            }
            else
            {
                var firstValue = e.Current;

                if (!e.MoveNext())
                {
                    return $"{firstValue}";
                }

                var sh = new DefaultInterpolatedStringHandler(0, 0, provider, stackalloc char[256]);

                sh.AppendFormatted(firstValue, format);

                var isLast = false;

                while (!isLast)
                {
                    var current = e.Current;

                    isLast = !e.MoveNext();

                    sh.AppendFormatted(isLast ? finalSeparator : separator);

                    sh.AppendFormatted(current, format);
                }

                return sh.ToStringAndClear();
            }
        }
    }

    private static string JoinSpan(
        ReadOnlySpan<char> separator,
        ReadOnlySpan<string?> values,
        ReadOnlySpan<char> finalSeparator)
    {
        switch (values.Length)
        {
            case 0:
                return string.Empty;
            case 1:
                return values[0] ?? string.Empty;
        }

        var totalSeparatorsLength = ((long)(values.Length - 2) * separator.Length) + finalSeparator.Length;

        if (totalSeparatorsLength > int.MaxValue)
        {
            ThrowHelper.ThrowInvalidOperationException(
                "Resulting string would be > Int32.MaxValue characters.");
        }

        var totalLength = totalSeparatorsLength + values.Sum(s => s?.Length ?? 0);

        if (totalLength > int.MaxValue)
        {
            ThrowHelper.ThrowInvalidOperationException(
                "Resulting string would be > Int32.MaxValue characters.");
        }

        return totalLength switch
        {
            0 => string.Empty,
            _ => JoinPreAllocated(separator, values, finalSeparator, (int)totalLength)
        };
    }

    private static string JoinPreAllocated(
        ReadOnlySpan<char> separator,
        ICollection<string?> collection,
        ReadOnlySpan<char> finalSeparator,
        int charCount)
    {
        Debug.Assert(collection.Count > 1);
        Debug.Assert(charCount > 0);

        char[]? rented = null;

        Span<char> chars = charCount <= StackallocThresholds.MaxCharLength
            ? stackalloc char[charCount]
            : rented = ArrayPool<char>.Shared.Rent(charCount);

        try
        {
            var written = Format(chars, separator, collection, finalSeparator);
            return new string(chars[..written]);
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    private static string JoinPreAllocated(
        ReadOnlySpan<char> separator,
        ReadOnlySpan<string?> values,
        ReadOnlySpan<char> finalSeparator,
        int charCount)
    {
        Debug.Assert(values.Length > 1);
        Debug.Assert(charCount > 0);

        char[]? rented = null;

        Span<char> chars = charCount <= StackallocThresholds.MaxCharLength
            ? stackalloc char[charCount]
            : rented = ArrayPool<char>.Shared.Rent(charCount);

        try
        {
            var written = 0;
            var wordIndex = 0;

            foreach (var str in values)
            {
                var span = str.AsSpan();

                span.CopyTo(chars.Slice(written, span.Length));
                written += span.Length;
                wordIndex++;

                if (wordIndex < values.Length - 1)
                {
                    foreach (var ch in separator)
                    {
                        chars[written++] = ch;
                    }
                }
                else if (wordIndex == values.Length - 1)
                {
                    foreach (var ch in finalSeparator)
                    {
                        chars[written++] = ch;
                    }
                }
            }

            return new string(chars[..written]);
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }
}
