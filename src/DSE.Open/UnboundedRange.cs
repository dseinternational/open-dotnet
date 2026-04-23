// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// Specifies a range of comparable values where the lower or upper bound may be unspecified.
/// </summary>
/// <typeparam name="T"></typeparam>
[StructLayout(LayoutKind.Sequential)]
public readonly record struct UnboundedRange<T>
    where T : struct, IComparable<T>
{
    /// <summary>
    /// A range with no lower or upper bound. <see cref="Includes"/> returns <see langword="true"/>
    /// for any value.
    /// </summary>
    public static readonly UnboundedRange<T> Infinite;

    /// <summary>
    /// Initialises a <see cref="UnboundedRange{T}"/> value.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    [JsonConstructor]
    public UnboundedRange(T? start, T? end)
    {
        if (start is not null && end is not null)
        {
            Guard.IsLessThanOrEqualTo(start.Value, end.Value);
        }

        Start = start;
        End = end;
    }

    /// <summary>
    /// The inclusive start of the range of values.
    /// </summary>
    public T? Start { get; }

    /// <summary>
    /// The inclusive end of the range of values.
    /// </summary>
    public T? End { get; }

    /// <summary>
    /// Indicates if the value is within the range specified.
    /// </summary>
    /// <param name="value"></param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is greater that or equal to
    /// <see cref="Start"/> and less than or equal to <see cref="End"/>, otherwise
    /// <see langword="false"/>.</returns>
    public bool Includes(T value)
    {
        if (Start is not null)
        {
            return End is not null
                ? Start.Value.CompareTo(value) <= 0 && End.Value.CompareTo(value) >= 0
                : Start.Value.CompareTo(value) <= 0;
        }

        if (End is not null)
        {
            return End.Value.CompareTo(value) >= 0;
        }

        // Both null

        return true;
    }
}

/// <summary>
/// Factory methods for building <see cref="UnboundedRange{T}"/> values.
/// </summary>
public static class UnboundedRange
{
    /// <summary>
    /// Creates a bounded range <c>[minimum, maximum]</c>.
    /// </summary>
    /// <param name="minimum">The inclusive lower bound.</param>
    /// <param name="maximum">The inclusive upper bound.</param>
    public static UnboundedRange<T> Between<T>(T minimum, T maximum)
        where T : struct, IComparable<T>
    {
        return new(minimum, maximum);
    }

    /// <summary>
    /// Creates an open-ended range <c>[minimum, ∞)</c> that includes every value greater
    /// than or equal to <paramref name="minimum"/>.
    /// </summary>
    /// <param name="minimum">The inclusive lower bound.</param>
    public static UnboundedRange<T> GreaterThanOrEqual<T>(T minimum)
        where T : struct, IComparable<T>
    {
        return new(minimum, default);
    }

    /// <summary>
    /// Creates an open-ended range <c>(-∞, maximum]</c> that includes every value less
    /// than or equal to <paramref name="maximum"/>.
    /// </summary>
    /// <param name="maximum">The inclusive upper bound.</param>
    public static UnboundedRange<T> LessThanOrEqual<T>(T maximum)
        where T : struct, IComparable<T>
    {
        return new(default, maximum);
    }
}

