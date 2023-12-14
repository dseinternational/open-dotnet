// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Language.Annotations;
using DSE.Open.Text.Json;
using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public sealed class SentenceToJsonStringConverter : ValueConverter<Sentence, string>
{
    public static readonly SentenceToJsonStringConverter Default = new();

    public SentenceToJsonStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static string ConvertTo(Sentence value)
    {
        return JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
    }

    private static Sentence ConvertFrom(string value)
    {
        return JsonSerializer.Deserialize<Sentence>(value, JsonSharedOptions.RelaxedJsonEscaping)!;
    }
}
