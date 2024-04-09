// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;

namespace DSE.Open.Turnstile.Web;

public sealed class WidgetOptions
{
    /// <summary>
    /// A customer value that can be used to differentiate widgets under the same
    /// sitekey in analytics and which is returned upon validation. This can only
    /// contain up to 32 alphanumeric characters including _ and -.
    /// </summary>
    public string? Action { get; set; }

    /// <summary>
    /// A customer payload that can be used to attach customer data to the
    /// challenge throughout its issuance and which is returned upon validation.
    /// This can only contain up to 255 alphanumeric characters including _ and -.
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// A JavaScript callback invoked upon success of the challenge. The callback is
    /// passed a token that can be validated.
    /// </summary>
    public string? Callback { get; set; }

    /// <summary>
    /// A JavaScript callback invoked when there is an error (e.g. network error
    /// or the challenge failed).
    /// </summary>
    public string? ErrorCallback { get; set; }

    /// <summary>
    /// A JavaScript callback invoked before the challenge enters interactive mode.
    /// </summary>
    public string? BeforeInteractiveCallback { get; set; }

    /// <summary>
    /// A JavaScript callback invoked when challenge has left interactive mode.
    /// </summary>
    public string? AfterInteractiveCallback { get; set; }

    /// <summary>
    /// A JavaScript callback invoked when a given client/browser is not supported
    /// by Turnstile.
    /// </summary>
    public string? UnsupportedCallback { get; set; }

    /// <summary>
    /// Execution controls when to obtain the token of the widget and can be on
    /// render (default) or on execute.
    /// </summary>
    public WidgetExecution Execution { get; set; }

    /// <summary>
    /// The widget theme. Can take the following values: light, dark, auto.
    /// The default is auto, which respects the user preference.
    /// </summary>
    public WidgetTheme Theme { get; set; }

    /// <summary>
    /// Language to display, must be either: auto (default) to use the language
    /// that the visitor has chosen, or an ISO 639-1 two-letter language code
    /// (e.g. en) or language and country code (e.g. en-US). 
    /// </summary>
    public LanguageTag? Language { get; set; }

    /// <summary>
    /// The tabindex of Turnstile’s iframe for accessibility purposes.
    /// The default value is 0.
    /// </summary>
    public int TabIndex { get; set; }

    /// <summary>
    /// A JavaScript callback invoked when the challenge presents an interactive
    /// challenge but was not solved within a given time. A callback will reset
    /// the widget to allow a visitor to solve the challenge again.
    /// </summary>
    public string? TimeoutCallback { get; set; }

    /// <summary>
    /// A boolean that controls if an input element with the response token is
    /// created, defaults to true.
    /// </summary>
    public bool CreateResponseField { get; set; }

    /// <summary>
    /// Name of the input element, defaults to cf-turnstile-response.
    /// </summary>
    public string? ResponseFieldName { get; set; }

    /// <summary>
    /// The widget size. Can take the following values: normal, compact.
    /// </summary>
    public WidgetSize Size { get; set; }

    /// <summary>
    /// Controls whether the widget should automatically retry to obtain a token if
    /// it did not succeed. The default is auto, which will retry automatically.
    /// This can be set to never to disable retry upon failure.
    /// </summary>
    public WidgetRetry Retry { get; set; }

    /// <summary>
    /// When retry is set to auto, retry-interval controls the time between retry.
    /// attempts in milliseconds. Value must be a positive integer less than 900000,
    /// defaults to 8000.
    /// </summary>
    public int RetryInterval { get; set; }

    /// <summary>
    /// Appearance controls when the widget is visible. It can be always (default),
    /// execute, or interaction-only. 
    /// </summary>
    public WidgetAppearance Appearance { get; set; }
}
