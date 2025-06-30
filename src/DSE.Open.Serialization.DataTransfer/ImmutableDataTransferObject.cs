// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
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

    [JsonIgnore]
    public IReadOnlyDictionary<string, object> ExtensionData => ExtensionDataCore;

    IDictionary<string, object> IExtensionData.ExtensionData
        => _extensionData is not null
            ? new(_extensionData)
            : new Dictionary<string, object>();

    [JsonExtensionData]
    [JsonPropertyOrder(2100010010)]
    internal ReadOnlyValueDictionary<string, object> ExtensionDataCore
    {
        get => _extensionData ??= [];
        init => _extensionData = value;
    }

    [JsonIgnore]
    public bool HasExtensionData => _extensionData is not null && _extensionData.Count > 0;
}
