// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;

namespace DSE.Open.Text;

public static partial class StringHelper
{
    public static string Join(string? separator, IEnumerable<string?> values, string? finalSeparator)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (finalSeparator is null)
        {
            return string.Join(separator, values);
        }

        if (string.IsNullOrEmpty(separator))
        {
            return string.Concat(values);
        }

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

            return string.Create(length, collection, (chars, col) =>
            {
                var charIndex = 0;
                var wordIndex = 0;

                foreach (var str in collection)
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
        ArgumentNullException.ThrowIfNull(values);

        if (finalSeparator.IsEmpty)
        {
            return string.Join(separator.ToString(), values);
        }

        throw new NotImplementedException();
    }
}
