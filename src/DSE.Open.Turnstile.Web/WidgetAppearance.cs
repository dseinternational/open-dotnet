// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Turnstile.Web;

/// <summary>
/// Controls when the Turnstile widget is visible.
/// </summary>
public enum WidgetAppearance
{
    /// <summary>
    /// The widget is always visible (default).
    /// </summary>
    Always,
    /// <summary>
    /// The widget is shown when execution is triggered.
    /// </summary>
    Execute,
    /// <summary>
    /// The widget is shown only when an interactive challenge is required.
    /// </summary>
    InteractionOnly
}

