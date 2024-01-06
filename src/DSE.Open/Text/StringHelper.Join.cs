// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;

namespace DSE.Open.Text;

public static partial class StringHelper
{
    public static string Join(string? separator, IEnumerable<string?> values, string? finalSeparator)
    {
        if (finalSeparator is null)
        {
            return string.Join(separator, values);
        }

        if (string.IsNullOrEmpty(separator))
        {
            return string.Concat(values);
        }

        if (values is List<string?> valuesList)
        {
            return JoinCore(separator.AsSpan(), CollectionsMarshal.AsSpan(valuesList), finalSeparator);
        }

        if (values is string?[] valuesArray)
        {
            return JoinCore(separator.AsSpan(), new ReadOnlySpan<string?>(valuesArray), finalSeparator);
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
                ThrowHelper.ThrowInvalidOperationException("Resulting string would be > Int32.MaxValue characters.");
            }

            var length = (int)charCount;

            // TODO: for smaller strings, consider using stackalloc to build the result

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

                    if (wordIndex < count - 1)
                    {
                        separator.CopyTo(chars[charIndex..]);
                        charIndex += separator.Length;
                    }
                    else if (wordIndex == count - 1)
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

    public static string Join(ReadOnlySpan<char> separator, IEnumerable<string?> values, ReadOnlySpan<char> finalSeparator)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (finalSeparator.IsEmpty)
        {
            return string.Join(separator.ToString(), values);
        }

        throw new NotImplementedException();
    }

    public static string Join<T>(ReadOnlySpan<char> separator, IEnumerable<T> values, ReadOnlySpan<char> finalSeparator)
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
        }

        ArgumentNullException.ThrowIfNull(values);

        if (finalSeparator.IsEmpty)
        {
            return string.Join(separator.ToString(), values);
        }

        throw new NotImplementedException();
    }

    private static string JoinCore(ReadOnlySpan<char> separator, ReadOnlySpan<string?> values, ReadOnlySpan<char> finalSeparator)
    {
        if (values.Length <= 1)
        {
            return values.IsEmpty
                ? string.Empty
                : values[0] ?? string.Empty;
        }

        var totalSeparatorsLength = ((long)(values.Length - 2) * separator.Length) + finalSeparator.Length;

        if (totalSeparatorsLength > int.MaxValue)
        {
            ThrowHelper.ThrowInvalidOperationException("Resulting string would be > Int32.MaxValue characters.");
        }

        var totalLength = (int)totalSeparatorsLength;

        foreach (var value in values)
        {
            if (value is not null)
            {
                totalLength += value.Length;

                if (totalLength < 0) // Check for overflow
                {
                    ThrowHelper.ThrowInvalidOperationException("Resulting string would be > Int32.MaxValue characters.");
                }
            }
        }

        if (totalLength == 0)
        {
            return string.Empty;
        }

        char[]? rented = null;

        Span<char> chars = totalLength < StackallocThresholds.MaxCharLength
            ? stackalloc char[totalLength]
            : rented = ArrayPool<char>.Shared.Rent(totalLength);

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
