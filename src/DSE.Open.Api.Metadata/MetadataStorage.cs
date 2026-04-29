// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Api.Metadata;

/// <summary>
/// Identifies the storage medium used to convey metadata.
/// </summary>
public enum MetadataStorage
{
    /// <summary>
    /// Metadata is conveyed via HTTP headers.
    /// </summary>
    HttpHeader,

    /// <summary>
    /// Metadata is conveyed via HTTP cookies.
    /// </summary>
    Cookie,

    /// <summary>
    /// Metadata is conveyed via message headers.
    /// </summary>
    MessageHeader,
}
