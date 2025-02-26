// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Provides data for a series of 2D data points where x-axis values are of type <typeparamref name="TX"/>
/// and y-axis values are of type <typeparamref name="TY"/>.
/// </summary>
/// <typeparam name="TX"></typeparam>
/// <typeparam name="TY"></typeparam>
public class DataSeries<TX, TY>
    where TX : struct, INumber<TX>
    where TY : struct, INumber<TY>
{
    [JsonPropertyName("x")]
    public required NumericVector<TX> X { get; init; }

    [JsonPropertyName("y")]
    public required NumericVector<TY> Y { get; init; }

    public IEnumerable<DataPoint<TX, TY>> CreateDataPoints()
    {
        if (X.Length == 0)
        {
            if (Y.Length == 0)
            {
                yield break;
            }

            TX x = default;
            for (var i = 0; i < Y.Length; i++)
            {
                x += TX.One;
                yield return new DataPoint<TX, TY>(x, Y.Span[i]);
            }
        }

        for (var i = 0; i < X.Length; i++)
        {
            if (i >= Y.Length)
            {
                yield break;
            }

            yield return new DataPoint<TX, TY>(X.Span[i], Y.Span[i]);
        }
    }
}
