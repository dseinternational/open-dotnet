// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Notifications;

namespace DSE.Open.Results;

/// <summary>
/// Represents the result of an operation.
/// </summary>
public record Result
{
    /// <summary>
    /// An empty <see cref="Result"/> with default <see cref="Status"/> and no notifications.
    /// </summary>
    public static readonly Result Empty = new();

    /// <summary>
    /// A <see langword="null"/> <see cref="Result"/> reference.
    /// </summary>
    public static readonly Result? Null;

    private Guid? _resultId;
    private ReadOnlyValueCollection<Notification>? _notifications;

    /// <summary>
    /// Gets the status of the operation that produced this result.
    /// </summary>
    [JsonPropertyName("status")]
    public ResultStatus Status { get; init; }

    /// <summary>
    /// Gets the unique identifier of this result. A new identifier is generated on first access
    /// if one has not been explicitly set.
    /// </summary>
    [JsonPropertyName("result_id")]
    public Guid ResultId
    {
        get => _resultId ??= Guid.NewGuid();
        init => _resultId = value;
    }

    /// <summary>
    /// Gets notifications connected with the result of the operation.
    /// </summary>
    [JsonPropertyName("notifications")]
    public ReadOnlyValueCollection<Notification> Notifications
    {
        get => _notifications ??= [];
        init => _notifications = value;
    }

    /// <summary>
    /// Gets a value indicating whether this result carries any notifications.
    /// </summary>
    [JsonIgnore]
    public bool HasNotifications => Notifications.Count != 0;

    /// <summary>
    /// Returns <see langword="true"/> if this result carries any error-level notifications.
    /// </summary>
    public bool HasAnyErrorNotifications()
    {
        return HasNotifications && Notifications.AnyErrors();
    }
}
