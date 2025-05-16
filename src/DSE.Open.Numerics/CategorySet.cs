// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics;

public static class CategorySet
{
    public static CategorySet<T> Create<T>(ISet<T> set)
        where T : struct, IBinaryNumber<T>
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        return new CategorySet<T>(set);
#pragma warning restore IDE0028 // Simplify collection initialization
    }

    public static CategorySet<T> Create<T>(ReadOnlySpan<T> span)
        where T : struct, IBinaryNumber<T>
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        return new CategorySet<T>(span.ToArray());
#pragma warning restore IDE0028 // Simplify collection initialization
    }
}

[CollectionBuilder(typeof(CategorySet), nameof(CategorySet.Create))]
public sealed class CategorySet<T> : Set<T>, ICategorySet<T>
    where T : struct, IBinaryNumber<T>
{
    public CategorySet()
    {
    }

    public CategorySet(IEnumerable<T> set) : base(set)
    {
    }

    public CategorySet(ISet<T> set) : base(set)
    {
    }

    public new ReadOnlyCategorySet<T> AsReadOnly()
    {
        return new ReadOnlyCategorySet<T>(this);
    }
}
