// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;

namespace DSE.Open.Collections.Generic;

public static class CollectionWriter
{
    public static string? WriteToString<T>(IEnumerable<T>? collection)
    {
        if (collection is null)
        {
            return null;
        }

        var array = collection.ToArray();

        var sb = new StringBuilder(array.Length * 24);

        for (var i = 0; i < array.Length; i++)
        {
            var item = array[i];

            _ = sb.Append(CultureInfo.CurrentCulture, $"\"{item}\"");

            if (i < array.Length - 1)
            {
                _ = sb.Append(", ");
            }
        }

        return sb.ToString();
    }
}
