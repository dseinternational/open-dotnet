// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Text.Json;

/// <summary>
/// Encapsulates a method that reads a value of type <typeparamref name="T"/> from a
/// <see cref="Utf8JsonReader"/>.
/// </summary>
public delegate T JsonValueReader<T>(ref Utf8JsonReader reader);
