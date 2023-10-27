// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Serialization.DataTransfer;

/// <summary>
/// Base implementation of an object that can be serialized/deserialized
/// to pass data between processes.
/// </summary>
public abstract record DataTransferObject : IJsonSerializable, IExtensionData
{
    private ValueDictionary<string, object>? _extensionData;

    [JsonIgnore]
    public ValueDictionary<string, object> ExtensionData => ExtensionDataCore;

    IDictionary<string, object> IExtensionData.ExtensionData
        => _extensionData is not null
            ? new Dictionary<string, object>(_extensionData)
            : new Dictionary<string, object>();

    [JsonExtensionData]
    [JsonPropertyOrder(2100010010)]
    internal ValueDictionary<string, object> ExtensionDataCore
    {
        get => _extensionData ??= ValueDictionary<string, object>.Empty;
        init => _extensionData = new ValueDictionary<string, object>(value);
    }

    [JsonIgnore]
    public bool HasExtensionData => _extensionData is not null && _extensionData.Count > 0;
}
