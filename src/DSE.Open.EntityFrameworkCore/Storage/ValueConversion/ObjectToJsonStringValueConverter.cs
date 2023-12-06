// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        ArgumentNullException.ThrowIfNull(model);

        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        return JsonSerializer.Serialize(model, jsonSerializerOptions);
    }

    private static TModel ConvertFrom(string providerValue, JsonSerializerOptions? jsonSerializerOptions)
    {
        ArgumentNullException.ThrowIfNull(providerValue);

        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        return JsonSerializer.Deserialize<TSerialized>(providerValue, jsonSerializerOptions)!;
    }
}
