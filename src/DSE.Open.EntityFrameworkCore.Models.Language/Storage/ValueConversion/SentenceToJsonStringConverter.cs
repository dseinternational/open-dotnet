// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Language.Annotations;
using DSE.Open.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="Sentence"/> values to and from a JSON <see cref="string"/>.
/// </summary>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public sealed class SentenceToJsonStringConverter : ValueConverter<Sentence, string>
{
    /// <summary>
    /// Gets the default <see cref="SentenceToJsonStringConverter"/> instance.
    /// </summary>
    public static readonly SentenceToJsonStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="SentenceToJsonStringConverter"/> class.
    /// </summary>
    public SentenceToJsonStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Serialises a <see cref="Sentence"/> to a JSON string.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(Sentence value)
    {
        return JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
    }

    /// <summary>
    /// Deserialises a <see cref="Sentence"/> from a JSON string.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static Sentence ConvertFrom(string value)
    {
        return JsonSerializer.Deserialize<Sentence>(value, JsonSharedOptions.RelaxedJsonEscaping)!;
    }
}
