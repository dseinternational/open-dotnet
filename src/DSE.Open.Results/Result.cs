// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Notifications;
using DSE.Open.Serialization.DataTransfer;
using DSE.Open.Sessions;
using DSE.Open.Values;

namespace DSE.Open.Results;

/// <summary>
/// Represents the result of an operation.
/// </summary>
public record Result : ImmutableDataTransferObject
{
    public static readonly Result Empty = new();

    public static readonly Result? Null;

    private Identifier? _id;
    private ReadOnlyValueCollection<Notification>? _notifications;
    private readonly ValueDictionary<string, SessionContext> _sessions = [];

    protected virtual string IdPrefix => "dse_res";

    [JsonPropertyName("result_id")]
    public Identifier ResultId
    {
        get => _id ??= Identifier.New(24, IdPrefix);
        init => _id = value;
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
    /// Gets or initialises the default session context for this result - stored
    /// in <see cref="Sessions"/> with the key "default". For use-cases requiring
    /// multiple session contexts, use <see cref="Sessions"/> directly.
    /// </summary>
    [JsonIgnore]
    public SessionContext? Session
    {
        get
        {
            if (Sessions.TryGetValue("default", out var session))
            {
                return session;
            }

            return null;
        }
        init
        {
            if (value is not null)
            {
                _sessions["default"] = value;
            }
            else
            {
                _ = _sessions.Remove("default");
            }
        }
    }

    [JsonPropertyName("sessions")]
    public IReadOnlyDictionary<string, SessionContext> Sessions
    {
        get => _sessions;
        init => _sessions.AddOrSetRange(value);
    }

    [JsonIgnore]
    public bool HasNotifications => Notifications.Any();

    public bool HasAnyErrorNotifications() => HasNotifications && Notifications.AnyErrors();
}
