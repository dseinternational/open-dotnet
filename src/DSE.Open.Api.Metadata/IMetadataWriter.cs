// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

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
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask WriteResultMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        MetadataStorageContext context,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes key value pairs to the specified dictionary representing the data in
    /// the specified <paramref name="result"/> metadata.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="result"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask WriteRequestMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        MetadataStorageContext context,
        CancellationToken cancellationToken = default);
}
