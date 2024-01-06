// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Text;

public static class StringConcatenator
{
    public static string Join(string separator, IEnumerable<string> values, string? finalSeparator = null)
    {
        ArgumentNullException.ThrowIfNull(separator);
        ArgumentNullException.ThrowIfNull(values);

        if (finalSeparator is null)
        {
            return separator.Length == 0
                ? string.Concat(values)
                : string.Join(separator, values);
        }

        var valuesArray = values as string[] ?? values.ToArray();

        var count = valuesArray.Length;

        if (count == 0)
        {
            return string.Empty;
        }

        var charCount = valuesArray.Sum(s => s is not null ? (long)s.Length : 0) + ((count - 2) * separator.Length) + finalSeparator.Length;

        if (charCount > int.MaxValue)
        {
            ThrowHelper.ThrowInvalidOperationException("Resulting string would be > Int32.MaxValue characters.");
        }

        var length = (int)charCount;

        char[]? rented = null;

        Span<char> chars = length <= StackallocThresholds.MaxCharLength
            ? stackalloc char[length]
            : rented = ArrayPool<char>.Shared.Rent(length);

        var charIndex = 0;

        try
        {
            var wordIndex = 0;

            foreach (var str in valuesArray)
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
