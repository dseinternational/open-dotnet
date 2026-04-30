// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.ViewModels;

namespace DSE.Open.Turnstile.Web.Views.Shared.Components.TurnstileWidget;

/// <summary>
/// View model for the Turnstile widget view component.
/// </summary>
public sealed class TurnstileWidgetViewModel : UserInterfaceModel
{
    /// <summary>
    /// The Turnstile site key associated with the widget.
    /// </summary>
    public required string SiteKey { get; init; }

    /// <summary>
    /// The options that control the widget's behaviour and appearance.
    /// </summary>
    public required WidgetOptions WidgetOptions { get; init; }

    /// <summary>
    /// Builds the sequence of HTML data-* attribute name/value pairs that should be
    /// rendered on the widget element based on the configured <see cref="WidgetOptions"/>.
    /// </summary>
    /// <returns>The attribute name/value pairs to render.</returns>
    public IEnumerable<(string Name, string Value)> GetAttributes()
    {
        List<(string Name, string Value)> attributes = [];

        attributes.Add((WidgetDataAttributes.SiteKey, SiteKey));

        attributes.Add((WidgetDataAttributes.Language, PresentationCulture.Name));

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
