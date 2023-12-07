// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Tests;

public class Utf8StringTests
{
    [Theory]
    [MemberData(nameof(TestUtf8Strings))]
    public void CreateFromReadOnlyMemory(ReadOnlyMemory<byte> testUtf8String)
    {
        var result = new Utf8String(testUtf8String);

        if (testUtf8String.Length > 0)
        {
            Assert.False(result.IsEmpty);
        }
        else
        {
            Assert.True(result.IsEmpty);
        }

        Assert.Equal(testUtf8String.Length, result.Length);
    }

    [Theory]
    [MemberData(nameof(TestUtf8Strings))]
    public void ToStringReturnsString(ReadOnlyMemory<byte> testUtf8String)
    {
        var result = new Utf8String(testUtf8String).ToString();
        var original = Encoding.UTF8.GetString(testUtf8String.Span);
        Assert.Equal(original, result);
    }

    [Theory]
    [MemberData(nameof(TestUtf8Strings))]
    public void EqualsReturnsTrueForEqualValues(ReadOnlyMemory<byte> testUtf8String)
    {
        var value1 = new Utf8String(testUtf8String);
        var value2 = new Utf8String(testUtf8String);
        Assert.True(value1.Equals(value2));
        Assert.True(value2.Equals(value1));
        Assert.Equal(value1, value2);
    }

    [Theory]
    [MemberData(nameof(TestUtf8Strings))]
    public void EqualityOperatorReturnsTrueForEqualValues(ReadOnlyMemory<byte> testUtf8String)
    {
        var value1 = new Utf8String(testUtf8String);
        var value2 = new Utf8String(testUtf8String);
        Assert.True(value1 == value2);
        Assert.True(value2 == value1);
    }

    [Theory]
    [MemberData(nameof(TestUtf8Strings))]
    public void InequalityOperatorReturnsFalseForEqualValues(ReadOnlyMemory<byte> testUtf8String)
    {
        var value1 = new Utf8String(testUtf8String);
        var value2 = new Utf8String(testUtf8String);
        Assert.False(value1 != value2);
        Assert.False(value2 != value1);
    }

    [Theory]
    [MemberData(nameof(TestUtf8Strings))]
    public void JsonSerializeDeserialize(ReadOnlyMemory<byte> testUtf8String)
    {
        var value1 = new Utf8String(testUtf8String);
        var json = JsonSerializer.Serialize(value1, JsonSharedOptions.RelaxedJsonEscaping);
        var expectedJson = JsonSerializer.Serialize(value1.ToString(), JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(expectedJson, json);
        var value2 = JsonSerializer.Deserialize<Utf8String>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.False(value1 != value2);
        Assert.False(value2 != value1);
    }

    public static TheoryData<ReadOnlyMemory<byte>> TestUtf8Strings
    {
        get
        {
            var result = new TheoryData<ReadOnlyMemory<byte>>();
            foreach (var s in s_testStrings)
            {
                result.Add(Encoding.UTF8.GetBytes(s));
            }

            return result;
        }
    }

    private static readonly string[] s_testStrings =
    [
        "Test string.",
        "Test \"string\".",
        "",
        "1",
        "£",
        "Faut-il combattre le moustique-tigre ou apprendre à vivre avec en France ? « Tous les humains n’attirent pas les " +
            "moustiques de la même manière : cela dépend des molécules, de l’acide lactique ou butyrique »",
        "In Charleston Harbor, where the initiating shots of the Civil War were fired — Fort Sumter is distantly visible — " +
            "I’m on the site of a former shipping pier known as Gadsden’s Wharf.",
        @"A
multiline
string."
    ];
}
