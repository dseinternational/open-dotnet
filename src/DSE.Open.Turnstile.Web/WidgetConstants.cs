// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Turnstile.Web;

/// <summary>
/// Provides constant values used when rendering and processing the Turnstile widget.
/// </summary>
public static class WidgetConstants
{
    /// <summary>
    /// The CSS class applied to the Turnstile widget container element.
    /// </summary>
    public const string WidgetClass = "cf-turnstile";

    /// <summary>
    /// The default name of the form field that carries the widget's response token.
    /// </summary>
    public const string DefaultResponseFieldName = "cf-turnstile-response";
}
