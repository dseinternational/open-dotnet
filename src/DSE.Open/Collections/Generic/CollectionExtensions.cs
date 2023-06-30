// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace DSE.Open.Collections.Generic;

public static class CollectionExtensions
{
    private static readonly ThreadLocal<Random> s_random =
        new(() => new Random(unchecked((Environment.TickCount * 31) + Environment.CurrentManagedThreadId)));

    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T>? values)
    {
        Guard.IsNotNull(collection);

        if (values is null)
        {
            return;
        }

        foreach (var item in values)
        {
            collection.Add(item);
        }
    }

    public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T>? values)
    {
        Guard.IsNotNull(collection);

        if (values is null)
        {
            return;
        }

        foreach (var item in values)
        {
            _ = collection.Remove(item);
        }
    }

    public static IList<T> FindAll<T>(this IEnumerable<T> collection, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

        List<T> results = new();

        foreach (var item in collection)
        {
            if (match(item))
            {
                results.Add(item);
            }
        }

        return results;
    }

    public static int FindIndex<T>(this T[] collection, Predicate<T> match) => collection.FindIndex(0, match);

    public static int FindIndex<T>(this T[] collection, int startIndex, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

        if (startIndex > collection.Length - 1)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(startIndex));
            return -1; // unreachable
        }

        return collection.FindIndex(startIndex, collection.Length, match);
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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

        for (var i = startIndex; i < count; i++)
        {
            if (match(collection[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static int FindIndex<T>(this IList<T> collection, Predicate<T> match) => collection.FindIndex(0, match);

    public static int FindIndex<T>(this IList<T> collection, int startIndex, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

        for (var i = startIndex; i < count; i++)
        {
            if (match(collection[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static int FindIndex<T>(this IReadOnlyList<T> collection, Predicate<T> match) => collection.FindIndex(0, match);

    public static int FindIndex<T>(this IReadOnlyList<T> collection, int startIndex, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

        for (var i = startIndex; i < count; i++)
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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

        return collection.FindLastIndex(collection.Length - 1, match);
    }

    public static int FindLastIndex<T>(this T[] collection, int startIndex, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);
        Guard.IsLessThan(startIndex, int.MaxValue);

        return collection.FindLastIndex(startIndex, startIndex + 1, match);
    }

    public static int FindLastIndex<T>(this T[] collection, int startIndex, int count, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

        return collection.FindLastIndex(collection.Count - 1, match);
    }

    public static int FindLastIndex<T>(this IList<T> collection, int startIndex, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);
        Guard.IsLessThan(startIndex, int.MaxValue);

        return collection.FindLastIndex(startIndex, startIndex + 1, match);
    }

    public static int FindLastIndex<T>(this IList<T> collection, int startIndex, int count, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

        return collection.FindLastIndex(collection.Count - 1, match);
    }

    public static int FindLastIndex<T>(this IReadOnlyList<T> collection, int startIndex, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);
        Guard.IsLessThan(startIndex, int.MaxValue);

        return collection.FindLastIndex(startIndex, startIndex + 1, match);
    }

    public static int FindLastIndex<T>(this IReadOnlyList<T> collection, int startIndex, int count, Predicate<T> match)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(action);

        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this ReadOnlyMemory<T> collection, Action<T> action)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(action);

        var span = collection.Span;

        foreach (var item in span)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        Guard.IsNotNull(array);
        Guard.IsNotNull(action);

        foreach (var item in array)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this IList<T> collection, Action<T> action)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(action);

        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this IReadOnlyList<T> collection, Action<T> action)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(action);

        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static IEnumerable<T> RandomSelection<T>(this IEnumerable<T> collection, int count)
    {
        Guard.IsNotNull(collection);

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
        Guard.IsNotNull(collection);
        Guard.IsNotNull(match);

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
        Guard.IsNotNull(list);

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

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection)
    {
        Guard.IsNotNull(collection);
        return new HashSet<T>(collection);
    }

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection, IEqualityComparer<T>? equalityComparer)
    {
        Guard.IsNotNull(collection);
        return new HashSet<T>(collection, equalityComparer);
    }

    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
    {
        Guard.IsNotNull(collection);
        return new ObservableCollection<T>(collection);
    }

    public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
    {
        Guard.IsNotNull(list);
        return new ObservableCollection<T>(list);
    }

    public static ReadOnlyValueCollection<T> ToReadOnlyValueCollection<T>(this IEnumerable<T> collection)
    {
        Guard.IsNotNull(collection);

        return collection is ReadOnlyValueCollection<T> rovc ? rovc : new ReadOnlyValueCollection<T>(collection);
    }

    public static ValueCollection<T> ToValueCollection<T>(this IEnumerable<T> collection)
    {
        Guard.IsNotNull(collection);

        return collection is ValueCollection<T> vc ? vc : new ValueCollection<T>(collection);
    }

    public static string? WriteToString<T>(this IEnumerable<T>? collection) => CollectionWriter.WriteToString(collection);
}
