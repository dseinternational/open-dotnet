// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Sessions;

/// <summary>
/// Carries data relating to a session. A session is a period of time during which
/// a user is actively using an app.
/// </summary>
public sealed class SessionContext
{
    private readonly ConcurrentDictionary<string, string> _storageTokens;

    /// <summary>
    /// Initialises a new instance using <see cref="TimeProvider.System"/>.
    /// </summary>
    public SessionContext() : this(TimeProvider.System)
    {
    }

    /// <summary>
    /// Initialises a new instance using the specified <paramref name="timeProvider"/>
    /// to facilitate testing.
    /// </summary>
    /// <param name="timeProvider"></param>
    public SessionContext(TimeProvider timeProvider)
    {
        Guard.IsNotNull(timeProvider);

        Id = Identifier.New("sess");

        Created = timeProvider.GetUtcNow();

        _storageTokens = new ConcurrentDictionary<string, string>();
    }

    [JsonConstructor]
    public SessionContext(Identifier id, DateTimeOffset created, ConcurrentDictionary<string, string> storageTokens)
    {
        Id = id;
        Created = created;
        _storageTokens = storageTokens;
    }

    /// <summary>
    /// Identifies the session.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    public Identifier Id { get; }

    [JsonPropertyName("storage_tokens")]
    public ConcurrentDictionary<string, string> StorageTokens => _storageTokens;

    /// <summary>
    /// A token that can be used to ensure consistent access to persistent storage.
    /// </summary>
    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; }

    /// <summary>
    /// Merges the data from the specified <paramref name="other"/> session context
    /// into this instance.
    /// </summary>
    /// <param name="other"></param>
    public void Merge(SessionContext? other)
    {
        if (other is null)
        {
            return;
        }

        if (other == this)
        {
            return;
        }

        foreach (var (key, value) in other.StorageTokens)
        {
            _storageTokens[key] = value;
        }
    }
}
