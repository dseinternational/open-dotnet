// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Threading.Tasks;

public static class TaskExtensions
{
    public static Task<T> ToTask<T>(this T value)
    {
        return Task.FromResult(value);
    }
}
