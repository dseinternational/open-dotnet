// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Speech;
using DSE.Open.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Speech.Storage.ValueConversion;

[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public sealed class TranscriptionListToJsonStringConverter : ValueConverter<IList<Transcription>, string>
{
    public static readonly TranscriptionListToJsonStringConverter Default = new();

    public TranscriptionListToJsonStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static string ConvertTo(IList<Transcription> value)
    {
        return JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
    }

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
    private static IList<Transcription> ConvertFrom(string value)
#pragma warning restore CA1859 // Use concrete types when possible for improved performance
    {
        return JsonSerializer.Deserialize<List<Transcription>>(value, JsonSharedOptions.RelaxedJsonEscaping)!;
    }
}
