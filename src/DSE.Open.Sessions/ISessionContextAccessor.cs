// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Sessions;

public interface ISessionContextAccessor
{
    /// <summary>
    /// Gets or sets the current session context.
    /// </summary>
    SessionContext Current { get; set; }
}
