// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using DSE.Open.Text;
#pragma warning disable CA1861


namespace DSE.Open.Tests.Text;

public class StringConcatenatorTests
{

    [Fact]
    public void Join_Empty()
    {
        var result = StringConcatenator.Join(" ", Enumerable.Empty<string>());
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Join_Null()
    {
        void Act() => _ = StringConcatenator.Join(" ", null!);
        Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact]
    public void Join_Single()
    {
        var result = StringConcatenator.Join(" ", new[] { "Hello" });
        Assert.NotNull(result);
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void Join_Two()
    {
        var result = StringConcatenator.Join(" ", new[] { "Hello", "World" });
        Assert.NotNull(result);
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Join_Three()
    {
        var result = StringConcatenator.Join(" ", new[] { "Hello", "World", "!" });
        Assert.NotNull(result);
        Assert.Equal("Hello World !", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator()
    {
        var result = StringConcatenator.Join(" ", new[] { "Hello", "World", "!" }, " and ");
        Assert.NotNull(result);
        Assert.Equal("Hello World and !", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_Null()
    {
        var result = StringConcatenator.Join(" ", new[] { "Hello", "World", "!" }, null);
        Assert.NotNull(result);
        Assert.Equal("Hello World !", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_Empty()
    {
        var result = StringConcatenator.Join(" ", new[] { "Hello", "World", "!" }, string.Empty);
        Assert.NotNull(result);
        Assert.Equal("Hello World!", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_EmptySeparator()
    {
        var result = StringConcatenator.Join(string.Empty, new[] { "Hello", "World", "!" }, " and ");
        Assert.NotNull(result);
        Assert.Equal("HelloWorld and !", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_EmptySeparator_Null()
    {
        var result = StringConcatenator.Join(string.Empty, new[] { "Hello", "World", "!" }, null);
        Assert.NotNull(result);
        Assert.Equal("HelloWorld!", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_Empty_WithCollection()
    {
        var result = StringConcatenator.Join(" ", new Collection<string> { "Hello", "World", "!" }, string.Empty);
        Assert.NotNull(result);
        Assert.Equal("Hello World!", result);
    }

    [Fact]
    public void Join_Three_FinalSeparator_NonEmpty_WithEnumerable()
    {
        // non collection enumerable
        var result = StringConcatenator.Join(" ", new[] { "Hello", "World", "!" }.Select(s => s), " and ");
        Assert.NotNull(result);
        Assert.Equal("Hello World and !", result);
    }

    [Fact]
    //long string over 256 chars to force use of ArrayPool
    public void Join_Many_FinalSeparator_Null()
    {
        // ipsum text
        ICollection<string> text = new List<string>() {"Lorem", "ipsum", "dolor", "sit", "amet,", "consectetur", "adipiscing", "elit.", "Sed", "non", "risus.", "Suspendisse", "lectus", "tortor,", "dignissim", "sit", "amet,", "adipiscing", "nec,", "ultricies", "sed,", "dolor.", "Cras", "elementum", "ultrices", "diam.", "Maecenas", "ligula", "massa,", "varius", "a,", "semper", "congue,", "euismod", "non,", "mi.", "Proin", "porttitor,", "orci", "nec", "nonummy", "molestie,", "enim", "est", "eleifend", "mi,", "non", "fermentum", "diam", "nisi", "sit", "amet,", "sodales", "vel,", "dolor.", "Nulla", "in", "est.", "Curabitur", "viverra", "metus", "accumsan", "nunc.", "In", "nibh.", "Duis", "vitae", "velit", "eu", "erat", "mollis", "placerat.", "Praesent", "dapibus,", "neque", "id", "cursus", "faucibus,", "tortor", "neque", "egestas", "augue,", "eu", "vulputate", "magna", "eros", "eu", "erat.", "Aliquam", "erat", "volutpat.", "Nam", "dui", "mi,", "tincidunt", "quis,", "accumsan", "porttitor,", "facilisis", "luctus,", "metus."};
        var result = StringConcatenator.Join(" ", text, null);
        Assert.NotNull(result);
        Assert.Equal("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum diam nisi sit amet, sodales vel, dolor. Nulla in est. Curabitur viverra metus accumsan nunc. In nibh. Duis vitae velit eu erat mollis placerat. Praesent dapibus, neque id cursus faucibus, tortor neque egestas augue, eu vulputate magna eros eu erat. Aliquam erat volutpat. Nam dui mi, tincidunt quis, accumsan porttitor, facilisis luctus, metus.", result);
    }



}
