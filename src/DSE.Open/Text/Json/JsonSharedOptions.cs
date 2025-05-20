// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace DSE.Open.Text.Json;

public static class JsonSharedOptions
{
    private static readonly JsonNamingPolicy s_defaultNamingPolicy = JsonNamingPolicy.SnakeCaseLower;

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
    public static readonly JsonSerializerOptions RelaxedJsonEscaping = Create(encoder: JavaScriptEncoders.RelaxedJsonEscaping);

    public static JsonSerializerOptions Create(
        bool writeIndented = false,
        bool addDefaultConverters = true,
        JavaScriptEncoder? encoder = null)
    {
        var options = new JsonSerializerOptions();

        ConfigureJsonOptions(
            options,
            s_defaultNamingPolicy,
            writeIndented,
            addDefaultConverters,
            includeJsonValueObjectConverter: false,
            encoder);

        return options;
    }

    public static JsonSerializerOptions Create(
        bool writeIndented,
        bool addDefaultConverters,
        bool includeJsonValueObjectConverter)
    {
        var options = new JsonSerializerOptions();

        ConfigureJsonOptions(
            options,
            s_defaultNamingPolicy,
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
    {
        ConfigureJsonOptions(options, s_defaultNamingPolicy, writeIndented, addDefaultConverters);
    }

    public static void ConfigureJsonOptions(
        JsonSerializerOptions options,
        JsonNamingPolicy commonNamingPolicy,
        bool writeIndented = false,
        bool addDefaultConverters = true)
    {
        ConfigureJsonOptions(
            options,
            commonNamingPolicy,
            writeIndented,
            addDefaultConverters,
            false,
            null);
    }

    public static void ConfigureJsonOptions(
        JsonSerializerOptions options,
        JsonNamingPolicy commonNamingPolicy,
        bool writeIndented,
        bool addDefaultConverters,
        bool includeJsonValueObjectConverter,
        JavaScriptEncoder? encoder)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(commonNamingPolicy);

        options.Encoder = encoder ?? JavaScriptEncoders.UnicodeRangesAll;

        options.PropertyNamingPolicy = commonNamingPolicy;
        options.DictionaryKeyPolicy = commonNamingPolicy;

        if (addDefaultConverters)
        {
            options.Converters.AddDseOpenCoreJsonConverters();
        }

        _ = options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

        options.WriteIndented = writeIndented;
        options.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
    }

    /// <summary>
    /// Creates the default JsonSerializerOptions with the specified
    /// <see cref="JsonSerializerContext"/> instances registered as type info resolvers.
    /// </summary>
    /// <param name="contexts"></param>
    public static JsonSerializerOptions CreateWithContexts(
        params ReadOnlySpan<JsonSerializerContext> contexts)
    {
        var options = Create();

        foreach (var context in contexts)
        {
            options.TypeInfoResolverChain.Add(context);
        }

        options.MakeReadOnly();

        return options;
    }
}
