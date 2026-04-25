// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// Regression tests for bugs identified in the DSE.Open.Numerics review and fixed in
/// the same change set. Grouped by bug rather than by source file so each fix is
/// covered by a self-contained set of assertions.
/// </summary>
public class BugFixesTests
{
    // ─────────────────────────────────────────────────────────────────────
    // Bug #1: NaInt<T> operator % was infinite recursion.
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void NaInt_Modulo_OnConcreteValues_ReturnsRemainder()
    {
        NaInt<int> a = 17;
        NaInt<int> b = 5;

        var result = a % b;

        Assert.False(result.IsNa);
        Assert.Equal(2, (int)result);
    }

    [Fact]
    public void NaInt_Modulo_LeftIsNa_ReturnsNa()
    {
        var result = NaInt<int>.Na % (NaInt<int>)5;
        Assert.True(result.IsNa);
    }

    [Fact]
    public void NaInt_Modulo_RightIsNa_ReturnsNa()
    {
        var result = (NaInt<int>)5 % NaInt<int>.Na;
        Assert.True(result.IsNa);
    }

    [Fact]
    public void NaInt_Modulo_DivideByZero_ReturnsNa()
    {
        var result = (NaInt<int>)10 % (NaInt<int>)0;
        Assert.True(result.IsNa);
    }

    // (Earlier revisions had a `DoesNotRecurseInfinitely` test that ran the
    // operator on a worker thread to "bound" a regression. StackOverflowException
    // terminates the entire process regardless of thread, so the wrapper provided
    // no actual safety; the four `NaInt_Modulo_*` tests above already exercise
    // the operator and would crash the test host if the recursion regressed.)

    // ─────────────────────────────────────────────────────────────────────
    // Bug #2: NaInt<T>.ToString(format)/TryFormat ignored IsNa.
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void NaInt_ToStringFormat_NaValue_ReturnsNaLabel()
    {
        Assert.Equal(NaValue.NaValueLabel, NaInt<int>.Na.ToString("G", CultureInfo.InvariantCulture));
        Assert.Equal(NaValue.NaValueLabel, NaInt<int>.Na.ToString(null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void NaInt_ToStringFormat_KnownValue_DelegatesToUnderlying()
    {
        var v = (NaInt<int>)42;
        Assert.Equal("42", v.ToString("G", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void NaInt_TryFormat_NaValue_WritesNaLabel()
    {
        Span<char> buffer = stackalloc char[8];
        var ok = ((ISpanFormattable)NaInt<int>.Na).TryFormat(buffer, out var written, default, CultureInfo.InvariantCulture);
        Assert.True(ok);
        Assert.Equal(NaValue.NaValueLabel, buffer[..written].ToString());
    }

    [Fact]
    public void NaInt_TryFormat_BufferTooSmallForNa_ReturnsFalse()
    {
        Span<char> buffer = stackalloc char[1];
        var ok = ((ISpanFormattable)NaInt<int>.Na).TryFormat(buffer, out var written, default, CultureInfo.InvariantCulture);
        Assert.False(ok);
        Assert.Equal(0, written);
    }

    [Fact]
    public void NaInt_TryFormat_KnownValue_DelegatesToUnderlying()
    {
        Span<char> buffer = stackalloc char[8];
        var ok = ((ISpanFormattable)(NaInt<int>)42).TryFormat(buffer, out var written, default, CultureInfo.InvariantCulture);
        Assert.True(ok);
        Assert.Equal("42", buffer[..written].ToString());
    }

    // ─────────────────────────────────────────────────────────────────────
    // Bug #3: NaInt<T>.PopCount didn't propagate Na.
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void NaInt_PopCount_NaInput_ReturnsNa()
    {
        var result = BinaryInteger_PopCount(NaInt<int>.Na);
        Assert.True(result.IsNa);
    }

    [Fact]
    public void NaInt_PopCount_KnownInput_DelegatesToUnderlying()
    {
        var result = BinaryInteger_PopCount((NaInt<int>)0b1011);
        Assert.False(result.IsNa);
        Assert.Equal(3, (int)result);
    }

    private static T BinaryInteger_PopCount<T>(T value) where T : IBinaryInteger<T>
    {
        return T.PopCount(value);
    }

    // ─────────────────────────────────────────────────────────────────────
    // Bug #4: VectorValue.ToString threw NotImplementedException for every
    // Na* variant. With the storage-encoding fix, an Na value should also
    // round-trip cleanly to "NA".
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void VectorValue_ToString_NaInt32Na_ReturnsNaLabel()
    {
        var v = VectorValue.FromNaInt32(NaInt<int>.Na);
        Assert.Equal(NaValue.NaValueLabel, v.ToString());
    }

    [Fact]
    public void VectorValue_ToString_NaInt32Value_ReturnsValue()
    {
        var v = VectorValue.FromNaInt32((NaInt<int>)42);
        Assert.Equal("42", v.ToString());
    }

    [Fact]
    public void VectorValue_ToString_NaFloat64Na_ReturnsNaLabel()
    {
        var v = VectorValue.FromNaFloat64(NaFloat<double>.Na);
        Assert.Equal(NaValue.NaValueLabel, v.ToString());
    }

    [Fact]
    public void VectorValue_ToString_NaFloat64Value_ReturnsValue()
    {
        var v = VectorValue.FromNaFloat64((NaFloat<double>)1.5);
        Assert.Equal("1.5", v.ToString());
    }

    [Fact]
    public void VectorValue_ToString_NaFloat64Value_IsCultureInvariant()
    {
        // Pin the formatting culture to one that uses ',' as the decimal separator
        // to confirm Na* float branches honour InvariantCulture (matching the
        // non-Na branches), not the ambient culture.
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");

            Assert.Equal("1.5", VectorValue.FromFloat64(1.5).ToString());
            Assert.Equal("1.5", VectorValue.FromNaFloat64((NaFloat<double>)1.5).ToString());
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }

    [Fact]
    public void VectorValue_ToString_NaDateTimeValue_IsCultureInvariant()
    {
        var dt = new DateTime(2026, 1, 15, 9, 30, 0, DateTimeKind.Utc);

        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");

            // The Na branch must format the DateTime with InvariantCulture, so it
            // produces the same string as the non-Na DateTime branch — not the
            // ambient culture's date format.
            Assert.Equal(
                VectorValue.FromDateTime(dt).ToString(),
                VectorValue.FromNaDateTime((NaValue<DateTime>)dt).ToString());
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }

    [Fact]
    public void VectorValue_ToString_NaBoolNa_ReturnsNaLabel()
    {
        var v = VectorValue.FromNaBoolean(default);
        Assert.Equal(NaValue.NaValueLabel, v.ToString());
    }

    [Fact]
    public void VectorValue_ToString_NaBoolFalse_ReturnsFalse()
    {
        var v = VectorValue.FromNaBoolean((NaValue<bool>)false);
        Assert.Equal(false.ToString(), v.ToString());
    }

    [Fact]
    public void VectorValue_ToString_NaCharNa_ReturnsNaLabel()
    {
        var v = VectorValue.FromNaChar(default);
        Assert.Equal(NaValue.NaValueLabel, v.ToString());
    }

    [Fact]
    public void VectorValue_ToString_NaStringNa_ReturnsNaLabel()
    {
        var v = VectorValue.FromNaString(default);
        Assert.Equal(NaValue.NaValueLabel, v.ToString());
    }

    [Fact]
    public void VectorValue_ToString_NaStringValue_ReturnsValue()
    {
        var v = VectorValue.FromNaString("hello");
        Assert.Equal("hello", v.ToString());
    }

    [Fact]
    public void VectorValue_NaRoundTrip_PreservesNa_ForAllNumericNaTypes()
    {
        Assert.True(VectorValue.FromNaInt8(NaInt<sbyte>.Na).ToNaInt8().IsNa);
        Assert.True(VectorValue.FromNaInt16(NaInt<short>.Na).ToNaInt16().IsNa);
        Assert.True(VectorValue.FromNaInt32(NaInt<int>.Na).ToNaInt32().IsNa);
        Assert.True(VectorValue.FromNaInt64(NaInt<long>.Na).ToNaInt64().IsNa);
        Assert.True(VectorValue.FromNaUInt8(NaInt<byte>.Na).ToNaUInt8().IsNa);
        Assert.True(VectorValue.FromNaUInt16(NaInt<ushort>.Na).ToNaUInt16().IsNa);
        Assert.True(VectorValue.FromNaUInt32(NaInt<uint>.Na).ToNaUInt32().IsNa);
        Assert.True(VectorValue.FromNaUInt64(NaInt<ulong>.Na).ToNaUInt64().IsNa);
        Assert.True(VectorValue.FromNaFloat16(NaFloat<Half>.Na).ToNaFloat16().IsNa);
        Assert.True(VectorValue.FromNaFloat32(NaFloat<float>.Na).ToNaFloat32().IsNa);
        Assert.True(VectorValue.FromNaFloat64(NaFloat<double>.Na).ToNaFloat64().IsNa);
        Assert.True(VectorValue.FromNaDateTime64(NaInt<DateTime64>.Na).ToNaDateTime64().IsNa);
    }

    [Fact]
    public void VectorValue_NaRoundTrip_PreservesNa_ForAllNaValueTypes()
    {
        Assert.True(VectorValue.FromNaBoolean(default).ToNaBoolean().IsNa);
        Assert.True(VectorValue.FromNaChar(default).ToNaChar().IsNa);
        Assert.True(VectorValue.FromNaString(default).ToNaString().IsNa);
        Assert.True(VectorValue.FromNaDateTime(default).ToNaDateTime().IsNa);
        Assert.True(VectorValue.FromNaDateTimeOffset(default).ToNaDateTimeOffset().IsNa);
    }

    [Fact]
    public void VectorValue_NaRoundTrip_PreservesValue_ForKnownInputs()
    {
        Assert.Equal((NaInt<int>)42, VectorValue.FromNaInt32((NaInt<int>)42).ToNaInt32());
        Assert.Equal(1.5, (double)VectorValue.FromNaFloat64((NaFloat<double>)1.5).ToNaFloat64());
        Assert.Equal((NaValue<bool>)true, VectorValue.FromNaBoolean((NaValue<bool>)true).ToNaBoolean());
        Assert.Equal((NaValue<char>)'x', VectorValue.FromNaChar((NaValue<char>)'x').ToNaChar());
    }

    // ─────────────────────────────────────────────────────────────────────
    // Bug #5: NaValue<T> threw NotImplementedException from
    // TernaryEquals/EqualOrBothNa/EqualOrEitherNa and Parse(string,...) /
    // TryParse(string,...).
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void NaValue_TernaryEquals_BothNa_ReturnsNa()
    {
        Assert.Equal(Trilean.Na, default(NaValue<int>).TernaryEquals(default));
    }

    [Fact]
    public void NaValue_TernaryEquals_EitherNa_ReturnsNa()
    {
        Assert.Equal(Trilean.Na, ((NaValue<int>)1).TernaryEquals(default));
        Assert.Equal(Trilean.Na, default(NaValue<int>).TernaryEquals(1));
    }

    [Fact]
    public void NaValue_TernaryEquals_EqualConcrete_ReturnsTrue()
    {
        Assert.Equal(Trilean.True, ((NaValue<int>)42).TernaryEquals(42));
    }

    [Fact]
    public void NaValue_TernaryEquals_DifferentConcrete_ReturnsFalse()
    {
        Assert.Equal(Trilean.False, ((NaValue<int>)1).TernaryEquals(2));
    }

    [Fact]
    public void NaValue_EqualOrBothNa_BothNa_ReturnsTrue()
    {
        Assert.True(default(NaValue<int>).EqualOrBothNa(default));
    }

    [Fact]
    public void NaValue_EqualOrBothNa_OnlyOneNa_ReturnsFalse()
    {
        Assert.False(((NaValue<int>)1).EqualOrBothNa(default));
        Assert.False(default(NaValue<int>).EqualOrBothNa(1));
    }

    [Fact]
    public void NaValue_EqualOrBothNa_BothEqualConcrete_ReturnsTrue()
    {
        Assert.True(((NaValue<int>)1).EqualOrBothNa(1));
    }

    [Fact]
    public void NaValue_EqualOrEitherNa_EitherNa_ReturnsTrue()
    {
        Assert.True(((NaValue<int>)1).EqualOrEitherNa(default));
        Assert.True(default(NaValue<int>).EqualOrEitherNa(1));
        Assert.True(default(NaValue<int>).EqualOrEitherNa(default));
    }

    [Fact]
    public void NaValue_EqualOrEitherNa_DifferentConcrete_ReturnsFalse()
    {
        Assert.False(((NaValue<int>)1).EqualOrEitherNa(2));
    }

    [Fact]
    public void NaValue_TryParseString_ConcreteValue_ReturnsValue()
    {
        var ok = NaValue<int>.TryParse("42", CultureInfo.InvariantCulture, out var result);
        Assert.True(ok);
        Assert.False(result.IsNa);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void NaValue_TryParseString_NaLabel_ReturnsNa()
    {
        var ok = NaValue<int>.TryParse(NaValue.NaValueLabel, CultureInfo.InvariantCulture, out var result);
        Assert.True(ok);
        Assert.True(result.IsNa);
    }

    [Fact]
    public void NaValue_TryParseString_Invalid_ReturnsFalse()
    {
        var ok = NaValue<int>.TryParse("not a number", CultureInfo.InvariantCulture, out _);
        Assert.False(ok);
    }

    [Fact]
    public void NaValue_TryParseString_NullInput_ReturnsFalse()
    {
        var ok = NaValue<int>.TryParse((string?)null, CultureInfo.InvariantCulture, out _);
        Assert.False(ok);
    }

    [Fact]
    public void NaValue_ParseString_ConcreteValue_ReturnsValue()
    {
        var result = NaValue<int>.Parse("42", CultureInfo.InvariantCulture);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void NaValue_ParseString_NullThrows()
    {
        Assert.Throws<ArgumentNullException>(() => NaValue<int>.Parse((string)null!, CultureInfo.InvariantCulture));
    }

    // ─────────────────────────────────────────────────────────────────────
    // Bug #6: DataFrame mutation didn't enforce equal column length.
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void DataFrame_Add_FirstColumn_AnyLengthAccepted()
    {
        var frame = new DataFrame();
        frame.Add(Series.Create([1, 2, 3, 4, 5], "a"));
        Assert.Single(frame);
    }

    [Fact]
    public void DataFrame_Add_MismatchedLength_Throws()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
        };

        Assert.Throws<NumericsArgumentException>(
            () => frame.Add(Series.Create([1, 2], "b")));
    }

    [Fact]
    public void DataFrame_Insert_MismatchedLength_Throws()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
        };

        Assert.Throws<NumericsArgumentException>(
            () => frame.Insert(0, Series.Create([1, 2], "b")));
    }

    [Fact]
    public void DataFrame_IndexerSetByIndex_MismatchedLength_Throws()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
            Series.Create([4, 5, 6], "b"),
        };

        Assert.Throws<NumericsArgumentException>(
            () => frame[0] = Series.Create([1, 2], "a2"));
    }

    [Fact]
    public void DataFrame_IndexerSetByIndex_SameLength_Replaces()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
            Series.Create([4, 5, 6], "b"),
        };

        var replacement = Series.Create([7, 8, 9], "a2");
        frame[0] = replacement;

        Assert.Same(replacement, frame[0]);
    }

    [Fact]
    public void DataFrame_IndexerSetByName_MismatchedLength_Throws()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
            Series.Create([4, 5, 6], "b"),
        };

        Assert.Throws<NumericsArgumentException>(
            () => frame["a"] = Series.Create([1, 2], "a"));
    }

    [Fact]
    public void DataFrame_IndexerSetByName_NewName_Appends()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
        };

        frame["b"] = Series.Create([4, 5, 6]);

        Assert.Equal(2, frame.Count);
        Assert.Equal("b", frame[1].Name);
    }

    [Fact]
    public void DataFrame_IndexerSetByName_NewName_MismatchedLength_Throws()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
        };

        Assert.Throws<NumericsArgumentException>(
            () => frame["b"] = Series.Create([4, 5]));
    }

    [Fact]
    public void DataFrame_IndexerSetByIndex_SingleColumnFrame_AnyLengthAccepted()
    {
        // With only one column, replacing it has no other column to constrain length against.
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
        };

        frame[0] = Series.Create([4, 5, 6, 7, 8], "a2");
        Assert.Equal(5, frame[0].Length);
    }

    // ─────────────────────────────────────────────────────────────────────
    // Bug #7: ReadOnlySeries<T>.Equals(object) checked `obj is Series<T>`
    // and so never returned true for ReadOnlySeries-vs-ReadOnlySeries via
    // object equality.
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void ReadOnlySeries_EqualsObject_ReadOnlySeriesWithSameData_ReturnsTrue()
    {
        var a = new ReadOnlySeries<int>([1, 2, 3]);
        var b = new ReadOnlySeries<int>([1, 2, 3]);

        Assert.True(a.Equals((object)b));
    }

    [Fact]
    public void ReadOnlySeries_EqualsObject_ReadOnlySeriesWithDifferentData_ReturnsFalse()
    {
        var a = new ReadOnlySeries<int>([1, 2, 3]);
        var b = new ReadOnlySeries<int>([1, 2, 4]);

        Assert.False(a.Equals((object)b));
    }

    [Fact]
    public void ReadOnlySeries_EqualsObject_MutableSeriesWithSameData_ReturnsTrue()
    {
        var a = new ReadOnlySeries<int>([1, 2, 3]);
        var b = Series.Create([1, 2, 3]);

        Assert.True(a.Equals((object)b));
    }

    [Fact]
    public void ReadOnlySeries_EqualsObject_NonSeriesObject_ReturnsFalse()
    {
        var a = new ReadOnlySeries<int>([1, 2, 3]);
        Assert.False(a.Equals("not a series"));
    }

    [Fact]
    public void Series_EqualsObject_ReadOnlySeriesWithSameData_ReturnsTrue()
    {
        var a = Series.Create([1, 2, 3]);
        var b = new ReadOnlySeries<int>([1, 2, 3]);

        Assert.True(a.Equals((object)b));
    }
}
