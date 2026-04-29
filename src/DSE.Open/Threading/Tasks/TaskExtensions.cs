// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Threading.Tasks;

/// <summary>
/// Provides extension methods for wrapping values in completed <see cref="Task{TResult}"/> instances.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Returns a cached completed task with the supplied <see cref="bool"/> result.
    /// </summary>
    public static Task<bool> ToTask(this bool value)
    {
        return value ? CachedTasks.True : CachedTasks.False;
    }

    /// <summary>
    /// Returns a completed task whose result is <paramref name="value"/>, returning a
    /// shared cached task when <paramref name="value"/> is a recognised empty sequence.
    /// </summary>
    public static Task<IEnumerable<T>> ToTask<T>(this IEnumerable<T> value)
    {
        if (ReferenceEquals(value, Array.Empty<T>()))
        {
            return CachedTasks.EmptyArrayEnumerable<T>();
        }

        if (ReferenceEquals(value, Enumerable.Empty<T>()))
        {
            return CachedTasks.EmptyEnumerable<T>();
        }

        return Task.FromResult(value);
    }

    /// <summary>
    /// Returns a completed task whose result is <paramref name="value"/>.
    /// </summary>
    public static Task<T> ToTask<T>(this T value)
    {
        return Task.FromResult(value);
    }
}
