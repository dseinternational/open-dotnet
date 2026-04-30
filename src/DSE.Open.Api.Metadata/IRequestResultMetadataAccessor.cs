// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Requests;
using DSE.Open.Results;

namespace DSE.Open.Api.Metadata;

/// <summary>
/// Provides access to the current <see cref="RequestMetadata"/> and <see cref="ResultMetadata"/>.
/// </summary>
public interface IRequestResultMetadataAccessor
{
    /// <summary>
    /// Gets the current <see cref="RequestMetadata"/>.
    /// </summary>
    RequestMetadata CurrentRequest { get; }

    /// <summary>
    /// Gets the current <see cref="ResultMetadata"/>.
    /// </summary>
    ResultMetadata CurrentResult { get; }
}
