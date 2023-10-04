// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Requests;
using DSE.Open.Results;

namespace DSE.Open.Api.Metadata;

public interface IMetadataReader
{
    ValueTask ReadRequestMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        IDictionary<string, string> source,
        CancellationToken cancellationToken = default);
}
