// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
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
    /// <paramref name="values"/> has more than one element.</param>
    /// <param name="values">A collection that contains the strings to concatenate.</param>
    /// <param name="finalSeparator">The string to use as the final separator. It is included in the returned string
    /// only if <paramref name="values"/> has more than one element.</param>
    /// <returns>A string that consists of the elements of <paramref name="values"/> delimited by the
    /// <paramref name="separator"/> string between each member before the penultimate, and by the
    /// <paramref name="finalSeparator"/> between the penultimate and final members; or, <see cref="string.Empty"/>
    /// if <paramref name="values"/> has zero elements.</returns>   
    public static string Join(string? separator, IEnumerable<string?> values, string? finalSeparator)
    {
        if (values is List<string?> valuesList)
        {
            return JoinCore(separator.AsSpan(), CollectionsMarshal.AsSpan(valuesList), finalSeparator.AsSpan());
        }

        if (values is string?[] valuesArray)
        {
            return JoinCore(separator.AsSpan(), new ReadOnlySpan<string?>(valuesArray), finalSeparator.AsSpan());
        }

        ArgumentNullException.ThrowIfNull(values);

        if (values is ICollection<string?> collection)
        {
            var count = collection.Count;

            if (count == 0)
            {
                return string.Empty;
            }

            var charCount = collection.Sum(s => s is not null ? (long)s.Length : 0)
                            + ((count - 2) * separator?.Length ?? 0)
                            + finalSeparator?.Length ?? 0;

            if (charCount > int.MaxValue)
            {
                ThrowHelper.ThrowInvalidOperationException("Resulting string would be > Int32.MaxValue characters.");
            }

            var length = (int)charCount;

            if (length == 0)
            {
                return string.Empty;
            }

            if (length <= StackallocThresholds.MaxCharLength)
            {
                return JoinPreAllocated(separator.AsSpan(), collection, finalSeparator.AsSpan(), length);
            }

            return string.Create(length, collection, (chars, state) =>
            {
                var charIndex = 0;
                var wordIndex = 0;

                foreach (var str in state)
                {
                    var strSpan = str.AsSpan();

                    strSpan.CopyTo(chars[charIndex..]);
                    charIndex += strSpan.Length;
                    wordIndex++;

                    if (separator is not null && wordIndex < state.Count - 1)
                    {
                        separator.CopyTo(chars[charIndex..]);
                        charIndex += separator.Length;
                    }
                    else if (finalSeparator is not null && wordIndex == state.Count - 1)
                    {
                        finalSeparator.CopyTo(chars[charIndex..]);
                        charIndex += finalSeparator.Length;
                    }
                }
            });
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
    /// Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type <see cref="string"/>,
    /// using the specified <paramref name="separator"/> between each member before the penultimate, and the
    /// specified <paramref name="finalSeparator"/> between the penultimate and final members.
    /// </summary>
    /// <param name="separator">The string to use as a separator. It is included in the returned string only if
    /// <paramref name="values"/> has more than one element.</param>
    /// <param name="values">A collection that contains the strings to concatenate.</param>
    /// <param name="finalSeparator">The string to use as the final separator. It is included in the returned string
    /// only if <paramref name="values"/> has more than one element.</param>
    /// <returns>A string that consists of the elements of <paramref name="values"/> delimited by the
    /// <paramref name="separator"/> string between each member before the penultimate, and by the
    /// <paramref name="finalSeparator"/> between the penultimate and final members; or, <see cref="string.Empty"/>
    /// if <paramref name="values"/> has zero elements.</returns>
    public static string Join(
        ReadOnlySpan<char> separator,
        IEnumerable<string?> values,
        ReadOnlySpan<char> finalSeparator)
    {
        if (values is List<string?> valuesList)
        {
            return JoinCore(separator, CollectionsMarshal.AsSpan(valuesList), finalSeparator);
        }

        if (values is string?[] valuesArray)
        {
            return JoinCore(separator, new ReadOnlySpan<string?>(valuesArray), finalSeparator);
        }

        ArgumentNullException.ThrowIfNull(values);

        if (values is ICollection<string?> collection)
        {
            var count = collection.Count;

            if (count == 0)
            {
                return string.Empty;
            }

            var charCount = collection.Sum(s => s is not null ? (long)s.Length : 0)
                            + ((count - 2) * separator.Length)
                            + finalSeparator.Length;

            if (charCount > int.MaxValue)
            {
                ThrowHelper.ThrowInvalidOperationException(
                    "Resulting string would be > Int32.MaxValue characters.");
            }

            var length = (int)charCount;

            if (length == 0)
            {
                return string.Empty;
            }

            // Cannot use string.Create here because ReadOnlySpan<char> values cannot be
            // passed to lambdas/anonymous functions

            return JoinPreAllocated(separator, collection, finalSeparator, length);
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

    // We add this as String.Join overloads do not inlucde formatting options

    /// <summary>
    /// Concatenates the members of a collection, using the specified <paramref name="separator"/> between each member
    /// before the penultimate, and the specified <paramref name="finalSeparator"/> between the penultimate and final
    /// members, optionally applying a specified <paramref name="format"/> to each member using a specified
    /// <paramref name="provider"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="separator">The string to use as a separator. It is included in the returned string only if
    /// <paramref name="values"/> has more than one element.</param>
    /// <param name="values">A collection that contains the strings to concatenate.</param>
    /// <param name="finalSeparator">The string to use as the final separator. It is included in the returned string
    /// only if <paramref name="values"/> has more than one element.</param>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static string Join<T>(
        ReadOnlySpan<char> separator,
        IEnumerable<T> values,
        ReadOnlySpan<char> finalSeparator,
        string? format = default,
        IFormatProvider? provider = default)
    {
        if (typeof(T) == typeof(string))
        {
            if (values is List<string?> valuesList)
            {
                return JoinCore(separator, CollectionsMarshal.AsSpan(valuesList), finalSeparator);
            }

            if (values is string?[] valuesArray)
            {
                return JoinCore(separator, new ReadOnlySpan<string?>(valuesArray), finalSeparator);
            }

            if (values is IEnumerable<string> valuesEnumerable)
            {
                return Join(separator, valuesEnumerable, finalSeparator);
            }
        }

        ArgumentNullException.ThrowIfNull(values);

        using (var e = values.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                return string.Empty;
            }

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

                    if (isLast)
                    {
                        sh.AppendFormatted(finalSeparator);
                    }
                    else
                    {
                        sh.AppendFormatted(separator);
                    }

                    sh.AppendFormatted(current, format);
                }

                return sh.ToStringAndClear();
            }
        }
    }

    private static string JoinCore(
        ReadOnlySpan<char> separator,
        ReadOnlySpan<string?> values,
        ReadOnlySpan<char> finalSeparator)
    {
        if (values.Length <= 1)
        {
            return values.IsEmpty
                ? string.Empty
                : values[0] ?? string.Empty;
        }

        var totalSeparatorsLength = ((long)(values.Length - 2) * separator.Length)
            + finalSeparator.Length;

        if (totalSeparatorsLength > int.MaxValue)
        {
            ThrowHelper.ThrowInvalidOperationException(
                "Resulting string would be > Int32.MaxValue characters.");
        }

        var totalLength = (int)totalSeparatorsLength;

        foreach (var value in values)
        {
            if (value is not null)
            {
                totalLength += value.Length;

                if (totalLength < 0)
                {
                    ThrowHelper.ThrowInvalidOperationException(
                        "Resulting string would be > Int32.MaxValue characters.");
                }
            }
        }

        if (totalLength == 0)
        {
            return string.Empty;
        }

        return JoinPreAllocated(separator, values, finalSeparator, totalLength);
    }

    private static string JoinPreAllocated(
        ReadOnlySpan<char> separator,
        ICollection<string?> collection,
        ReadOnlySpan<char> finalSeparator,
        int charCount)
    {
        var count = 0;
        char[]? rented = null;

        Span<char> chars = charCount <= StackallocThresholds.MaxCharLength
            ? stackalloc char[charCount]
            : rented = ArrayPool<char>.Shared.Rent(charCount);

        var charIndex = 0;

        try
        {
            var wordIndex = 0;

            foreach (var str in collection)
            {
                var span = str.AsSpan();

                span.CopyTo(chars.Slice(charIndex, span.Length));
                charIndex += span.Length;
                wordIndex++;

                if (wordIndex < count - 1)
                {
                    foreach (var ch in separator)
                    {
                        chars[charIndex++] = ch;
                    }
                }
                else if (wordIndex == count - 1)
                {
                    foreach (var ch in finalSeparator)
                    {
                        chars[charIndex++] = ch;
                    }
                }
            }

            return new string(chars);
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
        char[]? rented = null;

        Span<char> chars = charCount <= StackallocThresholds.MaxCharLength
            ? stackalloc char[charCount]
            : rented = ArrayPool<char>.Shared.Rent(charCount);

        var charIndex = 0;

        try
        {
            var wordIndex = 0;

            foreach (var str in values)
            {
                var span = str.AsSpan();

                span.CopyTo(chars.Slice(charIndex, span.Length));
                charIndex += span.Length;
                wordIndex++;

                if (wordIndex < values.Length - 1)
                {
                    foreach (var ch in separator)
                    {
                        chars[charIndex++] = ch;
                    }
                }
                else if (wordIndex == values.Length - 1)
                {
                    foreach (var ch in finalSeparator)
                    {
                        chars[charIndex++] = ch;
                    }
                }
            }

            return new string(chars[..charIndex]);
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
