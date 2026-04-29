// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace DSE.Open.Serialization;

/// <summary>
/// Provides extension methods for <see cref="IJsonSerializable"/> instances.
/// </summary>
public static class JsonSerializableExtensions
{
    /// <summary>
    /// Serializes the specified object to a JSON string using its runtime type.
    /// </summary>
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    [Obsolete("Removing")]
    public static string ToJsonString(this IJsonSerializable obj, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return JsonSerializer.Serialize(obj, obj.GetType(), options);
    }
}
