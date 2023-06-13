// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Notifications;

public enum NotificationLevel
{
    /// <summary>
    /// Notifications that contain the most detailed messages. These messages may contain sensitive
    /// application data.
    /// </summary>
    Trace = 0,

    /// <summary>
    /// Notifications that are used for interactive investigation during development.
    /// </summary>
    Debug = 1,

    /// <summary>
    /// Notifications that track the general flow of the application. These logs should have long-term value.
    /// </summary>
    Information = 2,

    /// <summary>
    /// Notifications that highlight an abnormal or unexpected event in the application flow, but do not
    /// otherwise cause the application execution to stop.
    /// </summary>
    Warning = 3,

    /// <summary>
    /// Notifications that highlight when the current flow of execution is stopped due to a failure.
    /// These should indicate a failure in the current activity, not an application-wide failure.
    /// </summary>
    Error = 4,

    /// <summary>
    /// Notifications that describe an unrecoverable application or system crash, or a catastrophic
    /// failure that requires immediate attention.
    /// </summary>
    Critical = 5,

    /// <summary>
    /// Not used for reporting notifications. Specifies that a notification should not be reported.
    /// </summary>
    None = 6,
}
