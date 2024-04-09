// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Turnstile.Web;

internal static class WarningMessages
{
    public const string RequiresDynamicCode =
        "JSON serialization and deserialization might require types that cannot be statically " +
        "analyzed and might need runtime code generation. Use System.Text.Json source generation for " +
        "native AOT applications.";

    public const string RequiresUnreferencedCode =
        "JSON serialization and deserialization might require types that cannot be " +
        "statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make " +
        "sure all of the required types are preserved.";
}

