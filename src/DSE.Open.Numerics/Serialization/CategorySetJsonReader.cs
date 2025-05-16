// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

public static class CategorySetJsonReader
{
    public static ICategorySet? Read(ref Utf8JsonReader reader)
    {
        throw new NotImplementedException("ReadCategorySet is not implemented yet.");
    }

    public static CategorySet<T>? Read<T>(ref Utf8JsonReader reader)
        where T : struct, IBinaryNumber<T>
    {
        throw new NotImplementedException("ReadCategorySet<T> is not implemented yet.");
    }
}
