// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Requests;

/// <summary>
/// Ambient accessor for the <see cref="RequestMetadata"/> associated with the
/// current logical operation (typically scoped per-request in a hosting
/// environment).
/// </summary>
public interface IRequestMetadataAccessor
{
    /// <summary>The <see cref="RequestMetadata"/> for the current operation.</summary>
    RequestMetadata Current { get; }
}
