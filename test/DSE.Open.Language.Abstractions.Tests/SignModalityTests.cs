// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language;

public sealed class SignModalityTests
{
    [Theory]
    [MemberData(nameof(Modalities))]
    public void ParseInvariant_round_trips_canonical_values(string modalityStr)
    {
        var modality = SignModality.ParseInvariant(modalityStr);
        Assert.Equal(modalityStr, modality.ToStringInvariant());
    }

    [Theory]
    [MemberData(nameof(Modalities))]
    public void Serialize_deserialize(string modalityStr)
    {
        var modality = SignModality.ParseInvariant(modalityStr);
        AssertJson.Roundtrip(modality);
    }

    [Fact]
    public void All_contains_exactly_the_canonical_values()
    {
        Assert.Equal(4, SignModality.All.Count);
        Assert.Contains(SignModality.Pictured, SignModality.All);
        Assert.Contains(SignModality.Spoken, SignModality.All);
        Assert.Contains(SignModality.Gestured, SignModality.All);
        Assert.Contains(SignModality.Written, SignModality.All);
    }

    [Fact]
    public void Repeatable_hash_is_stable_for_equal_values()
    {
        var a = SignModality.ParseInvariant("written");
        var b = SignModality.ParseInvariant("written");
        Assert.Equal(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }

    [Fact]
    public void Repeatable_hash_differs_between_distinct_modalities()
    {
        Assert.NotEqual(
            SignModality.Spoken.GetRepeatableHashCode(),
            SignModality.Written.GetRepeatableHashCode());
    }

    [Fact]
    public void TryParse_rejects_unknown_value()
    {
        Assert.False(SignModality.TryParse("signed", out _));
    }

    [Fact]
    public void Deserialize_from_json_string()
    {
        var modality = JsonSerializer.Deserialize<SignModality>("\"pictured\"");
        Assert.Equal(SignModality.Pictured, modality);
    }

    public static readonly TheoryData<string> Modalities =
    [
        "pictured",
        "spoken",
        "gestured",
        "written",
    ];
}
