// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Mediators;

/// <summary>
/// Handles a query of type <typeparamref name="TQuery"/> and returns a
/// <typeparamref name="TQueryResult"/>. Exactly one implementation is expected
/// per query type.
/// </summary>
/// <typeparam name="TQuery">The query type handled by this handler.</typeparam>
/// <typeparam name="TQueryResult">The result type produced by this handler.</typeparam>
public interface IQueryHandler<in TQuery, TQueryResult>
{
    /// <summary>
    /// Handles <paramref name="query"/> and returns the produced result.
    /// </summary>
    /// <param name="query">The query to handle.</param>
    /// <param name="cancellation">A token that can be used to request cancellation.</param>
    Task<TQueryResult> HandleAsync(TQuery query, CancellationToken cancellation = default);
}
