// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;
using DSE.Open.Requests;
using DSE.Open.Results;

namespace DSE.Open.Api.Metadata;

public interface IMetadataWriter
{
    /// <summary>
    /// Writes key value pairs to the specified dictionary representing the data in
    /// the specified <paramref name="request"/> metadata.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="result"></param>
    /// <param name="target"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask WriteResultMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        ConcurrentDictionary<string, string> target,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes key value pairs to the specified dictionary representing the data in
    /// the specified <paramref name="result"/> metadata.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="result"></param>
    /// <param name="target"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask WriteRequestMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        ConcurrentDictionary<string, string> target,
        CancellationToken cancellationToken = default);
}
