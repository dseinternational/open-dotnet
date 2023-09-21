// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class UriAsciiPathListToStringCollectionConverter : ValueConverter<IList<UriAsciiPath>, IEnumerable<string>>
{
    public static readonly UriAsciiPathListToStringCollectionConverter Default = new();

    public UriAsciiPathListToStringCollectionConverter()
        : base(v => ToStringCollection(v), v => ToPathCollection(v))
    {
    }

    private static IEnumerable<string> ToStringCollection(IList<UriAsciiPath> set)
    {
        Guard.IsNotNull(set);
        return set.Select(p => p.ToString());
    }

    private static IList<UriAsciiPath> ToPathCollection(IEnumerable<string> collection)
    {
        Guard.IsNotNull(collection);
        return collection.Select(UriAsciiPath.Parse).ToList();
    }
}
