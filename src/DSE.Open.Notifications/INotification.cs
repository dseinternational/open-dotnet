// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics;

namespace DSE.Open.Notifications;

/// <summary>
/// Represents a notification produced by application code to communicate a
/// diagnostic message, optionally accompanied by a <see cref="DiagnosticCode"/>
/// and a <see cref="NotificationLevel"/> severity.
/// </summary>
public interface INotification
{
    /// <summary>
    /// Gets the <see cref="DiagnosticCode"/> that identifies the kind of notification.
    /// </summary>
    DiagnosticCode Code { get; }

    /// <summary>
    /// Gets the <see cref="NotificationLevel"/> that indicates the severity of the notification.
    /// </summary>
    NotificationLevel Level { get; }

    /// <summary>
    /// Gets the human-readable message describing the notification.
    /// </summary>
    string Message { get; }
}
