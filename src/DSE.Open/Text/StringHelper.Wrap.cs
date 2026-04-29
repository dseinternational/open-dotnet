// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DSE.Open.Linq;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Text;

public static partial class StringHelper
{
    /// <summary>
    /// Returns <paramref name="value"/> with <paramref name="wrapper"/> placed before and after it.
    /// </summary>
    public static string Wrap(ReadOnlySpan<char> wrapper, ReadOnlySpan<char> value)
    {
        return Wrap(wrapper, wrapper, value);
    }

    /// <summary>
    /// Returns <paramref name="value"/> with <paramref name="before"/> prepended and <paramref name="after"/> appended.
    /// </summary>
    public static string Wrap(ReadOnlySpan<char> before, ReadOnlySpan<char> after, ReadOnlySpan<char> value)
    {
        return $"{before}{value}{after}";
    }

    /// <summary>
    /// Returns <paramref name="value"/> with <paramref name="wrapper"/> placed before and after it.
    /// </summary>
    public static string Wrap(string wrapper, string value)
    {
        return Wrap(wrapper, wrapper, value);
    }

    /// <summary>
    /// Returns <paramref name="value"/> with <paramref name="before"/> prepended and <paramref name="after"/> appended.
    /// </summary>
    public static string Wrap(string before, string after, string value)
    {
        return $"{before}{value}{after}";
    }

    /// <summary>
    /// Formats <paramref name="value"/> and returns the result with <paramref name="wrapper"/>
    /// placed before and after it.
    /// </summary>
    public static string Wrap<T>(char wrapper, T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
    {
        return Wrap(wrapper, wrapper, value, format, provider);
    }

    /// <summary>
    /// Formats <paramref name="value"/> and returns the result with <paramref name="before"/>
    /// prepended and <paramref name="after"/> appended.
    /// </summary>
    [SkipLocalsInit]
    public static string Wrap<T>(char before, char after, T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
    {
        var sh = new DefaultInterpolatedStringHandler(2, 3, provider, stackalloc char[MemoryThresholds.StackallocCharThreshold]);
        sh.AppendFormatted(before, null);
        sh.AppendFormatted(value, format.ToString()); // TODO: ??
        sh.AppendFormatted(after, null);
        return sh.ToStringAndClear();
    }

    /// <summary>
    /// Formats <paramref name="value"/> and returns the result with <paramref name="wrapper"/>
    /// placed before and after it.
    /// </summary>
    public static string Wrap<T>(string wrapper, T value, string? format = default, IFormatProvider? provider = default)
    {
        return Wrap(wrapper, wrapper, value, format, provider);
    }

    /// <summary>
    /// Formats <paramref name="value"/> and returns the result with <paramref name="before"/>
    /// prepended and <paramref name="after"/> appended.
    /// </summary>
    [SkipLocalsInit]
    public static string Wrap<T>(string before, string after, T value, string? format = default, IFormatProvider? provider = default)
    {
        ArgumentNullException.ThrowIfNull(before);
        ArgumentNullException.ThrowIfNull(after);

        var sh = new DefaultInterpolatedStringHandler(before.Length + after.Length, 1, provider, stackalloc char[MemoryThresholds.StackallocCharThreshold]);
        sh.AppendLiteral(before);
        sh.AppendFormatted(value, format);
        sh.AppendLiteral(after);
        return sh.ToStringAndClear();
    }

    /// <summary>
    /// Formats <paramref name="value"/> and returns the result with <paramref name="wrapper"/>
    /// placed before and after it.
    /// </summary>
    public static string Wrap<T>(
        ReadOnlySpan<char> wrapper,
        T value,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
        where T : struct, IFormattable
    {
        return Wrap(wrapper, wrapper, value, format, provider);
    }

    /// <summary>
    /// Formats <paramref name="value"/> and returns the result with <paramref name="before"/>
    /// prepended and <paramref name="after"/> appended.
    /// </summary>
    [SkipLocalsInit]
    public static string Wrap<T>(
        ReadOnlySpan<char> before,
        ReadOnlySpan<char> after,
        T value,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
        where T : struct, IFormattable
    {
        var sh = new DefaultInterpolatedStringHandler(before.Length + after.Length, 1,
            provider, stackalloc char[MemoryThresholds.StackallocCharThreshold]);
        sh.AppendFormatted(before);
        sh.AppendFormatted(value, format.ToString()); // TODO: isn't this avoidable?
        sh.AppendFormatted(after);
        return sh.ToStringAndClear();
    }

    /// <summary>
    /// Wraps each string in <paramref name="values"/> with <paramref name="before"/> and
    /// <paramref name="after"/>.
    /// </summary>
    public static IEnumerable<string> WrapRange(
        ReadOnlySpan<char> before,
        ReadOnlySpan<char> after,
        IEnumerable<string> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (values is List<string> valuesList)
        {
            return WrapRangeCore(before, after, CollectionsMarshal.AsSpan(valuesList));
        }

        if (values is string[] valuesArray)
        {
            return WrapRangeCore(before, after, new(valuesArray));
        }

        var initialCapacity = values is ICollection<string> valuesCollection
            ? valuesCollection.Count : 8;

        var results = new List<string>(initialCapacity);

        foreach (var v in values)
        {
            results.Add(Wrap(before, after, v));
        }

        return results;
    }

    /// <summary>
    /// Wraps the formatted representation of each value in <paramref name="values"/> with the
    /// <paramref name="before"/> and <paramref name="after"/> characters.
    /// </summary>
    public static IEnumerable<string> WrapRange<T>(
        char before,
        char after,
        IEnumerable<T> values,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
        where T : struct, IFormattable
    {
        ArgumentNullException.ThrowIfNull(values);

        if (values.TryGetSpan(out var valuesSpan))
        {
            return WrapRange(before, after, valuesSpan, format, provider);
        }

        var initialCapacity = values is ICollection<string> valuesCollection
            ? valuesCollection.Count : 8;

        var results = new List<string>(initialCapacity);

        foreach (var v in values)
        {
            results.Add(Wrap(before, after, v, format, provider));
        }

        return results;
    }

    /// <summary>
    /// Wraps the formatted representation of each value in <paramref name="values"/> with the
    /// <paramref name="before"/> and <paramref name="after"/> spans.
    /// </summary>
    public static IEnumerable<string> WrapRange<T>(
        ReadOnlySpan<char> before,
        ReadOnlySpan<char> after,
        IEnumerable<T> values,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
        where T : struct, IFormattable
    {
        ArgumentNullException.ThrowIfNull(values);

        if (values.TryGetSpan(out var valuesSpan))
        {
            return WrapRange(before, after, valuesSpan, format, provider);
        }

        var initialCapacity = values is ICollection<T> valuesCollection
            ? valuesCollection.Count : 8;

        var results = new List<string>(initialCapacity);

        foreach (var v in values)
        {
            results.Add(Wrap(before, after, v, format, provider));
        }

        return results;
    }

    /// <summary>
    /// Wraps the formatted representation of each value in <paramref name="values"/> with the
    /// <paramref name="before"/> and <paramref name="after"/> characters.
    /// </summary>
    public static IEnumerable<string> WrapRange<T>(
        char before,
        char after,
        ReadOnlySpan<T> values,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
        where T : struct, IFormattable
    {
        if (values.IsEmpty)
        {
            return [];
        }

        var results = new string[values.Length];

        for (var i = 0; i < results.Length; i++)
        {
            results[i] = Wrap(before, after, values[i], format, provider);
        }

        return results;
    }

    /// <summary>
    /// Wraps the formatted representation of each value in <paramref name="values"/> with the
    /// <paramref name="before"/> and <paramref name="after"/> spans.
    /// </summary>
    public static IEnumerable<string> WrapRange<T>(
        ReadOnlySpan<char> before,
        ReadOnlySpan<char> after,
        ReadOnlySpan<T> values,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
        where T : struct, IFormattable
    {
        if (values.IsEmpty)
        {
            return [];
        }

        var results = new string[values.Length];

        for (var i = 0; i < results.Length; i++)
        {
            results[i] = Wrap(before, after, values[i], format, provider);
        }

        return results;
    }

    private static string[] WrapRangeCore(
        ReadOnlySpan<char> before,
        ReadOnlySpan<char> after,
        ReadOnlySpan<string> values)
    {
        if (values.IsEmpty)
        {
            return [];
        }

        var results = new string[values.Length];

        for (var i = 0; i < results.Length; i++)
        {
            results[i] = Wrap(before, after, values[i]);
        }

        return results;
    }
}
