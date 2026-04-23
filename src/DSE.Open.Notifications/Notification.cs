// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Notifications;

/// <summary>
/// An immutable notification carrying a <see cref="DiagnosticCode"/>, a
/// <see cref="NotificationLevel"/> and a human-readable message.
/// </summary>
public sealed record Notification : INotification
{
    /// <summary>
    /// Initialises a new <see cref="Notification"/>. The <paramref name="code"/> string is
    /// parsed as a <see cref="DiagnosticCode"/>.
    /// </summary>
    /// <param name="code">The diagnostic code as a string.</param>
    /// <param name="level">The severity of the notification.</param>
    /// <param name="message">A non-empty human-readable message.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is not a defined
    /// <see cref="NotificationLevel"/>, or <paramref name="code"/> is not a valid <see cref="DiagnosticCode"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="message"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="code"/> or <paramref name="message"/>
    /// is <see langword="null"/>.</exception>
    public Notification(string code, NotificationLevel level, string message)
        : this(new DiagnosticCode(code), level, message)
    {
    }

    /// <summary>
    /// Initialises a new <see cref="Notification"/>.
    /// </summary>
    /// <param name="code">The <see cref="DiagnosticCode"/> identifying the notification.</param>
    /// <param name="level">The severity of the notification.</param>
    /// <param name="message">A non-empty human-readable message.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is not a defined
    /// <see cref="NotificationLevel"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="message"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
    [JsonConstructor]
    public Notification(DiagnosticCode code, NotificationLevel level, string message)
    {
        if (!Enum.IsDefined(level))
        {
            throw new ArgumentOutOfRangeException(nameof(level), level, "Invalid notification level.");
        }

        ArgumentException.ThrowIfNullOrEmpty(message);

        Code = code;
        Level = level;
        Message = message;
    }

    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("code")]
    [JsonConverter(typeof(JsonStringDiagnosticCodeConverter))]
    public DiagnosticCode Code { get; }

    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("level")]
    public NotificationLevel Level { get; }

    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("message")]
    public string Message { get; }

    /// <summary>
    /// Returns a string of the form <c>"{Level} ({Code}): {Message}"</c>.
    /// </summary>
    public override string ToString()
    {
        return $"{Level} ({Code}): {Message}";
    }

    /// <summary>Creates a <see cref="NotificationLevel.Trace"/> notification.</summary>
    public static Notification Trace(DiagnosticCode code, string message)
    {
        return new(code, NotificationLevel.Trace, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Trace"/> notification.</summary>
    public static Notification Trace(string code, string message)
    {
        return new(code, NotificationLevel.Trace, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Debug"/> notification.</summary>
    public static Notification Debug(DiagnosticCode code, string message)
    {
        return new(code, NotificationLevel.Debug, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Debug"/> notification.</summary>
    public static Notification Debug(string code, string message)
    {
        return new(code, NotificationLevel.Debug, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Information"/> notification.</summary>
    public static Notification Information(DiagnosticCode code, string message)
    {
        return new(code, NotificationLevel.Information, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Information"/> notification.</summary>
    public static Notification Information(string code, string message)
    {
        return new(code, NotificationLevel.Information, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Error"/> notification.</summary>
    public static Notification Error(DiagnosticCode code, string message)
    {
        return new(code, NotificationLevel.Error, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Error"/> notification.</summary>
    public static Notification Error(string code, string message)
    {
        return new(code, NotificationLevel.Error, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Critical"/> notification.</summary>
    public static Notification Critical(DiagnosticCode code, string message)
    {
        return new(code, NotificationLevel.Critical, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Critical"/> notification.</summary>
    public static Notification Critical(string code, string message)
    {
        return new(code, NotificationLevel.Critical, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Warning"/> notification.</summary>
    public static Notification Warning(DiagnosticCode code, string message)
    {
        return new(code, NotificationLevel.Warning, message);
    }

    /// <summary>Creates a <see cref="NotificationLevel.Warning"/> notification.</summary>
    public static Notification Warning(string code, string message)
    {
        return new(code, NotificationLevel.Warning, message);
    }
}
