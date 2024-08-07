// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DSE.Open.Turnstile.Web.Views.Shared.Components.TurnstileWidget;

public sealed class TurnstileWidgetViewComponent : ViewComponent
{
    private readonly TurnstileClientOptions _clientOptions;

    public TurnstileWidgetViewComponent(IOptions<TurnstileClientOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.Value);

        _clientOptions = options.Value;
    }

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
