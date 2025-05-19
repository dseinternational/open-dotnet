// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;


/// <summary>
/// A value that may be missing or unknown. Unlike <see cref="INullable"/> or <see cref="Nullable{T}"/>,
/// with <see cref="ITriNullable"/>, 'unknown' == 'unknown' is <see langword="false"/>.
/// </summary>
public interface ITriNullable
{
    /// <summary>
    /// Indicates if the current value has been set.
    /// </summary>
    bool HasValue { get; }

    bool IsMissing => !HasValue;

    /// <summary>
    /// If the value is set (<see cref="HasValue"/>), then returns that value, otherwise throws
    /// a <see cref="Open.UnknownValueException"/>.
    /// </summary>
    /// <exception cref="Open.UnknownValueException">Thrown if no value is available
    /// (<see cref="HasValue"/> is false).</exception>
    object Value { get; }
}

/// <summary>
/// A value that may be missing or unknown. Unlike <see cref="INullable"/> or <see cref="Nullable{T}"/>,
/// with <see cref="ITriNullable"/>, 'unknown' == 'unknown' is <see langword="false"/>.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Implementors should implement <see cref="IEquatable{T}.Equals"/> to return <see langword="true"/>
/// if neither value has a value (<c>missing1.Equals(missing2) == true</c>). This is for consistency
/// with the <see cref="IEquatable{T}"/> implementation requirements. (See also:
/// <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/7.0/equals-nan">
/// Equals method behavior change for NaN</see>). However, implementors must implement the equality
/// operators to return <see langword="false"/> if either value is missing
/// (<c>missing1 == missing2 == false</c> as in <c>float.NaN == float.Nan == false</c>).
/// </remarks>
public interface IMissingNullable<TSelf, T>
    : ITriNullable,
      IEquatable<TSelf>,
      IEqualityOperators<TSelf, TSelf, bool>
    where T : IEquatable<T>
    where TSelf : IMissingNullable<TSelf, T>
{
    /// <summary>
    /// Gets a value that represents the 'null' or 'no value' state.
    /// </summary>
    static virtual TSelf Null => default!;

    /// <summary>
    /// If the value is set (<see cref="ITriNullable.HasValue"/>), then returns that value, otherwise
    /// throws a <see cref="Open.UnknownValueException"/>.
    /// </summary>
    /// <exception cref="Open.UnknownValueException">Thrown if no value is available
    /// (<see cref="ITriNullable.HasValue"/> is false).</exception>
    new T Value { get; }

    object ITriNullable.Value => Value;

    static virtual bool Equals(TSelf v1, TSelf v2)
    {
        return v1.HasValue && v2.HasValue && v1.Value.Equals(v2.Value);
    }

    static virtual bool EqualOrMissing(TSelf v1, TSelf v2)
    {
        return v1.HasValue
            ? v2.HasValue && v1.Value.Equals(v2.Value)
            : !v2.HasValue;
    }

    static virtual bool operator ==(TSelf left, TSelf right)
    {
        return TSelf.Equals(left, right);
    }

    static virtual bool operator !=(TSelf left, TSelf right)
    {
        return !TSelf.Equals(left, right);
    }
}
