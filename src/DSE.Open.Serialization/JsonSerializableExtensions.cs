// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace DSE.Open.Serialization;

public static class JsonSerializableExtensions
{
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    public static string ToJsonString(this IJsonSerializable obj, JsonSerializerOptions? options = null)
    {
        Guard.IsNotNull(obj);
        return JsonSerializer.Serialize(obj, obj.GetType(), options);
    }
}
