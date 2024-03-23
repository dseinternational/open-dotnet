// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.Requests;

/// <summary>
/// A helper class to create an <see cref="Update{T}"/> instance.
/// </summary>
public static class Update
{
    public static Update<T> New<T>(T value, UpdateMode mode)
    {
        if (mode is UpdateMode.None && value is not null)
        {
            throw new ArgumentException(
                $"A {nameof(value)} must not be provided when the {nameof(mode)} is {nameof(UpdateMode.None)}",
                nameof(value));
        }

        return new Update<T>(value, mode);
    }

    public static Update<T?> NewUpdate<T>(T value)
    {
        return new Update<T?>(value, UpdateMode.Update);
    }

    public static Update<T?> NoChange<T>()
    {
        return new Update<T?>(default, UpdateMode.None);
    }
}

/// <summary>
/// A discriminated union to represent an update request for a value, or no update request.
/// </summary>
/// <typeparam name="T">The type of the value</typeparam>
#pragma warning disable CA1815 // Override equals and operator equals on value types
public readonly struct Update<T>
#pragma warning restore CA1815
{
    private readonly T? _value;

    internal Update(T? value, UpdateMode mode)
    {
        _value = value;
        Mode = mode;
    }

    public UpdateMode Mode { get; }

    /// <summary>
    /// Gets the value
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="Mode"/> is <see cref="UpdateMode.None"/></exception>
    public T GetValue()
    {
        if (!HasUpdate)
        {
            ThrowGetForNoUpdateException();
        }

        return _value;
    }

    /// <summary>
    /// Gets the value if the <see cref="Mode"/> is not <see cref="UpdateMode.None"/>
    /// </summary>
    /// <param name="value">The value</param>
    /// <returns><see langword="true"/> if the <see cref="Mode"/> is not <see cref="UpdateMode.None"/>; otherwise, <see langword="false"/></returns>
    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        if (HasUpdate)
        {
            value = _value;
            return true;
        }

        value = default;
        return false;
    }

    [MemberNotNullWhen(true, nameof(_value))]
    public bool HasUpdate => Mode is UpdateMode.Update;

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowGetForNoUpdateException()
    {
        throw new InvalidOperationException(
            $"Cannot get value when {nameof(Update<T>)}.{nameof(Mode)} is {UpdateMode.None}");
    }
}

public enum UpdateMode
{
    /// <summary>
    /// Represents no update request. The value should not be changed.
    /// </summary>
    None,

    /// <summary>
    /// Requests the update of a value.
    /// </summary>
    Update
}
