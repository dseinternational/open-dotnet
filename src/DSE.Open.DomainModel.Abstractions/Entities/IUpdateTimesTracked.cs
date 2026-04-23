// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An object that tracks when it was created and most recently updated.
/// </summary>
public interface IUpdateTimesTracked
{
    /// <summary>
    /// The time the object was created, or <see langword="null"/> if it has
    /// not been set.
    /// </summary>
    DateTimeOffset? Created { get; }

    /// <summary>
    /// The time the object was most recently updated, or <see langword="null"/>
    /// if it has not been set.
    /// </summary>
    DateTimeOffset? Updated { get; }

    /// <summary>
    /// Stamps the <see cref="Created"/> and <see cref="Updated"/> times using
    /// <paramref name="timeProvider"/>, or the local system clock if
    /// <paramref name="timeProvider"/> is <see langword="null"/>. Intended to
    /// be called by the persistence layer. Throws
    /// <see cref="InvalidOperationException"/> if <see cref="Created"/> has
    /// already been set.
    /// </summary>
    /// <param name="timeProvider">The <see cref="TimeProvider"/> used to read
    /// the current time, or <see langword="null"/> to use the system clock.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void SetCreated(TimeProvider? timeProvider = null);

    /// <summary>
    /// Stamps the <see cref="Updated"/> time using <paramref name="timeProvider"/>,
    /// or the local system clock if <paramref name="timeProvider"/> is
    /// <see langword="null"/>. Intended to be called by the persistence layer.
    /// Throws <see cref="InvalidOperationException"/> if <see cref="Created"/>
    /// has not yet been set.
    /// </summary>
    /// <param name="timeProvider">The <see cref="TimeProvider"/> used to read
    /// the current time, or <see langword="null"/> to use the system clock.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void SetUpdated(TimeProvider? timeProvider = null);
}
