// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;

namespace DSE.Open.Collections.Generic;

public static class DictionaryWriter
{
    public static string? WriteToString<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? dictionary)
        where TKey : notnull
    {
        if (dictionary is null)
        {
            return null;
        }

        var dict = dictionary.ToDictionary(x => x.Key, x => x.Value);

        var sb = new StringBuilder(dict.Count * 32);
        var i = 0;

        foreach (var item in dict)
        {
            _ = sb.Append($"{{ \"{item.Key}\": \"{item.Value}\" }}");

            if (i < dict.Count - 1)
            {
                _ = sb.Append(", ");
            }

            i++;
        }

        return sb.ToString();
    }
}
