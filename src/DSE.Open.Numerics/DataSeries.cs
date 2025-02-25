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
    public Vector<TX> X { get; init; }

    [JsonPropertyName("y")]
    public Vector<TY> Y { get; init; }

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

/// <summary>
/// Provides data for one or more series of 2D data points where x-axis values are of type <typeparamref name="TX"/>
/// and y-axis values are of type <typeparamref name="TY"/>.
/// </summary>
/// <typeparam name="TX"></typeparam>
/// <typeparam name="TY"></typeparam>
public abstract class DataSeriesSet<TX, TY>
    where TX : struct, INumber<TX>
    where TY : struct, INumber<TY>
{
    public abstract IEnumerable<DataSeries<TX, TY>> GetDataSeries();

    public virtual IEnumerable<IEnumerable<DataPoint<TX, TY>>> GetDataPoints()
    {
        foreach (var series in GetDataSeries())
        {
            yield return series.CreateDataPoints();
        }
    }
}

/// <summary>
/// Implements a data set for one or more series of 2D data points with data stored
/// in collections of vectors for each axis.
/// </summary>
/// <typeparam name="TX"></typeparam>
/// <typeparam name="TY"></typeparam>
public class VectorDataSeriesSet<TX, TY> : DataSeriesSet<TX, TY>
    where TX : struct, INumber<TX>
    where TY : struct, INumber<TY>
{
    [JsonPropertyName("x")]
    public VectorList<TX> VectorsX { get; init; } = [];

    [JsonPropertyName("y")]
    public VectorList<TY> VectorsY { get; init; } = [];

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
