// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class IntegerMath
{
    public static int DivideByRoundUp(this int dividend, int divisor)
    {
        switch (divisor)
        {
            case 0:
                throw new ArgumentOutOfRangeException(nameof(divisor));
            case -1 when dividend == int.MinValue:
                throw new ArgumentOutOfRangeException(nameof(divisor));
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

    public static long DivideByRoundUp(this long dividend, long divisor)
    {
        switch (divisor)
        {
            case 0:
                throw new ArgumentOutOfRangeException(nameof(divisor));
            case -1 when dividend == long.MinValue:
                throw new ArgumentOutOfRangeException(nameof(divisor));
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
