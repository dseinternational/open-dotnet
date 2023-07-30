// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;

namespace DSE.Open.DomainModel.Abstractions;

public interface IUpdateTimesTracked
{
    DateTimeOffset? Created { get; }

    DateTimeOffset? Updated { get; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    void SetCreated(TimeProvider? timeProvider = null);

    [EditorBrowsable(EditorBrowsableState.Never)]
    void SetUpdated(TimeProvider? timeProvider = null);
}
