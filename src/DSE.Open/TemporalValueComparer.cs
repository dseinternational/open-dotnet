// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public abstract class TemporalValueComparer<T> : IComparer<TemporalValue<T>>
{
    /// <summary>
    /// A comparer that compares the times of two <see cref="TemporalValue{T}"/> instances.
    /// </summary>
    public static readonly IComparer<TemporalValue<T>> Time = new TemporalValueTimeComparer();

    /// <summary>
    /// A comparer that compares the values of two <see cref="TemporalValue{T}"/> instances
    /// using <see cref="Comparer{T}.Default"/>.
    /// </summary>
    public static readonly IComparer<TemporalValue<T>> Value = new TemporalValueValueComparer();

    public abstract int Compare(TemporalValue<T> x, TemporalValue<T> y);

    private sealed class TemporalValueTimeComparer : TemporalValueComparer<T>
    {
        public override int Compare(TemporalValue<T> x, TemporalValue<T> y)
        {
            return x.Time.CompareTo(y.Time);
        }
    }

    private sealed class TemporalValueValueComparer : TemporalValueComparer<T>
    {
        public override int Compare(TemporalValue<T> x, TemporalValue<T> y)
        {
            return Comparer<T>.Default.Compare(x.Value, y.Value);
        }
    }
}

public sealed class ComparableTemporalValueComparer<T> : IComparer<TemporalValue<T>>
    where T : IComparable<T>
{
    /// <summary>
    /// A comparer that compares the values of two <see cref="TemporalValue{T}"/> instances
    /// using their implementation of <see cref="IComparable{T}"/>.
    /// </summary>
    public static readonly IComparer<TemporalValue<T>> Default = new ComparableTemporalValueComparer<T>();

    public int Compare(TemporalValue<T> x, TemporalValue<T> y)
    {
        return x.Value.CompareTo(y.Value);
    }
}

public sealed class StringTemporalValueComparer : IComparer<TemporalValue<string>>
{
    public static readonly IComparer<TemporalValue<string>> Ordinal = new StringTemporalValueComparer(StringComparer.Ordinal);

    public static readonly IComparer<TemporalValue<string>> OrdinalIgnoreCase = new StringTemporalValueComparer(StringComparer.OrdinalIgnoreCase);

    private readonly StringComparer _comparer;

    public StringTemporalValueComparer(StringComparer comparer)
    {
        _comparer = comparer;
    }

    public int Compare(TemporalValue<string> x, TemporalValue<string> y)
    {
        return _comparer.Compare(x.Value, y.Value);
    }
}
