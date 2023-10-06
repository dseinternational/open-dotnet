// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;

namespace DSE.Open.Api.Metadata;

public class MetadataStorageContext
{
    public MetadataStorage StorageType { get; init; }

    public ConcurrentDictionary<string, string> Data { get; } = new ConcurrentDictionary<string, string>();
}
