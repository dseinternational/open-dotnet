// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Requests;
using DSE.Open.Results;

namespace DSE.Open.Api.Metadata;

public interface IMetadataReader
{
    /// <summary>
    /// Reads metadata from the source key value pairs and sets the corresponding
    /// values in the specified <paramref name="request"/> metadata.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="result"></param>
    /// <param name="source"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask ReadRequestMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        IDictionary<string, string> source,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads metadata from the source key value pairs and sets the corresponding
    /// values in the specified <paramref name="result"/> metadata.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="result"></param>
    /// <param name="source"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask ReadResultMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        IDictionary<string, string> source,
        CancellationToken cancellationToken = default);
}
