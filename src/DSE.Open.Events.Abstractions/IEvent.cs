// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Events;

/// <summary>
/// A common representation of an event, based on the CloudEvents specification.
/// </summary>
/// <remarks>
/// This provides a common representation to be shared by domain events and integration events.
/// <para>An "event" is a data record expressing an occurrence and its context. Events are routed
/// from an event producer (the source) to interested event consumers. The routing can be
/// performed based on information contained in the event, but an event will not identify a
/// specific routing destination. Events will contain two types of information: the Event Data
/// representing the Occurrence and Context metadata providing contextual information about the
/// Occurrence. A single occurrence MAY result in more than one event.</para>
/// </remarks>
/// <list type="bullet">
/// <item><seealso href="https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md"/></item>
/// </list>
public interface IEvent
{
    /// <summary>
    /// The event payload.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item><seealso href="https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md"/></item>
    /// </list>
    /// </remarks>
    object? Data { get; }

    /// <summary>
    /// Identifies the event. 
    /// </summary>
    /// <remarks>
    /// Producers must ensure that source + id is unique for each distinct event. If a duplicate
    /// event is re-sent (e.g. due to a network error) it may have the same id. Consumers may
    /// assume that Events with identical source and id are duplicates.
    /// <para>This is required and must be unique within the scope of the producer.</para>
    /// <list type="bullet">
    /// <item><seealso href="https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md"/></item>
    /// </list>
    /// </remarks>
    Identifier Id { get; }

    /// <summary>
    /// Identifies the context in which an event happened.
    /// </summary>
    /// <remarks>
    /// Often this will include information such as the type of the event source, the organization
    /// publishing the event or the process that produced the event. The exact syntax and semantics
    /// behind the data encoded in the URI is defined by the event producer. Producers must ensure
    /// that source + id is unique for each distinct event.
    /// <para>This is required and must be a URI-reference. An absolute URI is recommended.</para>
    /// <list type="bullet">
    /// <item><seealso href="https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md"/></item>
    /// </list>
    /// </remarks>
    Uri Source { get; }

    /// <summary>
    /// This describes the subject of the event in the context of the event producer (identified
    /// by <see cref="Source"/>).
    /// </summary>
    /// <remarks>
    /// In publish-subscribe scenarios, a subscriber will typically subscribe to events emitted
    /// by a source, but the source identifier alone might not be sufficient as a qualifier for
    /// any specific event if the source context has internal sub-structure.
    /// <list type="bullet">
    /// <item><seealso href="https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md"/></item>
    /// </list>
    /// </remarks>
    string? Subject { get; }

    /// <summary>
    /// DSE.Open.Timestamp of when the occurrence happened.
    /// </summary>
    /// <remarks>
    /// If the time of the occurrence cannot be determined then this attribute may be set to some
    /// other time (such as the current time) by the producer, however all producers for the same
    /// source must be consistent in this respect.
    /// <para>Although not required by CloudEvents specification, we require this.</para>
    /// <list type="bullet">
    /// <item><seealso href="https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md"/></item>
    /// </list>
    /// </remarks>
    DateTimeOffset Time { get; }

    /// <summary>
    /// This attribute contains a value describing the type of event related to the originating occurrence.
    /// </summary>
    /// <remarks>
    /// Often this attribute is used for routing, observability, policy enforcement, etc. The format of
    /// this is producer defined and might include information such as the version of the type.
    /// <para>This is required and should be prefixed with a reverse-DNS name. The prefixed domain
    /// dictates the organization which defines the semantics of this event type.</para>
    /// <list type="bullet">
    /// <item><seealso href="https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md"/></item>
    /// </list>
    /// </remarks>
    string Type { get; }
}

/// <inheritdoc />
/// <typeparam name="TData">The type of the event payload.</typeparam>
public interface IEvent<TData> : IEvent
{
    /// <summary>
    /// The event payload.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item><seealso href="https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md"/></item>
    /// </list>
    /// </remarks>
    new TData? Data { get; }
}
