// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Requests;
using DSE.Open.Results;

namespace DSE.Open.Api.Metadata;

public interface IRequestResultMetadataAccessor
{
    RequestMetadata CurrentRequest { get; }

    ResultMetadata CurrentResult { get; }
}
