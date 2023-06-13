// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Mediators;

public interface IQueryHandler<in TQuery, TQueryResult>
{
    Task<TQueryResult> HandleAsync(TQuery query, CancellationToken cancellation = default);
}
