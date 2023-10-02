// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Serialization.DataTransfer;
using DSE.Open.Sessions;
using DSE.Open.Values;

namespace DSE.Open.Requests;

/// <summary>
/// Carries the specification for a command or query from a client
/// to a remote system.
/// </summary>
public record Request : ImmutableDataTransferObject
{
    private Identifier? _id;

    private readonly ValueDictionary<string, SessionContext> _sessions = [];

    protected virtual string IdPrefix => "dse_req";

    [JsonPropertyName("request_id")]
    public Identifier RequestId
    {
        get => _id ??= Identifier.New(24, IdPrefix);
        init => _id = value;
    }

    /// <summary>
    /// Gets or initialises the default session context for this request - stored
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
}
