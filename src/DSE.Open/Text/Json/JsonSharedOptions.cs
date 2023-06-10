// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Text.Json;

public static class JsonSharedOptions
{

    /// <summary>
    /// A shared default instance of <see cref="JsonSerializerOptions"/> configured for serializing
    /// and deserializing DSE.Open types with a snake casing naming policy.
    /// </summary>
    public static readonly JsonSerializerOptions UnicodeRangesAll = Create(encoder: JavaScriptEncoders.UnicodeRangesAll);

    /// <summary>
    /// A shared default instance of <see cref="JsonSerializerOptions"/> configured for serializing
    /// and deserializing DSE.Open types with a snake casing naming policy and with the encoder set to
    /// <see cref="JavaScriptEncoders.RelaxedJsonEscaping"/>.
    /// </summary>
    public static readonly JsonSerializerOptions RelaxedJsonEscaping
        = Create(encoder: JavaScriptEncoders.RelaxedJsonEscaping);

    public static JsonSerializerOptions Create(
        bool writeIndented = false,
        bool addDefaultConverters = true,
        JavaScriptEncoder? encoder = null)
    {
        var options = new JsonSerializerOptions();
        ConfigureJsonOptions(options, writeIndented, addDefaultConverters);
        return options;
    }

    [RequiresUnreferencedCode("Including the JsonValueObjectConverter might require types " +
        "that cannot be statically analyzed.")]
    public static JsonSerializerOptions Create(
        bool writeIndented,
        bool addDefaultConverters,
        bool includeJsonValueObjectConverter)
    {
        var options = new JsonSerializerOptions();

        ConfigureJsonOptions(
            options,
            JsonNamingPolicy.SnakeCaseLower,
            writeIndented,
            addDefaultConverters,
            includeJsonValueObjectConverter,
            null);

        return options;
    }

    public static void ConfigureJsonOptions(
        JsonSerializerOptions options,
        bool writeIndented = false,
        bool addDefaultConverters = true)
        => ConfigureJsonOptions(
            options,
            JsonNamingPolicy.SnakeCaseLower,
            writeIndented,
            addDefaultConverters);

    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Calls ConfigureJsonOptions with includeJsonValueObjectConverter = false")]
    public static void ConfigureJsonOptions(
        JsonSerializerOptions options,
        JsonNamingPolicy commonNamingPolicy,
        bool writeIndented = false,
        bool addDefaultConverters = true)
        => ConfigureJsonOptions(
            options,
            commonNamingPolicy,
            writeIndented,
            addDefaultConverters,
            false,
            null);

    [RequiresUnreferencedCode("Including the JsonValueObjectConverter might require types " +
        "that cannot be statically analyzed.")]
    public static void ConfigureJsonOptions(
        JsonSerializerOptions options,
        JsonNamingPolicy commonNamingPolicy,
        bool writeIndented,
        bool addDefaultConverters,
        bool includeJsonValueObjectConverter,
        JavaScriptEncoder? encoder)
    {
        Guard.IsNotNull(options);
        Guard.IsNotNull(commonNamingPolicy);

        options.Encoder = encoder ?? JavaScriptEncoders.UnicodeRangesAll;

        options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;

        options.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;

        if (addDefaultConverters)
        {
            AddDefaultConverters(options.Converters, includeJsonValueObjectConverter);
        }

        _ = options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

        options.WriteIndented = writeIndented;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Calls AddDefaultConverters with includeJsonValueObjectConverter = false")]
    public static void AddDefaultConverters(IList<JsonConverter> converters)
        => AddDefaultConverters(converters, false);

    [RequiresUnreferencedCode("Including the JsonValueObjectConverter might require types " +
        "that cannot be statically analyzed.")]
    public static void AddDefaultConverters(IList<JsonConverter> converters, bool includeJsonValueObjectConverter)
    {
        Guard.IsNotNull(converters);

        converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));

        converters.AddDseOpenCoreJsonConverters(includeJsonValueObjectConverter);
    }

}
