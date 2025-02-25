// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

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
