// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.ViewModels;

namespace DSE.Open.Turnstile.Web.Views.Shared.TurnstileWidget;

public sealed class TurnstileWidgetViewModel : LocalizedViewModel
{
    public required string SiteKey { get; init; }

    public required WidgetOptions WidgetOptions { get; init; }

    public IEnumerable<(string Name, string Value)> GetAttributes()
    {
        List<(string Name, string Value)> attributes = [];

        attributes.Add((WidgetDataAttributes.SiteKey, SiteKey));

        attributes.Add((WidgetDataAttributes.Language, Language.ToStringInvariant()));

        if (!string.IsNullOrWhiteSpace(WidgetOptions.Action))
        {
            attributes.Add((WidgetDataAttributes.Action, WidgetOptions.Action));
        }

        if (!string.IsNullOrWhiteSpace(WidgetOptions.Data))
        {
            attributes.Add((WidgetDataAttributes.Data, WidgetOptions.Data));
        }

        if (WidgetOptions.Execution == WidgetExecution.Execute)
        {
            attributes.Add((WidgetDataAttributes.Execution, "execute"));
        }

        if (WidgetOptions.Theme == WidgetTheme.Light)
        {
            attributes.Add((WidgetDataAttributes.Theme, "light"));
        }
        else if (WidgetOptions.Theme == WidgetTheme.Dark)
        {
            attributes.Add((WidgetDataAttributes.Theme, "dark"));
        }

        if (WidgetOptions.Size == WidgetSize.Compact)
        {
            attributes.Add((WidgetDataAttributes.Size, "compact"));
        }

        // TODO: complete

        return attributes;
    }
}
