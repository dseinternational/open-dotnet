// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Sessions;

/// <summary>
/// Provides access to the current <see cref="SessionContext"/>.
/// </summary>
public interface ISessionContextAccessor
{
    /// <summary>
    /// Gets or sets the current session context.
    /// </summary>
    SessionContext Current { get; }
}
