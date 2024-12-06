// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Observations;

public interface ISnapshot
{
    [JsonPropertyName("o")]
    IObservation Observation { get; }

    [JsonPropertyName("t")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    DateTimeOffset Time { get; }

    int GetMeasurementHashCode();
}

public interface ISnapshot<out TObs> : ISnapshot
    where TObs : IObservation
{
    [JsonPropertyName("o")]
    new TObs Observation { get; }
}
