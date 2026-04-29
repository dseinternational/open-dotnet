// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Speech;
using DSE.Open.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Speech.Storage.ValueConversion;

/// <summary>
/// Converts a list of <see cref="SpeechTranscription"/> values to and from a JSON
/// <see cref="string"/> representation for storage by Entity Framework Core.
/// </summary>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public sealed class SpeechTranscriptionListToJsonStringConverter : ValueConverter<IList<SpeechTranscription>, string>
{
    /// <summary>
    /// A default, shared instance of <see cref="SpeechTranscriptionListToJsonStringConverter"/>.
    /// </summary>
    public static readonly SpeechTranscriptionListToJsonStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="SpeechTranscriptionListToJsonStringConverter"/> class.
    /// </summary>
    public SpeechTranscriptionListToJsonStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Serializes the specified list of <see cref="SpeechTranscription"/> values to a JSON string.
    /// </summary>
    /// <param name="value">The list to serialize.</param>
    /// <returns>The JSON representation of <paramref name="value"/>.</returns>
    // public for EF Core model compilation
    public static string ConvertTo(IList<SpeechTranscription> value)
    {
        return JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
    }

    /// <summary>
    /// Deserializes the specified JSON string into a list of <see cref="SpeechTranscription"/> values.
    /// </summary>
    /// <param name="value">The JSON string to deserialize.</param>
    /// <returns>The deserialized list of <see cref="SpeechTranscription"/> values.</returns>
    // public for EF Core model compilation
    public static IList<SpeechTranscription> ConvertFrom(string value)
    {
        return JsonSerializer.Deserialize<List<SpeechTranscription>>(value, JsonSharedOptions.RelaxedJsonEscaping)!;
    }
}
