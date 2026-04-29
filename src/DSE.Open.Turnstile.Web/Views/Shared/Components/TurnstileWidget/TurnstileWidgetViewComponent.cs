// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DSE.Open.Turnstile.Web.Views.Shared.Components.TurnstileWidget;

/// <summary>
/// A view component that renders the Cloudflare Turnstile widget.
/// </summary>
public sealed class TurnstileWidgetViewComponent : ViewComponent
{
    private readonly TurnstileClientOptions _clientOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="TurnstileWidgetViewComponent"/> class.
    /// </summary>
    /// <param name="options">The configured Turnstile client options that supply the site key.</param>
    public TurnstileWidgetViewComponent(IOptions<TurnstileClientOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.Value);

        _clientOptions = options.Value;
    }

    /// <summary>
    /// Renders the Turnstile widget using the specified <paramref name="options"/>, or
    /// <see cref="WidgetOptions.Default"/> if none are supplied.
    /// </summary>
    /// <param name="options">Optional widget options that override the defaults.</param>
    /// <returns>A <see cref="IViewComponentResult"/> that renders the widget.</returns>
    public IViewComponentResult Invoke(WidgetOptions? options = null)
    {
        var model = new TurnstileWidgetViewModel
        {
            SiteKey = _clientOptions.SiteKey,
            WidgetOptions = options ?? WidgetOptions.Default
        };

        return View("~/Views/Shared/Components/TurnstileWidget/Default.cshtml", model);
    }
}
