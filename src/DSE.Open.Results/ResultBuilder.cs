// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Notifications;

namespace DSE.Open.Results;

/// <summary>
/// Base class for building instances of <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of <see cref="Result"/> produced by this builder.</typeparam>
public abstract class ResultBuilder<TResult>
    where TResult : Result
{
    /// <summary>
    /// Gets or sets the status that will be assigned to the built result.
    /// </summary>
    public virtual ResultStatus Status { get; set; }

    /// <summary>
    /// Gets the collection of notifications that will be assigned to the built result.
    /// </summary>
    public ICollection<Notification> Notifications { get; } = [];

    /// <summary>
    /// Copies the status (when not yet set) and notifications from <paramref name="result"/>
    /// into this builder.
    /// </summary>
    public virtual void MergeNotifications(Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (Status == ResultStatus.Unspecified && result.Status != ResultStatus.Unspecified)
        {
            Status = result.Status;
        }

        if (result.HasNotifications)
        {
            Notifications.AddRange(result.Notifications);
        }
    }

    /// <summary>
    /// Creates a new <typeparamref name="TResult"/> from the current state of the builder.
    /// </summary>
    public abstract TResult Build();
}

/// <summary>
/// Builds a plain <see cref="Result"/>.
/// </summary>
public class ResultBuilder : ResultBuilder<Result>
{
    /// <inheritdoc />
    public override Result Build()
    {
        return new()
        {
            Status = Status,
            Notifications = [.. Notifications],
        };
    }
}
