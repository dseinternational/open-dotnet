// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

internal static class JsonExceptionHelper
{
    public static void ThrowIfLengthExceedsSerializationLimit(int length)
    {
        if (length > VectorJsonConstants.MaximumSerializedLength)
        {
            throw new JsonException($"Vector length exceeds maximum serialization limit of {VectorJsonConstants.MaximumSerializedLength}");
        }
    }
}
