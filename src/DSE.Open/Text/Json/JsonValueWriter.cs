// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Text.Json;

/// <summary>
/// Encapsulates a method that writes a value of type <typeparamref name="T"/> to a
/// <see cref="Utf8JsonWriter"/>.
/// </summary>
public delegate void JsonValueWriter<T>(Utf8JsonWriter writer, T value);
