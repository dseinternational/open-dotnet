// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class FixedHashCodeTests
{
    public FixedHashCodeTests(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    [Fact]
    public void Distribution()
    {
        var inputs = new[]
        {
            "reading", "language", "speech", "numbers", "memory", "parent", "professional", "type: online course"
        };

        var hashes = inputs.Select(x => x.AsSpan().GetFixedHashCode());

        foreach (var h in hashes)
        {
            Output.WriteLine(h.ToStringInvariant());
        }
    }
    /*
    [Fact]
    public void Colors()
    {
        var inputs = new[]
        {
            "reading", "language", "speech", "numbers", "memory", "parent", "professional", "type: online course"
        };

        var hashes = inputs.Select(x => x.AsSpan().GetFixedHashCode()).ToArray();

        var hues = hashes.Select(h => Math.Abs(h / (float)int.MaxValue)).ToArray();

        var colors = hues.Select(h => Color.FromHsl(h, 0.6f, 0.75f)).ToArray();

        for (var i = 0; i < inputs.Length; i++)
        {
            Output.WriteLine($"""
                <div style="background-color:{colors[i]};width:120px;height:50px;text-align:center">{inputs[i]}</div>
                """);
        }
    }
    */
}
