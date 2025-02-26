// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Implements a data set for one or more series of 2D data points with data stored
/// in collections of vectors for each axis.
/// </summary>
/// <typeparam name="TX"></typeparam>
/// <typeparam name="TY"></typeparam>
public class NumericVectorDataSeriesSet<TX, TY> : DataSeriesSet<TX, TY>
    where TX : struct, INumber<TX>
    where TY : struct, INumber<TY>
{
    [JsonPropertyName("x")]
    public NumericVectorList<TX> VectorsX { get; init; } = [];

    [JsonPropertyName("y")]
    public NumericVectorList<TY> VectorsY { get; init; } = [];

    public override IEnumerable<DataSeries<TX, TY>> GetDataSeries()
    {
        if (VectorsX.Count == 0 || VectorsY.Count == 0)
        {
            yield break;
        }

        if (VectorsX.Count == VectorsY.Count)
        {
            for (var i = 0; i < VectorsX.Count; i++)
            {
                yield return new DataSeries<TX, TY>
                {
                    X = VectorsX[i],
                    Y = VectorsY[i]
                };
            }
        }
        else
        {
            for (var i = 0; i < VectorsY.Count; i++)
            {
                yield return new DataSeries<TX, TY>
                {
                    X = VectorsX[0],
                    Y = VectorsY[i]
                };
            }
        }
    }
}
