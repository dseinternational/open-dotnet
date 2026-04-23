// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Combines <see cref="IUpdateTimesTracked"/> with <see cref="ITimestamped"/> —
/// an object that tracks created/updated times and carries a concurrency
/// <see cref="Timestamp"/>.
/// </summary>
public interface IUpdatesTracked : IUpdateTimesTracked, ITimestamped;
