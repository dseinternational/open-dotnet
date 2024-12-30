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
    public static readonly Result Empty = new();

    public static readonly Result? Null;

    private Guid? _resultId;
    private ReadOnlyValueCollection<Notification>? _notifications;

    [JsonPropertyName("status")]
    public ResultStatus Status { get; init; }

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

    [JsonIgnore]
    public bool HasNotifications => Notifications.Count != 0;

    public bool HasAnyErrorNotifications()
    {
        return HasNotifications && Notifications.AnyErrors();
    }
}
