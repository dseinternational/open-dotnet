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

    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static string ConvertTo(TModel model, JsonSerializerOptions? jsonSerializerOptions)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        ArgumentNullException.ThrowIfNull(model);

        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        return JsonSerializer.Serialize(model, jsonSerializerOptions);
    }

    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TModel ConvertFrom(string providerValue,
#pragma warning restore CA1000 // Do not declare static members on generic types
                                     JsonSerializerOptions? jsonSerializerOptions)
    {
        ArgumentNullException.ThrowIfNull(providerValue);

        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        return JsonSerializer.Deserialize<TSerialized>(providerValue, jsonSerializerOptions)!;
    }
}
