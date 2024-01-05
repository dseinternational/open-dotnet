// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text;

namespace DSE.Open.Text;

public static class StringConcatenator
{
    public static string Join_Original(string separator, IEnumerable<string> values, string? finalSeparator = null)
    {
        ArgumentNullException.ThrowIfNull(separator);
        ArgumentNullException.ThrowIfNull(values);

        if (finalSeparator is null)
        {
            return string.Join(separator, values);
        }

        var sb = new StringBuilder();

        if (values is ICollection<string> collection)
        {
            var count = collection.Count;

            if (count == 0)
            {
                return string.Empty;
            }

            var i = 0;

            foreach (var s in collection)
            {
                _ = sb.Append(s);

                if (i < count - 2)
                {
                    _ = sb.Append(separator);
                }
                else if (i == count - 2)
                {
                    _ = sb.Append(finalSeparator);
                }

                i++;
            }

            return sb.ToString();
        }

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

                if (isLast)
                {
                    _ = sb.Append(finalSeparator);
                }
                else
                {
                    _ = sb.Append(separator);
                }

                _ = sb.Append(current);
            }
        }

        return sb.ToString();
    }


    public static string Join2(string separator, IEnumerable<string> values, string? finalSeparator = null)
    {
        ArgumentNullException.ThrowIfNull(separator);
        ArgumentNullException.ThrowIfNull(values);

        if (finalSeparator is null)
        {
            return string.Join(separator, values);
        }

        char[]? rented = null;

        if (values is ICollection<string> collection)
        {
            var count = collection.Count;

            if (count == 0)
            {
                return string.Empty;
            }

            var charCount = collection.Sum(s => s.Length)
                            + ((count - 2) * separator.Length)
                            + finalSeparator.Length;

            Span<char> chars = charCount < StackallocThresholds.MaxCharLength
                ? stackalloc char[charCount]
                : rented = ArrayPool<char>.Shared.Rent(charCount);

            try
            {
                var charIndex = 0;
                var wordIndex = 0;

                foreach (var str in collection)
                {
                    foreach (var ch in str)
                    {
                        chars[charIndex++] = ch;
                    }

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

        // for non-collection enumerables, we need to enumerate twice - use same char array approach as above
        var valuesArray = values.ToArray();
        var valuesCount = valuesArray.Length;

        if (valuesCount == 0)
        {
            return string.Empty;
        }

        var charCount2 = valuesArray.Sum(s => s.Length) + ((valuesCount - 2) * separator.Length) + finalSeparator.Length;

        Span<char> chars2 = charCount2 < StackallocThresholds.MaxCharLength
            ? stackalloc char[charCount2]
            : rented = ArrayPool<char>.Shared.Rent(charCount2);

        var charIndex2 = 0;

        try
        {

            for (var i = 0; i < valuesCount; i++)
            {
                var str = valuesArray[i];

                foreach (var ch in str)
                {
                    chars2[charIndex2++] = ch;
                }

                if (i < valuesCount - 2)
                {
                    foreach (var ch in separator)
                    {
                        chars2[charIndex2++] = ch;
                    }
                }
                else if (i == valuesCount - 2)
                {
                    foreach (var ch in finalSeparator)
                    {
                        chars2[charIndex2++] = ch;
                    }
                }
            }

            return new string(chars2);
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    public static string Join(string separator, IEnumerable<string> values, string? finalSeparator = null)
    {

        // pass through to string.Join if no final separator is specified
        if (finalSeparator is null)
        {
            return string.Join(separator, values);
        }

        // get size of char array needed to hold the result
        var charCount = 0;
        var count = 0;

        var enumerable = values as string[] ?? values.ToArray();
        foreach (var str in enumerable)
        {
            charCount += str.Length;
            count++;
        }

        // add the separators if not empty
        if (!string.IsNullOrEmpty(separator))
        {
            charCount += (count - 2) * separator.Length;
        }

        // add the final separator
        charCount += finalSeparator.Length;

        char[]? rented = null;

        Span<char> chars = charCount < StackallocThresholds.MaxCharLength
            ? stackalloc char[charCount]
            : rented = ArrayPool<char>.Shared.Rent(charCount);

        var charIndex = 0;

try
        {
            var wordIndex = 0;

            foreach (var str in enumerable)
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

}
