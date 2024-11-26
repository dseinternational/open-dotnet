// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class AsciiTestData
{
    public static TheoryData<byte> ValidAsciiCharBytes
    {
        get
        {
            var result = new TheoryData<byte>();
            for (var i = 0; i < 128; i++)
            {
                result.Add((byte)i);
            }
            return result;
        }
    }
    public static TheoryData<string> ValidAsciiCharSequenceStrings
    {
        get
        {
            var result = new TheoryData<string>()
            {
                "a",
                "A valid value.",
                "abcdefghijklmnopqrstuvwxyz",
            };
            return result;
        }
    }
}
