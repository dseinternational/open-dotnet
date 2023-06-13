// Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
// Information contained herein is PROPRIETARY AND CONFIDENTIAL.

namespace DSE.Open.Mediators;

public interface IQueryHandler<in TQuery, TQueryResult>
{
    Task<TQueryResult> HandleAsync(TQuery query, CancellationToken cancellation = default);
}
