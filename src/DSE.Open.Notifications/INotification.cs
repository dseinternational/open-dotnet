// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics;

namespace DSE.Open.Notifications;

public interface INotification
{
    DiagnosticCode Code { get; }

    NotificationLevel Level { get; }

    string Message { get; }
}
