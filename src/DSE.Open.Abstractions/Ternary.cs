// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Helpers for three-valued logic.
/// </summary>
public static class Ternary
{
    /// <summary>
    /// Compares two nullable value types for equality and returns a <see cref="Trilean"/>
    /// value indicating the result.
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    /// <typeparam name="T">The type of the values to compare. Must be a value type that
    /// implements <see cref="IEquatable{T}"/>.</typeparam>
    /// <param name="left">The first value to compare. Can be <see langword="null"/>.</param>
    /// <param name="right">The second value to compare. Can be <see langword="null"/>.</param>
    /// <returns>A <see cref="Trilean"/> indicating the result of the comparison:
    /// <see langword="null"/> if either value is <see langword="null"/>, <see cref="Trilean.True"/>
    /// if the values are not null and equal, or  <see cref="Trilean.False"/> if the
    /// values are not null and not equal.</returns>
    public static Trilean Equals<TSelf, T>(TSelf left, TSelf right)
        where T : notnull, IEquatable<T>
        where TSelf : struct, INaValue<TSelf, T>
    {
        if (left.IsNa || right.IsNa)
        {
            return Trilean.Na;
        }

        if (left.Value.Equals(right.Value))
        {
            return Trilean.True;
        }

        return Trilean.False;
    }

    /// <summary>
    /// Compares two nullable value types for equality and returns a <see cref="Trilean"/>
    /// value indicating the result.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare. Must be a value type that
    /// implements <see cref="IEquatable{T}"/>.</typeparam>
    /// <param name="left">The first value to compare. Can be <see langword="null"/>.</param>
    /// <param name="right">The second value to compare. Can be <see langword="null"/>.</param>
    /// <returns>A <see cref="Trilean"/> indicating the result of the comparison:
    /// <see langword="null"/> if either value is <see langword="null"/>, <see cref="Trilean.True"/>
    /// if the values are not null and equal, or  <see cref="Trilean.False"/> if the
    /// values are not null and not equal.</returns>
    public static Trilean Equals<T>(T? left, T? right)
        where T : struct, IEquatable<T>
    {
        if (left is null || right is null)
        {
            return Trilean.Na;
        }

        if (left.Value.Equals(right.Value))
        {
            return Trilean.True;
        }

        return Trilean.False;
    }

    /// <summary>
    /// Compares two values for equality using three-valued logic and returning a <see cref="Trilean"/> result.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>A <see cref="Trilean"/> value indicating the result of the comparison:
    /// <see cref="Trilean.True"/> if the values are equal, <see cref="Trilean.False"/> if they are not equal,
    /// or <see cref="Trilean.Na"/> if either value is unknown.</returns>
    public static Trilean Equals<T>(T left, T right)
        where T : struct, INaValue, IEquatable<T>
    {
        // if either value is unknown, return unknown.
        if (!(left.HasValue && right.HasValue))
        {
            return Trilean.Na;
        }

        // if both values are known, return true if equal or false if not equal.
        return left.Equals(right);
    }

    /*
     * TODO: Not sure about this
     * 
    /// <summary>
    /// Determines whether two objects are equal and returns a <see cref="Trilean"/>
    /// value indicating the result.
    /// </summary>
    /// <param name="left">The first object to compare. Can be <see langword="null"/>.</param>
    /// <param name="right">The second object to compare. Can be <see langword="null"/>.</param>
    /// <returns>A <see cref="Trilean"/> value indicating the equality of the two objects:
    /// <see cref="Trilean.True"/> if the objects are not null and equal, <see cref="Trilean.False"/>
    /// if they are not null and not equal, or <see langword="null"/> if either object
    /// is <see langword="null"/>.</returns>
    public static new Trilean Equals(object? left, object? right)
    {
        if (left is null || right is null)
        {
            return null;
        }

        if (left == right)
        {
            return Trilean.True;
        }

        return Trilean.False;
    }
    */

    public static bool EqualAndNeitherNa<T>(T left, T right)
        where T : struct, INaValue, IEquatable<T>
    {
        return Equals(left, right).IsTrue;
    }

    public static bool EqualAndNeitherNa<T>(T? left, T? right)
        where T : struct, IEquatable<T>
    {
        return Equals(left, right).IsTrue;
    }

    /*
    public static bool EqualAndNeitherNa(object? left, object? right)
    {
        return Equals(left, right).IsTrue;
    }
    */

    /// <summary>
    /// Compares two nullable value types for equality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if both values are equal or both values are unknown, otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// This is the default behavior for <see cref="IEquatable{T}.Equals"/> and <see cref="Nullable{T}.Equals"/>.
    /// </remarks>
    public static bool EqualOrBothNa<T>(T left, T right)
        where T : struct, INaValue, IEquatable<T>
    {
        // if both values are unknown, return true.

        if (left.IsNa)
        {
            return right.IsNa;
        }

        // if both values are known, return true if equal or false if not equal.
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two nullable value types for equality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if both values are equal or both values are unknown, otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// This is the default behavior for <see cref="IEquatable{T}.Equals"/> and <see cref="Nullable{T}.Equals"/>.
    /// </remarks>
    public static bool EqualOrBothNa<T>(T? left, T? right)
        where T : struct, IEquatable<T>
    {
        // if both values are unknown, return true.

        if (left is null)
        {
            return right is null || right.IsUnknown();
        }

        if (left.IsUnknown())
        {
            return right.IsUnknown();
        }

        // if both values are known, return true if equal or false if not equal.
        return left.Equals(right);
    }

    /*
    public static bool EqualOrBothNa(object? left, object? right)
    {
        // if both values are unknown, return true.

        if (left is null || (left is INaValue ln && ln.IsNa))
        {
            return right is null || (right is INaValue rn && rn.IsNa);
        }

        // if both values are known, return true if equal or false if not equal.
        return left.Equals(right);
    }
    */

    /// <summary>
    /// Compares two nullable value types for equality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if both values are equal or either value is unknown, otherwise <see langword="false"/>.
    /// </returns>
    public static bool EqualOrEitherNa<T>(T left, T right)
        where T : struct, INaValue, IEquatable<T>
    {
        // if either values are unknown, return true.

        if (left.IsNa || right.IsNa)
        {
            return true;
        }

        // if both values are known, return true if equal or false if not equal.
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two nullable value types for equality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if both values are equal or either value is unknown, otherwise <see langword="false"/>.
    /// </returns>
    public static bool EqualOrEitherNa<T>(T? left, T? right)
        where T : struct, IEquatable<T>
    {
        // if either values are unknown, return true.

        if (left is null || right is null || left.IsUnknown() || right.IsUnknown())
        {
            return true;
        }

        // if both values are known, return true if equal or false if not equal.
        return left.Equals(right);
    }
}
