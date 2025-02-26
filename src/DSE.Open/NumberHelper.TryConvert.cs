// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8605 // Unboxing a possibly null value.

public static partial class NumberHelper
{
    /// <summary>
    /// Tries to convert a <typeparamref name="TOther"/> value to a <see cref="Int64"/> value,
    /// throwing an overflow exception for any values that fall outside the representable range of the current type
    /// </summary>
    /// <typeparam name="TOther"></typeparam>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryConvertToInt64Checked<TOther>(TOther value, out long result)
    {
        // todo: complete
        // see: https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Int64.cs

        if (typeof(TOther) == typeof(long))
        {
            result = (long)(object)value;
            return true;
        }
        else if (typeof(TOther) == typeof(double))
        {
            var actualValue = (double)(object)value;
            result = checked((long)actualValue);
            return true;
        }
        else if (typeof(TOther) == typeof(short))
        {
            var actualValue = (short)(object)value;
            result = actualValue;
            return true;
        }
        else if (typeof(TOther) == typeof(int))
        {
            var actualValue = (int)(object)value;
            result = actualValue;
            return true;
        }
        else if (typeof(TOther) == typeof(ushort))
        {
            var actualValue = (ushort)(object)value;
            result = actualValue;
            return true;
        }
        else if (typeof(TOther) == typeof(uint))
        {
            var actualValue = (uint)(object)value;
            result = actualValue;
            return true;
        }
        else if (typeof(TOther) == typeof(ulong))
        {
            var actualValue = (ulong)(object)value;
            result = checked((long)actualValue);
            return true;
        }
        else if (typeof(TOther) == typeof(float))
        {
            var actualValue = (float)(object)value;
            result = checked((long)actualValue);
            return true;
        }
        else if (typeof(TOther) == typeof(Half))
        {
            var actualValue = (Half)(object)value;
            result = checked((long)actualValue);
            return true;
        }
        else if (typeof(TOther) == typeof(Int128))
        {
            var actualValue = (Int128)(object)value;
            result = checked((long)actualValue);
            return true;
        }
        else if (typeof(TOther) == typeof(nint))
        {
            var actualValue = (nint)(object)value;
            result = actualValue;
            return true;
        }
        else if (typeof(TOther) == typeof(sbyte))
        {
            var actualValue = (sbyte)(object)value;
            result = actualValue;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public static bool TryConvertFromInt64Checked<TOther>(long value, out TOther result)
    {
        // todo: complete
        // see: https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Int64.cs
        throw new NotImplementedException();
    }

    public static bool TryConvertToInt64Saturating<TOther>(long value, out TOther result)
    {
        // todo: complete
        // see: https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Int64.cs
        throw new NotImplementedException();
    }

    public static bool TryConvertFromInt64Saturating<TOther>(long value, out TOther result)
    {
        // todo: complete
        // see: https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Int64.cs
        throw new NotImplementedException();
    }

    public static bool TryConvertToInt64Truncating<TOther>(long value, out TOther result)
    {
        // todo: complete
        // see: https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Int64.cs
        throw new NotImplementedException();
    }

    public static bool TryConvertFromInt64Truncating<TOther>(long value, out TOther result)
    {
        // todo: complete
        // see: https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Int64.cs
        throw new NotImplementedException();
    }
}
