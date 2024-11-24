// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values.Units;

namespace DSE.Open.Observations;

/// <summary>
/// Defines a location on the surface of the Earth as defined by a <see cref="Longitude"/>
/// and <see cref="Longitude"/> that are accurate to within a specified <see cref="Accuracy"/>.
/// Longitude and latitude are defined in terms of the World Geodetic System 2D coordinate
/// system - WGS 84 (G2139) / EPSG:4326.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly record struct GroundPoint : IRepeatableHash64
{
    public GroundPoint(double latitude, double longitude, Length accuracy)
    {
        Latitude = latitude;
        Longitude = longitude;
        Accuracy = accuracy;
    }

    [JsonPropertyName("latitude")]
    public double Latitude { get; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; }

    [JsonPropertyName("accuracy")]
    public Length Accuracy { get; }

    public ulong GetRepeatableHashCode()
    {
        var h0 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Latitude);
        var h1 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Longitude);
        var h2 = Accuracy.GetRepeatableHashCode();
        return RepeatableHash64Provider.Default.CombineHashCodes(h0, h1, h2);
    }
}
