// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Threading.Tasks;

public static class ValueTaskExtensions
{
    public static ValueTask<T> ToValueTask<T>(this T value)
    {
        return ValueTask.FromResult(value);
    }
}
