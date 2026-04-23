// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Mediators;

/// <summary>
/// Dispatches queries to the single registered
/// <see cref="IQueryHandler{TQuery, TQueryResult}"/> for each query type.
/// </summary>
/// <remarks>
/// This project currently ships the contract only; a concrete implementation is
/// expected to be supplied by the consuming application.
/// </remarks>
public interface IQueryDispatcher
{
    /// <summary>
    /// Resolves the handler for <typeparamref name="TQuery"/> producing
    /// <typeparamref name="TQueryResult"/>, invokes it with <paramref name="query"/>,
    /// and returns its result.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    /// <typeparam name="TQueryResult">The result type produced by the handler.</typeparam>
    /// <param name="query">The query instance to dispatch.</param>
    /// <param name="cancellation">A token that can be used to request cancellation.</param>
    Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation = default);
}
