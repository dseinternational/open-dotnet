// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace DSE.Open.Collections.Generic;

public static class CollectionExtensions
{
    private static readonly ThreadLocal<Random> s_random =
        new(() => new(unchecked((Environment.TickCount * 31) + Environment.CurrentManagedThreadId)));

    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T>? values)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (values is null)
        {
            return;
        }

        if (collection is List<T> list)
        {
            list.AddRange(values);
            return;
        }

        foreach (var item in values)
        {
            collection.Add(item);
        }
    }

    public static int FindIndex<T>(this T[] collection, Predicate<T> match)
    {
        return collection.FindIndex(0, match);
    }

    public static int FindIndex<T>(this T[] collection, int startIndex, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        if (startIndex > collection.Length - 1)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(startIndex));
            return -1; // unreachable
        }

        return Array.FindIndex(collection, startIndex, match);
    }

    /// <summary>
    /// Returns the index of the first item matching the specified match, commencing the
    /// search at the specified start index and testing a specified number of items in the
    /// list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="startIndex"></param>
    /// <param name="count"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static int FindIndex<T>(this T[] collection, int startIndex, int count, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        for (var i = startIndex; i < count; i++)
        {
            if (match(collection[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static int FindIndex<T>(this IList<T> collection, Predicate<T> match)
    {
        return collection.FindIndex(0, match);
    }

    public static int FindIndex<T>(this IList<T> collection, int startIndex, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        if (startIndex > collection.Count - 1)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(startIndex));
            return -1; // unreachable
        }

        return collection.FindIndex(startIndex, collection.Count, match);
    }

    /// <summary>
    /// Returns the index of the first item matching the specified match, commencing the
    /// search at the specified start index and testing a specified number of items in the
    /// list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="startIndex"></param>
    /// <param name="count"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static int FindIndex<T>(this IList<T> collection, int startIndex, int count, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        var upperBound = startIndex + count;

        for (var i = startIndex; i < upperBound; i++)
        {
            if (match(collection[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static int FindIndex<T>(this IReadOnlyList<T> collection, Predicate<T> match)
    {
        return collection.FindIndex(0, match);
    }

    public static int FindIndex<T>(this IReadOnlyList<T> collection, int startIndex, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        if (startIndex > collection.Count - 1)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(startIndex));
            return -1; // unreachable
        }

        return collection.FindIndex(startIndex, collection.Count, match);
    }

    /// <summary>
    /// Returns the index of the first item matching the specified match, commencing the
    /// search at the specified start index and testing a specified number of items in the
    /// list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="startIndex"></param>
    /// <param name="count"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static int FindIndex<T>(this IReadOnlyList<T> collection, int startIndex, int count, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        var upperBound = startIndex + count;

        for (var i = startIndex; i < upperBound; i++)
        {
            if (match(collection[i]))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate,
    /// and returns the last occurrence within the entire array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static T? FindLast<T>(this T[] collection, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        for (var i = collection.Length - 1; i >= 0; i--)
        {
            if (match(collection[i]))
            {
                return collection[i];
            }
        }

        return default;
    }

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate,
    /// and returns the last occurrence within the entire list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static T? FindLast<T>(this IList<T> collection, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        for (var i = collection.Count - 1; i >= 0; i--)
        {
            if (match(collection[i]))
            {
                return collection[i];
            }
        }

        return default;
    }

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate,
    /// and returns the last occurrence within the entire list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static T? FindLast<T>(this IReadOnlyList<T> collection, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        for (var i = collection.Count - 1; i >= 0; i--)
        {
            if (match(collection[i]))
            {
                return collection[i];
            }
        }

        return default;
    }

    public static int FindLastIndex<T>(this T[] collection, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        return collection.FindLastIndex(collection.Length - 1, match);
    }

    public static int FindLastIndex<T>(this T[] collection, int startIndex, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);
        Guard.IsLessThan(startIndex, int.MaxValue);

        return collection.FindLastIndex(startIndex, startIndex + 1, match);
    }

    public static int FindLastIndex<T>(this T[] collection, int startIndex, int count, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        Guard.IsGreaterThanOrEqualTo(startIndex, 0);
        Guard.IsGreaterThanOrEqualTo(count, 0);
        Guard.IsGreaterThanOrEqualTo(startIndex, collection.Length - 1);

        var endIndex = startIndex - count;

        for (var i = startIndex; i > endIndex - count; i--)
        {
            if (match(collection[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static int FindLastIndex<T>(this IList<T> collection, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        return collection.FindLastIndex(collection.Count - 1, match);
    }

    public static int FindLastIndex<T>(this IList<T> collection, int startIndex, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);
        Guard.IsLessThan(startIndex, int.MaxValue);

        return collection.FindLastIndex(startIndex, startIndex + 1, match);
    }

    public static int FindLastIndex<T>(this IList<T> collection, int startIndex, int count, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        Guard.IsGreaterThanOrEqualTo(startIndex, 0);
        Guard.IsGreaterThanOrEqualTo(count, 0);
        Guard.IsGreaterThanOrEqualTo(startIndex, collection.Count - 1);

        var endIndex = startIndex - count;

        for (var i = startIndex; i > endIndex - count; i--)
        {
            if (match(collection[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static int FindLastIndex<T>(this IReadOnlyList<T> collection, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        return collection.FindLastIndex(collection.Count - 1, match);
    }

    public static int FindLastIndex<T>(this IReadOnlyList<T> collection, int startIndex, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);
        Guard.IsLessThan(startIndex, int.MaxValue);

        return collection.FindLastIndex(startIndex, startIndex + 1, match);
    }

    public static int FindLastIndex<T>(this IReadOnlyList<T> collection, int startIndex, int count, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        Guard.IsGreaterThanOrEqualTo(startIndex, 0);
        Guard.IsGreaterThanOrEqualTo(count, 0);
        Guard.IsGreaterThanOrEqualTo(startIndex, collection.Count - 1);

        var endIndex = startIndex - count;

        for (var i = startIndex; i > endIndex - count; i--)
        {
            if (match(collection[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this ReadOnlyMemory<T> collection, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        var span = collection.Span;

        foreach (var item in span)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in array)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this ImmutableArray<T> collection, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this ImmutableHashSet<T> collection, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this FrozenSet<T> collection, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this IList<T> collection, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this IReadOnlyList<T> collection, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static IEnumerable<T> RandomSelection<T>(this IEnumerable<T> collection, int count)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (s_random.Value is null)
        {
            throw new InvalidOperationException();
        }

        var randomSortTable = new Dictionary<double, T>();

        foreach (var someType in collection)
        {
#pragma warning disable CA5394 // Do not use insecure randomness
            randomSortTable[s_random.Value.NextDouble()] = someType;
#pragma warning restore CA5394 // Do not use insecure randomness
        }

        return randomSortTable.OrderBy(p => p.Key).Take(count).Select(p => p.Value);
    }

    public static int RemoveAll<T>(this IList<T> collection, Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(match);

        var count = 0;

        for (var i = 0; i < collection.Count; i++)
        {
            if (!match(collection[i]))
            {
                continue;
            }

            _ = collection.Remove(collection[i]);
            count++;
            i--;
        }

        return count;
    }

    /// <summary>
    /// Randomly shuffles the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <remarks>See http://en.wikipedia.org/wiki/Fisher-Yates_shuffle</remarks>
    public static void Shuffle<T>(this IList<T> list)
    {
        ArgumentNullException.ThrowIfNull(list);

        if (s_random.Value is null)
        {
            throw new InvalidOperationException();
        }

        var n = list.Count;

        while (n > 1)
        {
            n--;
#pragma warning disable CA5394 // Do not use insecure randomness
            var k = s_random.Value.Next(n + 1);
#pragma warning restore CA5394 // Do not use insecure randomness
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static Collection<T> ToCollection<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        return new(collection);
    }

    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        return new(collection);
    }

    public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
    {
        return new(list);
    }

    public static ReadOnlyValueCollection<T> ToReadOnlyValueCollection<T>(this IEnumerable<T> collection)
    {
        return [.. collection];
    }

    public static ReadOnlyValueDictionary<TKey, TValue> ToReadOnlyValueDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> collection)
        where TKey : notnull
    {
        return new(collection);
    }

    public static ReadOnlyValueSet<T> ToReadOnlyValueSet<T>(this IEnumerable<T> collection)
    {
        return [.. collection];
    }

    public static ValueCollection<T> ToValueCollection<T>(this IEnumerable<T> collection)
    {
        return [.. collection];
    }

    public static string? WriteToString<T>(this IEnumerable<T>? collection)
    {
        return CollectionWriter.WriteToString(collection);
    }

    public static uint CountUnsigned<T>(this IEnumerable<T> sequence)
    {
        if (sequence is ICollection<T> collection)
        {
            return (uint)collection.Count;
        }

        if (sequence is System.Collections.ICollection collection2)
        {
            return (uint)collection2.Count;
        }

        ArgumentNullException.ThrowIfNull(sequence);

        uint count = 0;

        foreach (var item in sequence)
        {
            checked
            {
                count++;
            }
        }

        return count;
    }

    public static uint CountUnsignedUnchecked<T>(this IEnumerable<T> sequence)
    {
        if (sequence is ICollection<T> collection)
        {
            return (uint)collection.Count;
        }

        if (sequence is System.Collections.ICollection collection2)
        {
            return (uint)collection2.Count;
        }

        ArgumentNullException.ThrowIfNull(sequence);

        uint count = 0;

        foreach (var item in sequence)
        {
            count++;
        }

        return count;
    }

    public static uint CountUnsigned<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        ArgumentNullException.ThrowIfNull(predicate);

        uint count = 0;

        foreach (var item in sequence)
        {
            if (!predicate(item))
            {
                checked
                {
                    count++;
                }
            }
        }

        return count;
    }

    public static uint CountUnsignedUnchecked<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        ArgumentNullException.ThrowIfNull(predicate);

        uint count = 0;

        foreach (var item in sequence)
        {
            if (!predicate(item))
            {
                count++;
            }
        }

        return count;
    }
}
