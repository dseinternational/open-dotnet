// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Turnstile.Web;

/// <summary>
/// Provides the names of the HTML data-* attributes recognised by the Turnstile widget.
/// </summary>
public static class WidgetDataAttributes
{
    /// <summary>The data-sitekey attribute.</summary>
    public const string SiteKey = "data-sitekey";
    /// <summary>The data-action attribute.</summary>
    public const string Action = "data-action";
    /// <summary>The data-cdata attribute used to attach customer data to the challenge.</summary>
    public const string Data = "data-cdata";
    /// <summary>The data-callback attribute that names the JavaScript success callback.</summary>
    public const string Callback = "data-callback";
    /// <summary>The data-execution attribute that controls when a token is obtained.</summary>
    public const string Execution = "data-execution";
    /// <summary>The data-error-callback attribute that names the JavaScript error callback.</summary>
    public const string ErrorCallback = "data-error-callback";
    /// <summary>The data-theme attribute that selects the widget theme.</summary>
    public const string Theme = "data-theme";
    /// <summary>The data-language attribute that selects the widget language.</summary>
    public const string Language = "data-language";
    /// <summary>The data-size attribute that selects the widget size.</summary>
    public const string Size = "data-size";
    /// <summary>The data-appearance attribute that controls widget visibility.</summary>
    public const string Appearance = "data-appearance";
}
