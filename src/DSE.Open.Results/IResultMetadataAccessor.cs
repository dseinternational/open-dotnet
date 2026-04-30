// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

/// <summary>
/// Provides access to the current ambient <see cref="ResultMetadata"/>.
/// </summary>
public interface IResultMetadataAccessor
{
    /// <summary>
    /// Gets the current <see cref="ResultMetadata"/>.
    /// </summary>
    ResultMetadata Current { get; }
}
