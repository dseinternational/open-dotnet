// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Threading.Tasks;

public static class TaskExtensions
{
    public static Task<bool> ToTask(this bool value)
    {
        return value ? CachedTasks.True : CachedTasks.False;
    }

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

    public static Task<T> ToTask<T>(this T value)
    {
        return Task.FromResult(value);
    }
}
