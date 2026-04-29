// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Serialization.DataTransfer;

/// <summary>
/// Base implementation of an immutable object that can be serialized/deserialized.
/// </summary>
[Obsolete("Implement JsonExtensionData directly if required")]
public abstract record ImmutableDataTransferObject : IJsonSerializable, IExtensionData
{
    private ReadOnlyValueDictionary<string, object>? _extensionData;

    /// <summary>
    /// Gets the read-only dictionary of additional data captured during deserialization that
    /// does not match any declared property on this object.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyDictionary<string, object> ExtensionData => ExtensionDataCore;

#pragma warning disable IDE0028 // Simplify collection initialization
    IDictionary<string, object> IExtensionData.ExtensionData
        => _extensionData is not null
            ? new(_extensionData)
            : new Dictionary<string, object>();
#pragma warning restore IDE0028 // Simplify collection initialization

    [JsonExtensionData]
    [JsonPropertyOrder(2100010010)]
    internal ReadOnlyValueDictionary<string, object> ExtensionDataCore
    {
        get => _extensionData ??= [];
        init => _extensionData = value;
    }

    /// <summary>
    /// Gets a value indicating whether any extension data has been captured.
    /// </summary>
    [JsonIgnore]
    public bool HasExtensionData => _extensionData is not null && _extensionData.Count > 0;
}
