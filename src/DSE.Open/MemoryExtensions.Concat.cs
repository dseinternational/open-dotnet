// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static partial class MemoryExtensions
{
    /// <summary>
    /// Concatenates two sequences into a new sequence. (Items are copied.)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public static T[] Concat<T>(this Span<T> s1, ReadOnlySpan<T> s2)
    {
        return Concat((ReadOnlySpan<T>)s1, s2);
    }

    /// <summary>
    /// Concatenates two sequences into a new sequence. (Items are copied.)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public static T[] Concat<T>(this ReadOnlySpan<T> s1, ReadOnlySpan<T> s2)
    {
        var array = new T[s1.Length + s2.Length];
        s1.CopyTo(array);
        s2.CopyTo(array.AsSpan(s1.Length));
        return array;
    }

    /// <summary>
    /// Concatenates three sequences into a new sequence. (Items are copied.)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <param name="s3"></param>
    /// <returns></returns>
    public static T[] Concat<T>(this Span<T> s1, ReadOnlySpan<T> s2, ReadOnlySpan<T> s3)
    {
        return Concat((ReadOnlySpan<T>)s1, s2, s3);
    }

    /// <summary>
    /// Concatenates three sequences into a new sequence. (Items are copied.)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <param name="s3"></param>
    /// <returns></returns>
    public static T[] Concat<T>(this ReadOnlySpan<T> s1, ReadOnlySpan<T> s2, ReadOnlySpan<T> s3)
    {
        var array = new T[s1.Length + s2.Length + s3.Length];
        s1.CopyTo(array);
        s2.CopyTo(array.AsSpan(s1.Length));
        s3.CopyTo(array.AsSpan(s1.Length + s2.Length));
        return array;
    }

    /// <summary>
    /// Concatenates a sequences and one element into a new sequence. (Items are copied.)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s1"></param>
    /// <param name="e1"></param>
    /// <returns></returns>
    public static T[] Concat<T>(this Span<T> s1, T e1)
    {
        return Concat((ReadOnlySpan<T>)s1, e1);
    }

    /// <summary>
    /// Concatenates a sequences and one element into a new sequence. (Items are copied.)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s1"></param>
    /// <param name="e1"></param>
    /// <returns></returns>
    public static T[] Concat<T>(this ReadOnlySpan<T> s1, T e1)
    {
        var array = new T[s1.Length + 1];
        s1.CopyTo(array);
        array[s1.Length] = e1;
        return array;
    }

    /// <summary>
    /// Concatenates a sequences and two elements into a new sequence. (Items are copied.)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s1"></param>
    /// <param name="e1"></param>
    /// <param name="e2"></param>
    /// <returns></returns>
    public static T[] Concat<T>(this Span<T> s1, T e1, T e2)
    {
        return Concat((ReadOnlySpan<T>)s1, e1, e2);
    }

    /// <summary>
    /// Concatenates a sequences and two elements into a new sequence. (Items are copied.)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s1"></param>
    /// <param name="e1"></param>
    /// <param name="e2"></param>
    /// <returns></returns>
    public static T[] Concat<T>(this ReadOnlySpan<T> s1, T e1, T e2)
    {
        var array = new T[s1.Length + 2];
        s1.CopyTo(array);
        array[s1.Length] = e1;
        array[s1.Length + 1] = e2;
        return array;
    }
}
