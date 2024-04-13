// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Entities;

public static class EventRaisingEntityExtensions
{
    public static IEnumerable<IBeforeSaveChangesDomainEvent> GetBeforeSaveChangesEvents(this IEventRaisingEntity entity)
    {
        Guard.IsNotNull(entity);

        if (!entity.HasEvents)
        {
            return [];
        }

        return entity.Events.OfType<IBeforeSaveChangesDomainEvent>();
    }

    public static bool HasBeforeSaveChangesEvents(this IEventRaisingEntity entity)
    {
        Guard.IsNotNull(entity);

        return entity.HasEvents && entity.Events.OfType<IBeforeSaveChangesDomainEvent>().Any();
    }
}
