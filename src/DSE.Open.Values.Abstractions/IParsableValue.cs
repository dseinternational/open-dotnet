// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Values;

/// <summary>
/// Indicates that a <see cref="IValue{TSelf, T}"/> type can be read from a string.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface IParsableValue<TSelf, T> : IValue<TSelf, T>, IParsable<TSelf>
    where T : IEquatable<T>, IParsable<T>
    where TSelf : struct, IParsableValue<TSelf, T>
{
    static new TSelf Parse(string s, IFormatProvider? provider)
    {
        return (TSelf)T.Parse(s, provider);
    }

    static new bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out TSelf result)
    {
        if (T.TryParse(s, provider, out var valueResult) && TSelf.TryFromValue(valueResult, out result))
        {
            return true;
        }

        result = default;
        return false;
    }
}
