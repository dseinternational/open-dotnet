// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Text;

public static partial class StringHelper
{
    public static string Wrap(ReadOnlySpan<char> wrapper, ReadOnlySpan<char> value)
    {
        return Wrap(wrapper, wrapper, value);
    }

    public static string Wrap(ReadOnlySpan<char> before, ReadOnlySpan<char> after, ReadOnlySpan<char> value)
    {
        return $"{before}{value}{after}";
    }

    public static string Wrap(string wrapper, string value)
    {
        return Wrap(wrapper, wrapper, value);
    }

    public static string Wrap(string before, string after, string value)
    {
        return $"{before}{value}{after}";
    }

    public static string Wrap<T>(char wrapper, T value, string? format = default, IFormatProvider? provider = default)
    {
        return Wrap(wrapper, wrapper, value, format, provider);
    }

    public static string Wrap<T>(char before, char after, T value, string? format = default, IFormatProvider? provider = default)
    {
        var sh = new DefaultInterpolatedStringHandler(2, 1, provider, stackalloc char[256]);
        sh.AppendFormatted(before, null);
        sh.AppendFormatted(value, format);
        sh.AppendFormatted(after, null);
        return sh.ToStringAndClear();
    }

    public static string Wrap<T>(string wrapper, T value, string? format = default, IFormatProvider? provider = default)
    {
        return Wrap(wrapper, wrapper, value, format, provider);
    }

    public static string Wrap<T>(string before, string after, T value, string? format = default, IFormatProvider? provider = default)
    {
        ArgumentNullException.ThrowIfNull(before);
        ArgumentNullException.ThrowIfNull(after);

        var sh = new DefaultInterpolatedStringHandler(before.Length + after.Length, 1, provider, stackalloc char[256]);
        sh.AppendLiteral(before);
        sh.AppendFormatted(value, format);
        sh.AppendLiteral(after);
        return sh.ToStringAndClear();
    }

    public static string Wrap<T>(ReadOnlySpan<char> wrapper, T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
    {
        return Wrap(wrapper, wrapper, value, format, provider);
    }

    public static string Wrap<T>(ReadOnlySpan<char> before, ReadOnlySpan<char> after, T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
    {
        var sh = new DefaultInterpolatedStringHandler(before.Length + after.Length, 1, provider, stackalloc char[256]);
        sh.AppendFormatted(before);
        sh.AppendFormatted(value, format.ToString()); // TODO: isn't this avoidable?
        sh.AppendFormatted(after);
        return sh.ToStringAndClear();
    }

    public static IEnumerable<string> WrapRange(ReadOnlySpan<char> before, ReadOnlySpan<char> after, IEnumerable<string> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (values is List<string?> valuesList)
        {
            return WrapRangeCore(before, after, (ReadOnlySpan<string>)CollectionsMarshal.AsSpan(valuesList));
        }

        if (values is string[] valuesArray)
        {
            return WrapRangeCore(before, after, new ReadOnlySpan<string>(valuesArray));
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

    public static IEnumerable<string> WrapRange<T>(
        ReadOnlySpan<char> before,
        ReadOnlySpan<char> after,
        IEnumerable<T> values,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (values is List<string?> valuesList)
        {
            return WrapRangeCore(before, after, (ReadOnlySpan<string>)CollectionsMarshal.AsSpan(valuesList), format, provider);
        }

        if (values is string[] valuesArray)
        {
            return WrapRangeCore(before, after, new ReadOnlySpan<string>(valuesArray), format, provider);
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

    private static IEnumerable<string> WrapRangeCore(
        ReadOnlySpan<char> before,
        ReadOnlySpan<char> after,
        ReadOnlySpan<string> values)
    {
        if (values.IsEmpty)
        {
            return Enumerable.Empty<string>();
        }

        var results = new string[values.Length];

        for (var i = 0; i < results.Length; i++)
        {
            results[i] = Wrap(before, after, values[i]);
        }

        return results;
    }

    private static IEnumerable<string> WrapRangeCore<T>(
        ReadOnlySpan<char> before,
        ReadOnlySpan<char> after,
        ReadOnlySpan<T> values,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
    {
        if (values.IsEmpty)
        {
            return Enumerable.Empty<string>();
        }

        var results = new string[values.Length];

        for (var i = 0; i < results.Length; i++)
        {
            results[i] = Wrap(before, after, values[i], format, provider);
        }

        return results;
    }
}
