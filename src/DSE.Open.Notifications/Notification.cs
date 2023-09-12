// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
using DSE.Open.Serialization.DataTransfer;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Notifications;

public sealed record Notification : ImmutableDataTransferObject, INotification
{
    private DateTimeOffset _time;

    public Notification(string code, NotificationLevel level, string message)
        : this(new DiagnosticCode(code), level, message)
    {
    }

    [JsonConstructor]
    public Notification(DiagnosticCode code, NotificationLevel level, string message)
    {
        Guard.IsNotNullOrEmpty(message);

        Code = code;
        Level = level;
        Message = message;
        _time = DateTimeOffset.Now.Truncate(DateTimeTruncation.Millisecond);
    }

    [JsonInclude]
    [JsonPropertyName("code")]
    [JsonConverter(typeof(JsonStringDiagnosticCodeConverter))]
    public DiagnosticCode Code { get; }

    [JsonInclude]
    [JsonPropertyName("level")]
        public NotificationLevel Level { get; }

    [JsonInclude]
    [JsonPropertyName("messsage")]
    public string Message { get; }

    [JsonInclude]
    [JsonPropertyName("time")]
    public DateTimeOffset Time { get => _time; init => _time = value.Truncate(DateTimeTruncation.Millisecond); }

    public override string ToString() => $"{Level} ({Code}): {Message}";

    public static Notification Trace(DiagnosticCode code, string message) => new(code, NotificationLevel.Trace, message);

    public static Notification Trace(string code, string message) => new(code, NotificationLevel.Trace, message);

    public static Notification Debug(DiagnosticCode code, string message) => new(code, NotificationLevel.Debug, message);

    public static Notification Debug(string code, string message) => new(code, NotificationLevel.Debug, message);

    public static Notification Information(DiagnosticCode code, string message) => new(code, NotificationLevel.Information, message);

    public static Notification Information(string code, string message) => new(code, NotificationLevel.Information, message);

    public static Notification Error(DiagnosticCode code, string message) => new(code, NotificationLevel.Error, message);

    public static Notification Error(string code, string message) => new(code, NotificationLevel.Error, message);

    public static Notification Critical(DiagnosticCode code, string message) => new(code, NotificationLevel.Critical, message);

    public static Notification Critical(string code, string message) => new(code, NotificationLevel.Critical, message);

    public static Notification Warning(DiagnosticCode code, string message) => new(code, NotificationLevel.Warning, message);

    public static Notification Warning(string code, string message) => new(code, NotificationLevel.Warning, message);
}
