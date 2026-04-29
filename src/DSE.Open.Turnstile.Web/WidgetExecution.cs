// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Turnstile.Web;

/// <summary>
/// Controls when the Turnstile widget obtains a token.
/// </summary>
public enum WidgetExecution
{
    /// <summary>
    /// The token is obtained when the widget is rendered (default).
    /// </summary>
    Render,
    /// <summary>
    /// The token is obtained when execution is explicitly triggered.
    /// </summary>
    Execute
}
