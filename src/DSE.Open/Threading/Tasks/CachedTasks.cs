// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Threading.Tasks;

/// <summary>
/// Provides cached, completed <see cref="Task"/> and <see cref="Task{TResult}"/> instances
/// for common values to avoid unnecessary allocations.
/// </summary>
public static class CachedTasks
{
    /// <summary>
    /// A completed task with no result value.
    /// </summary>
    public static readonly Task Empty = EmptyTasks<object>.Default;

    /// <summary>
    /// A completed task with a result of <see langword="false"/>.
    /// </summary>
    public static readonly Task<bool> False = Task.FromResult(false);

    /// <summary>
    /// A completed task with a result of <see langword="true"/>.
    /// </summary>
    public static readonly Task<bool> True = Task.FromResult(true);

    /// <summary>
    /// Returns a completed task whose result is the default value of <typeparamref name="T"/>.
    /// </summary>
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

    /// <summary>
    /// Returns a completed task whose result is an empty <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static Task<IEnumerable<T>> EmptyEnumerable<T>()
    {
        return EmptyTasks<T>.EmptyEnumerable;
    }

    /// <summary>
    /// Returns a completed task whose result is an empty <see cref="IReadOnlyCollection{T}"/>.
    /// </summary>
    public static Task<IReadOnlyCollection<T>> EmptyReadOnlyCollection<T>()
    {
        return EmptyTasks<T>.EmptyReadOnlyCollection;
    }

    /// <summary>
    /// Returns a completed task whose result is an empty <see cref="IReadOnlyList{T}"/>.
    /// </summary>
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
