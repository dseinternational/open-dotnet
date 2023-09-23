// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CommunityToolkit.Diagnostics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Text.Json;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public class ObjectToJsonStringValueConverter<TModel, TSerialized> : ValueConverter<TModel, string>
    where TSerialized : TModel
{
    public static readonly ObjectToJsonStringValueConverter<TModel, TSerialized> Default = new();

    public ObjectToJsonStringValueConverter() : this(null)
    {
    }

    public ObjectToJsonStringValueConverter(JsonSerializerOptions? jsonSerializerOptions)
        : base(v => ConvertTo(v, jsonSerializerOptions), v => ConvertFrom(v, jsonSerializerOptions))
    {
    }

    private static string ConvertTo(TModel model, JsonSerializerOptions? jsonSerializerOptions)
    {
        Guard.IsNotNull(model);

        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        return JsonSerializer.Serialize(model, jsonSerializerOptions);
    }

    private static TModel ConvertFrom(string providerValue, JsonSerializerOptions? jsonSerializerOptions)
    {
        Guard.IsNotNull(providerValue);

        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        return JsonSerializer.Deserialize<TSerialized>(providerValue, jsonSerializerOptions)!;
    }
}
