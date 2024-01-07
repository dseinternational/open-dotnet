// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using DSE.Open.Text;

namespace DSE.Open.Tests.Text;

public partial class StringHelperTests
{
    [Fact]
    public void Joins_array_of_string()
    {
        string[] values = ["one", "two", "three"];
        var joined = StringHelper.Join(", ", " and ", values);
        Assert.Equal("one, two and three", joined);
    }

    [Fact]
    public void Joins_list_of_string()
    {
        List<string> values = ["one", "two", "three"];
        var joined = StringHelper.Join(", ", " and ", values);
        Assert.Equal("one, two and three", joined);
    }

    [Fact]
    public void Join_WithCollectionOfString()
    {
        Collection<string> values = ["one", "two", "three"];
        var joined = StringHelper.Join(", ", " and ", values);
        Assert.Equal("one, two and three", joined);
    }

    [Fact]
    public void Joins_enumerable_of_string()
    {
        var joined = StringHelper.Join(", ", " and ", GetValues());
        Assert.Equal("one, two and three", joined);

        static IEnumerable<string> GetValues()
        {
            yield return "one";
            yield return "two";
            yield return "three";
        }
    }

    [Fact]
    public void JoinsSpan_array_of_string()
    {
        string[] values = ["one", "two", "three"];
        var joined = StringHelper.Join((ReadOnlySpan<char>)", ", " and ", values);
        Assert.Equal("one, two and three", joined);
    }

    [Fact]
    public void JoinsSpan_list_of_string()
    {
        List<string> values = ["one", "two", "three"];
        var joined = StringHelper.Join((ReadOnlySpan<char>)", ", " and ", values);
        Assert.Equal("one, two and three", joined);
    }

    [Fact]
    public void JoinSpan_WithCollectionOfString()
    {
        Collection<string> values = ["one", "two", "three"];
        var joined = StringHelper.Join((ReadOnlySpan<char>)", ", " and ", values);
        Assert.Equal("one, two and three", joined);
    }

    [Fact]
    public void JoinsSpan_enumerable_of_string()
    {
        var joined = StringHelper.Join((ReadOnlySpan<char>)", ", " and ", GetValues());
        Assert.Equal("one, two and three", joined);

        static IEnumerable<string> GetValues()
        {
            yield return "one";
            yield return "two";
            yield return "three";
        }
    }

    [Fact]
    public void JoinSpan_WithEmptyFinalSeparator_ShouldPerformNormalJoin()
    {
        // Arrange
        string[] values = ["one", "two", "three"];
        var finalSeparator = string.Empty;
        ReadOnlySpan<char> separator = " ";

        // Act
        var joined = StringHelper.Join(separator, finalSeparator, values);

        // Assert
        Assert.Equal("one two three", joined);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    public void JoinSpan_WithEmptyEnumerable_ShouldReturnEmptyString(string finalSeparator)
    {
        // Arrange
        string[] values = [];
        ReadOnlySpan<char> separator = " ";

        // Act
        var joined = StringHelper.Join(separator, finalSeparator, values);

        // Assert
        Assert.Equal(string.Empty, joined);
    }

    [Fact]
    public void Joins_array_of_int()
    {
        int[] values = [1, 2, 3, 4, 5];
        var joined = StringHelper.Join(", ", " and ", values);
        Assert.Equal("1, 2, 3, 4 and 5", joined);
    }

    [Fact]
    public void Joins_array_of_double_formatted()
    {
        double[] values = [1.7895, 2.84, 3.0, 4.44875, 5.555];
        var joined = StringHelper.Join(", ", " and ", values, "0.00", CultureInfo.InvariantCulture);
        Assert.Equal("1.79, 2.84, 3.00, 4.45 and 5.56", joined);
    }

    [Fact]
    public void Joins_big_array_of_double_formatted()
    {
        double[] values = Enumerable.Range(1,1000).Select(i => i * 0.77313).ToArray();
        var joined = StringHelper.Join(", ", default, values, "0.00", CultureInfo.InvariantCulture);
        var expected = string.Join(", ", values.Select(v => v.ToString("0.00", CultureInfo.InvariantCulture)));
        Assert.Equal(expected, joined);
    }

    [Fact]
    public void Join_Empty()
    {
        var result = StringHelper.Join(" ", default, Enumerable.Empty<string>());
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Join_Null()
    {
        static void Act()
        {
            _ = StringHelper.Join(" ", default, null!);
        }

        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact]
    public void Join_Single()
    {
        var result = StringHelper.Join(" ", default, s_hello);
        Assert.NotNull(result);
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void Join_Two()
    {
        var result = StringHelper.Join(" ", default, s_helloWorld);
        Assert.NotNull(result);
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Join_Three()
    {
        var result = StringHelper.Join(" ", default, s_helloWorldExc);
        Assert.NotNull(result);
        Assert.Equal("Hello World !", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator()
    {
        var result = StringHelper.Join(" ", " and ", s_helloWorldExc);
        Assert.NotNull(result);
        Assert.Equal("Hello World and !", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_Null()
    {
        var result = StringHelper.Join(" ", null, s_helloWorldExc);
        Assert.NotNull(result);
        Assert.Equal("Hello World !", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_Empty()
    {
        var result = StringHelper.Join(" ", string.Empty, s_helloWorldExc);
        Assert.NotNull(result);
        Assert.Equal("Hello World !", result);
    }

    [Fact(Skip = "TODO")]
    public void Join_Three_FinalSeparator_EmptySeparator()
    {
        var result = StringHelper.Join(string.Empty, " and ", s_helloWorldExc);
        Assert.NotNull(result);
        Assert.Equal("HelloWorld and !", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_EmptySeparator_Null()
    {
        var result = StringHelper.Join(string.Empty, null, s_helloWorldExc);
        Assert.NotNull(result);
        Assert.Equal("HelloWorld!", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_Empty_WithCollection()
    {
        var result = StringHelper.Join(" ", string.Empty, new Collection<string> { "Hello", "World", "!" });
        Assert.NotNull(result);
        Assert.Equal("Hello World !", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_NonEmpty_WithEnum()
    {
        // non collection enumerable
        var result = StringHelper.Join(" ", " and ", s_helloWorldExc.Select(s => s));
        Assert.NotNull(result);
        Assert.Equal("Hello World and !", result);
    }

    [Fact]
    public void Join_Many_Array_FinalSeperator_Null()
    {
        var text = s_manyWords;

        var result = StringHelper.Join(" ", null, text);

        Assert.NotNull(result);

        Assert.Equal("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, " +
            "adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. " +
            "Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum diam nisi sit amet, sodales vel, dolor. Nulla in est. " +
            "Curabitur viverra metus accumsan nunc. In nibh. Duis vitae velit eu erat mollis placerat. Praesent dapibus, neque id cursus faucibus, " +
            "tortor neque egestas augue, eu vulputate magna eros eu erat. Aliquam erat volutpat. Nam dui mi, tincidunt quis, accumsan porttitor, " +
            "facilisis luctus, metus.", result);
    }

    [Fact]
    public void Join_Many_List_FinalSeperator_Null()
    {
        var text = s_manyWords.ToList();

        var result = StringHelper.Join(" ", null, text);

        Assert.NotNull(result);

        Assert.Equal("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, " +
            "adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. " +
            "Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum diam nisi sit amet, sodales vel, dolor. Nulla in est. " +
            "Curabitur viverra metus accumsan nunc. In nibh. Duis vitae velit eu erat mollis placerat. Praesent dapibus, neque id cursus faucibus, " +
            "tortor neque egestas augue, eu vulputate magna eros eu erat. Aliquam erat volutpat. Nam dui mi, tincidunt quis, accumsan porttitor, " +
            "facilisis luctus, metus.", result);
    }

    [Fact]
    public void Join_Many_Collection_FinalSeperator_Null()
    {
        var text = new Collection<string>(s_manyWords);

        var result = StringHelper.Join(" ", null, text);

        Assert.NotNull(result);

        Assert.Equal("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, " +
            "adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. " +
            "Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum diam nisi sit amet, sodales vel, dolor. Nulla in est. " +
            "Curabitur viverra metus accumsan nunc. In nibh. Duis vitae velit eu erat mollis placerat. Praesent dapibus, neque id cursus faucibus, " +
            "tortor neque egestas augue, eu vulputate magna eros eu erat. Aliquam erat volutpat. Nam dui mi, tincidunt quis, accumsan porttitor, " +
            "facilisis luctus, metus.", result);
    }

    private static readonly string[] s_hello = ["Hello"];
    private static readonly string[] s_helloWorld = ["Hello", "World"];
    private static readonly string[] s_helloWorldExc = ["Hello", "World", "!"];
    private static readonly string[] s_manyWords =
    [
        "Lorem",
        "ipsum",
        "dolor",
        "sit",
        "amet,",
        "consectetur",
        "adipiscing",
        "elit.",
        "Sed",
        "non",
        "risus.",
        "Suspendisse",
        "lectus",
        "tortor,",
        "dignissim",
        "sit",
        "amet,",
        "adipiscing",
        "nec,",
        "ultricies",
        "sed,",
        "dolor.",
        "Cras",
        "elementum",
        "ultrices",
        "diam.",
        "Maecenas",
        "ligula",
        "massa,",
        "varius",
        "a,",
        "semper",
        "congue,",
        "euismod",
        "non,",
        "mi.",
        "Proin",
        "porttitor,",
        "orci",
        "nec",
        "nonummy",
        "molestie,",
        "enim",
        "est",
        "eleifend",
        "mi,",
        "non",
        "fermentum",
        "diam",
        "nisi",
        "sit",
        "amet,",
        "sodales",
        "vel,",
        "dolor.",
        "Nulla",
        "in",
        "est.",
        "Curabitur",
        "viverra",
        "metus",
        "accumsan",
        "nunc.",
        "In",
        "nibh.",
        "Duis",
        "vitae",
        "velit",
        "eu",
        "erat",
        "mollis",
        "placerat.",
        "Praesent",
        "dapibus,",
        "neque",
        "id",
        "cursus",
        "faucibus,",
        "tortor",
        "neque",
        "egestas",
        "augue,",
        "eu",
        "vulputate",
        "magna",
        "eros",
        "eu",
        "erat.",
        "Aliquam",
        "erat",
        "volutpat.",
        "Nam",
        "dui",
        "mi,",
        "tincidunt",
        "quis,",
        "accumsan",
        "porttitor,",
        "facilisis",
        "luctus,",
        "metus."
    ];
}
