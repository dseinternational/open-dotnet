// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static class ReadOnlyCategorySetExtensions
{
    public static CategorySet<T> Copied<T>(this ReadOnlyCategorySet<T> categorySet)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(categorySet);
        return new CategorySet<T>(categorySet);
    }
}
