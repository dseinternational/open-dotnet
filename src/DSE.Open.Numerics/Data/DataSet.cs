// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics.Data;

/// <summary>
/// A set of data frames (<see cref="DataFrame"/>).
/// </summary>
public class DataSet
{
    [JsonPropertyName("frames")]
    public Collection<DataFrame> DataFrames { get; init; } = [];
}
