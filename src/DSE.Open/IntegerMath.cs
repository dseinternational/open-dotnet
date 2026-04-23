// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Integer arithmetic helpers.
/// </summary>
public static class IntegerMath
{
    /// <summary>
    /// Divides <paramref name="dividend"/> by <paramref name="divisor"/> and rounds the
    /// result toward positive infinity (ceiling division). Any non-zero remainder
    /// increments the quotient when the mathematical result is positive.
    /// </summary>
    /// <param name="dividend">The value being divided.</param>
    /// <param name="divisor">The value to divide by.</param>
    /// <returns>The quotient, rounded toward positive infinity when the division has a remainder.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="divisor"/> is <c>0</c>, or <paramref name="dividend"/> is
    /// <see cref="int.MinValue"/> and <paramref name="divisor"/> is <c>-1</c> (would overflow).
    /// </exception>
    public static int DivideByRoundUp(this int dividend, int divisor)
    {
        switch (divisor)
        {
            case 0:
                throw new ArgumentOutOfRangeException(nameof(divisor));
            case -1 when dividend == int.MinValue:
                throw new ArgumentOutOfRangeException(nameof(divisor));
            default:
                break;
        }

        var roundedTowardsZeroQuotient = dividend / divisor;

        var dividedEvenly = dividend % divisor == 0;

        if (dividedEvenly)
        {
            return roundedTowardsZeroQuotient;
        }

        var wasRoundedDown = (divisor > 0) == (dividend > 0);

        return wasRoundedDown ? roundedTowardsZeroQuotient + 1 : roundedTowardsZeroQuotient;
    }

    /// <summary>
    /// Divides <paramref name="dividend"/> by <paramref name="divisor"/> and rounds the
    /// result toward positive infinity (ceiling division). Any non-zero remainder
    /// increments the quotient when the mathematical result is positive.
    /// </summary>
    /// <param name="dividend">The value being divided.</param>
    /// <param name="divisor">The value to divide by.</param>
    /// <returns>The quotient, rounded toward positive infinity when the division has a remainder.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="divisor"/> is <c>0</c>, or <paramref name="dividend"/> is
    /// <see cref="long.MinValue"/> and <paramref name="divisor"/> is <c>-1</c> (would overflow).
    /// </exception>
    public static long DivideByRoundUp(this long dividend, long divisor)
    {
        switch (divisor)
        {
            case 0:
                throw new ArgumentOutOfRangeException(nameof(divisor));
            case -1 when dividend == long.MinValue:
                throw new ArgumentOutOfRangeException(nameof(divisor));
            default:
                break;
        }

        var roundedTowardsZeroQuotient = dividend / divisor;

        var dividedEvenly = dividend % divisor == 0;

        if (dividedEvenly)
        {
            return roundedTowardsZeroQuotient;
        }

        var wasRoundedDown = (divisor > 0) == (dividend > 0);

        return wasRoundedDown ? roundedTowardsZeroQuotient + 1 : roundedTowardsZeroQuotient;
    }
}
