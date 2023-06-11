// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.TestValues;

[ComparableValue]
[JsonConverter(typeof(JsonInt32ValueConverter<AgreementLevel>))]
public readonly partial struct AgreementLevel : IComparableValue<AgreementLevel, int>
{
    static int ISpanSerializable<AgreementLevel>.MaxSerializedCharLength { get; } = 128; // TODO

    public static readonly AgreementLevel DisagreeStrongly = new(-2);
    public static readonly AgreementLevel Disagree = new(-1);
    public static readonly AgreementLevel NeitherAgreeNorDisagree = new(0);
    public static readonly AgreementLevel Agree = new(1);
    public static readonly AgreementLevel AgreeStrongly = new(2);

    public static IEnumerable<AgreementLevel> ValidValues { get; } = new[]
    {
        DisagreeStrongly,
        Disagree,
        NeitherAgreeNorDisagree,
        Agree,
        AgreeStrongly
    };

    public static bool IsValidValue(int value) => value is >= (-1) and <= 2;

}
