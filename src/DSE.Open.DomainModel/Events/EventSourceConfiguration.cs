// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Events;

/// <summary>
/// Provides a source for events raised in the current application process.
/// </summary>
public static class EventSourceConfiguration
{
    private static readonly Uri s_default = new("https://dseapi.app/event-source");
    private static Func<Type, Uri> s_provider = DefaultProvider;

    public static Func<Type, Uri> SourceProvider
    {
        get => s_provider;
        set
        {
            Guard.IsNotNull(value);
            s_provider = value;
        }
    }

    public static Uri GetEventSource(Type requestingType)
    {
        return SourceProvider(requestingType);
    }

    private static Uri DefaultProvider(Type requestingType)
    {

        if (Uri.TryCreate(s_default, requestingType.FullName ?? requestingType.Name, out var uri))
        {
            return uri;
        }

        return s_default;
    }
}
