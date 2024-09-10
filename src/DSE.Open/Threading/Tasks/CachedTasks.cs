// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Threading.Tasks;

public static class CachedTasks
{
    public static readonly Task Empty = EmptyTasks<object>.Default;

    public static readonly Task<bool> False = Task.FromResult(false);

    public static readonly Task<bool> True = Task.FromResult(true);

    public static Task<T> Default<T>()
    {
        return EmptyTasks<T>.Default;
    }

    internal static Task<T[]> EmptyArray<T>()
    {
        return EmptyTasks<T>.EmptyArray;
    }

    internal static Task<IEnumerable<T>> EmptyArrayEnumerable<T>()
    {
        return EmptyTasks<T>.EmptyArrayEnumerable;
    }

    public static Task<IEnumerable<T>> EmptyEnumerable<T>()
    {
        return EmptyTasks<T>.EmptyEnumerable;
    }

    public static Task<IReadOnlyCollection<T>> EmptyReadOnlyCollection<T>()
    {
        return EmptyTasks<T>.EmptyReadOnlyCollection;
    }

    public static Task<IReadOnlyList<T>> EmptyReadOnlyList<T>()
    {
        return EmptyTasks<T>.EmptyReadOnlyList;
    }

    private static class EmptyTasks<T>
    {
        public static readonly Task<T> Default = Task.FromResult(default(T))!;

        public static readonly Task<T[]> EmptyArray = Task.FromResult(Array.Empty<T>());

        public static readonly Task<IEnumerable<T>> EmptyArrayEnumerable = Task.FromResult((IEnumerable<T>)[]);

        public static readonly Task<IEnumerable<T>> EmptyEnumerable = Task.FromResult(Enumerable.Empty<T>());

        public static readonly Task<IReadOnlyCollection<T>> EmptyReadOnlyCollection = Task.FromResult<IReadOnlyCollection<T>>([]);

        public static readonly Task<IReadOnlyList<T>> EmptyReadOnlyList = Task.FromResult<IReadOnlyList<T>>([]);
    }
}
