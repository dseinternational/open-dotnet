// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics;

namespace DSE.Open.Numerics;

internal sealed class SeriesLabelEnumerable<T> : IEnumerable<string>
    where T : IEquatable<T>
{
    private readonly IReadOnlySeries<T> _series;

    public SeriesLabelEnumerable(IReadOnlySeries<T> series)
    {
        Debug.Assert(series is not null);
        _series = series;
    }

    public IEnumerator<string> GetEnumerator()
    {
        if (_series.DataLabels.Count > 0)
        {
            foreach (var value in _series)
            {
                if (_series.DataLabels.TryGetLabel(value, out var label))
                {
                    yield return label;
                }
                else
                {
                    yield return value?.ToString() ?? "NA";
                }
            }
        }
        else
        {
            foreach (var item in _series)
            {
                yield return item?.ToString() ?? "NA";
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
