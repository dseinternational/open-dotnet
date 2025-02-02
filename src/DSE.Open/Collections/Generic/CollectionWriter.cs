// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text;

namespace DSE.Open.Collections.Generic;

public static class CollectionWriter
{
    public static string WriteToString<T>(IEnumerable<T> collection)
    {
        return WriteToString(collection, CollectionOutputOptions.Default);
    }

    public static string WriteToString<T>(IEnumerable<T> collection, CollectionOutputOptions options)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(options);

        var max = options.MaximumCount;

        if (max == 0)
        {
            return "[]";
        }

        var quote = options.QuoteNonNumericValues && !TypeHelper.IsNumericType(typeof(T));

        using var sb = options.InitialOutputCapacity > 1024
            ? new ValueStringBuilder(options.InitialOutputCapacity)
            : new ValueStringBuilder(stackalloc char[1024]);

        sb.Append('[');

        var count = 0;

        foreach (var item in collection)
        {
            if (count > 0)
            {
                sb.Append(',');
            }

            if (quote)
            {
                sb.Append('"');
            }

            sb.Append($"{item}");

            if (quote)
            {
                sb.Append('"');
            }

            count++;

            if (count >= max)
            {
                break;
            }
        }

        sb.Append(']');

        return sb.ToString();
    }

    public static string WriteToString<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        return WriteToString(collection, CollectionOutputOptions.Default);
    }

    public static string WriteToString<TKey, TValue>(
        IEnumerable<KeyValuePair<TKey, TValue>> collection,
        CollectionOutputOptions options)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(options);

        var max = options.MaximumCount;

        if (max == 0)
        {
            return "[]";
        }

        var quoteKeys = options.QuoteNonNumericValues && !TypeHelper.IsNumericType(typeof(TKey));
        var quoteValues = options.QuoteNonNumericValues && !TypeHelper.IsNumericType(typeof(TValue));

        using var sb = options.InitialOutputCapacity > 1024
            ? new ValueStringBuilder(options.InitialOutputCapacity)
            : new ValueStringBuilder(stackalloc char[1024]);

        sb.Append('[');

        var count = 0;

        foreach (var kvp in collection)
        {
            if (count > 0)
            {
                sb.Append(',');
            }

            if (quoteKeys)
            {
                sb.Append('"');
            }

            sb.Append($"{kvp.Key}");

            if (quoteKeys)
            {
                sb.Append('"');
            }

            sb.Append(':');

            if (quoteValues)
            {
                sb.Append('"');
            }

            sb.Append($"{kvp.Value}");

            if (quoteValues)
            {
                sb.Append('"');
            }

            count++;

            if (count >= max)
            {
                break;
            }
        }

        sb.Append(']');

        return sb.ToString();
    }
}
