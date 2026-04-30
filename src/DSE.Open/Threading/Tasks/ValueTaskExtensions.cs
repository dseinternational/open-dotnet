// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Threading.Tasks;

/// <summary>
/// Provides extension methods for wrapping values in completed <see cref="ValueTask{TResult}"/> instances.
/// </summary>
public static class ValueTaskExtensions
{
    /// <summary>
    /// Returns a completed <see cref="ValueTask{TResult}"/> whose result is <paramref name="value"/>.
    /// </summary>
    public static ValueTask<T> ToValueTask<T>(this T value)
    {
        return ValueTask.FromResult(value);
    }
}
