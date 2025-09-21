// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

public class NaIntTests
{
    [Fact]
    public void Add()
    {
        NaInt<int>[] sequence1 = [1, 2, 3, 4, 5];
        NaInt<int>[] sequence2 = [1, 2, 3, 4, 5];
        var dest = new NaInt<int>[5];
        NaNumberPrimitives.Add(sequence1, sequence2, dest);

        Assert.Equal([2, 4, 6, 8, 10], dest);
    }

    [Fact]
    public void Add_Na()
    {
        NaInt<int>[] sequence1 = [1, 2, 3, 4, 5];
        NaInt<int>[] sequence2 = [1, 2, 3, 4, NaInt<int>.Na];
        var dest = new NaInt<int>[5];
        NaNumberPrimitives.Add(sequence1, sequence2, dest);

        Assert.Equal([2, 4, 6, 8, NaInt<int>.Sentinel], MemoryMarshal.Cast<NaInt<int>, int>(dest));
    }

    [Fact]
    public void Sum()
    {
        NaInt<int>[] sequence = [1, 2, 3, 4, 5];
        var sum = NaNumberPrimitives.Sum(sequence);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Sum_Na()
    {
        NaInt<int>[] sequence = [1, 2, 3, NaInt<int>.Na, 4, 5, null, 6, 7, 8];
        var sum = NaNumberPrimitives.Sum(sequence);
        Assert.Equal("NA", sum.ToString());
    }

    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        var naInt = new NaInt<int>(42);
        Assert.False(naInt.IsNa);
        Assert.Equal(42, (int)naInt);
    }

    [Fact]
    public void Constructor_WithSentinelValue_ThrowsException()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new NaInt<int>(int.MaxValue));
    }

    [Fact]
    public void ImplicitConversion_FromT_CreatesNaInt()
    {
        NaInt<int> naInt = 42;
        Assert.False(naInt.IsNa);
        Assert.Equal(42, (int)naInt);
    }

    [Fact]
    public void ImplicitConversion_FromNullableT_WithValue_CreatesNaInt()
    {
        int? value = 42;
        NaInt<int> naInt = value;
        Assert.False(naInt.IsNa);
        Assert.Equal(42, (int)naInt);
    }

    [Fact]
    public void ImplicitConversion_FromNullableT_WithNull_CreatesNa()
    {
        int? value = null;
        NaInt<int> naInt = value;
        Assert.True(naInt.IsNa);
    }

    [Fact]
    public void ExplicitConversion_ToT_WithValue_ReturnsValue()
    {
        NaInt<int> naInt = 42;
        var value = (int)naInt;
        Assert.Equal(42, value);
    }

    [Fact]
    public void ExplicitConversion_ToT_WithNa_ThrowsException()
    {
        var naInt = NaInt<int>.Na;
        _ = Assert.Throws<NaValueException>(() => (int)naInt);
    }

    [Fact]
    public void Na_IsNa_ReturnsTrue()
    {
        var na = NaInt<int>.Na;
        Assert.True(na.IsNa);
    }

    [Fact]
    public void ToString_WithValue_ReturnsValueString()
    {
        NaInt<int> naInt = 42;
        Assert.Equal("42", naInt.ToString());
    }

    [Fact]
    public void ToString_WithNa_ReturnsNa()
    {
        var naInt = NaInt<int>.Na;
        Assert.Equal("NA", naInt.ToString());
    }

    [Fact]
    public void IsNaN_WithNa_ReturnsTrue()
    {
        var naInt = NaInt<int>.Na;
        Assert.True(NaInt<int>.IsNaN(naInt));
    }

    [Fact]
    public void IsNaN_WithValue_ReturnsFalse()
    {
        NaInt<int> naInt = 42;
        Assert.False(NaInt<int>.IsNaN(naInt));
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        Assert.Equal(int.MaxValue - 1, (int)NaInt<int>.MaxValue);
    }

    [Fact]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 43;
        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_BothNa_ReturnsTrue()
    {
        var a = NaInt<int>.Na;
        var b = NaInt<int>.Na;
        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_OneNa_ReturnsFalse()
    {
        NaInt<int> a = 42;
        var b = NaInt<int>.Na;
        Assert.False(a.Equals(b));
        Assert.False(b.Equals(a));
    }

    [Fact]
    public void EqualityOperator_WithSameValues_ReturnsFalse()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 42;
        Assert.True(a == b);
    }

    [Fact]
    public void EqualityOperator_WithDifferentValues_ReturnsFalse()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 43;
        Assert.False(a == b);
    }

    [Fact]
    public void EqualityOperator_BothNa_ReturnsFalse()
    {
        var a = NaInt<int>.Na;
        var b = NaInt<int>.Na;
        Assert.False(a == b);
    }

    [Fact]
    public void InequalityOperator_WithSameValues_ReturnsFalse()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 42;
        Assert.False(a != b);
    }

    [Fact]
    public void InequalityOperator_WithDifferentValues_ReturnsTrue()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 43;
        Assert.True(a != b);
    }

    [Fact]
    public void InequalityOperator_BothNa_ReturnsTrue()
    {
        var a = NaInt<int>.Na;
        var b = NaInt<int>.Na;
        Assert.True(a != b);
    }

    [Fact]
    public void TernaryEquals_WithSameValues_ReturnsTrue()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 42;
        Assert.Equal(Trilean.True, a.TernaryEquals(b));
    }

    [Fact]
    public void TernaryEquals_WithDifferentValues_ReturnsFalse()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 43;
        Assert.Equal(Trilean.False, a.TernaryEquals(b));
    }

    [Fact]
    public void TernaryEquals_WithOneNa_ReturnsNa()
    {
        NaInt<int> a = 42;
        var b = NaInt<int>.Na;
        Assert.Equal(Trilean.Na, a.TernaryEquals(b));
        Assert.Equal(Trilean.Na, b.TernaryEquals(a));
    }

    [Fact]
    public void TernaryEquals_BothNa_ReturnsNa()
    {
        var a = NaInt<int>.Na;
        var b = NaInt<int>.Na;
        Assert.Equal(Trilean.Na, a.TernaryEquals(b));
    }

    [Fact]
    public void EqualAndNotNa_WithSameValuesAndNotNa_ReturnsTrue()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 42;
        Assert.True(a.EqualAndNotNa(b));
    }

    [Fact]
    public void EqualAndNotNa_WithDifferentValuesAndNotNa_ReturnsFalse()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 43;
        Assert.False(a.EqualAndNotNa(b));
    }

    [Fact]
    public void EqualAndNotNa_WithOneOrBothNa_ReturnsFalse()
    {
        NaInt<int> a = 42;
        var na = NaInt<int>.Na;

        Assert.False(a.EqualAndNotNa(na));
        Assert.False(na.EqualAndNotNa(a));
        Assert.False(na.EqualAndNotNa(na));
    }

    [Fact]
    public void EqualOrBothNa_WithSameValues_ReturnsTrue()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 42;
        Assert.True(a.EqualOrBothNa(b));
    }

    [Fact]
    public void EqualOrBothNa_WithDifferentValues_ReturnsFalse()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 43;
        Assert.False(a.EqualOrBothNa(b));
    }

    [Fact]
    public void EqualOrBothNa_WithOneNa_ReturnsFalse()
    {
        NaInt<int> a = 42;
        var na = NaInt<int>.Na;

        Assert.False(a.EqualOrBothNa(na));
        Assert.False(na.EqualOrBothNa(a));
    }

    [Fact]
    public void EqualOrBothNa_BothNa_ReturnsTrue()
    {
        var a = NaInt<int>.Na;
        var b = NaInt<int>.Na;
        Assert.True(a.EqualOrBothNa(b));
    }

    [Fact]
    public void EqualOrEitherNa_WithSameValues_ReturnsTrue()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 42;
        Assert.True(a.EqualOrEitherNa(b));
    }

    [Fact]
    public void EqualOrEitherNa_WithDifferentValues_ReturnsFalse()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 43;
        Assert.False(a.EqualOrEitherNa(b));
    }

    [Fact]
    public void EqualOrEitherNa_WithOneNa_ReturnsTrue()
    {
        NaInt<int> a = 42;
        var na = NaInt<int>.Na;

        Assert.True(a.EqualOrEitherNa(na));
        Assert.True(na.EqualOrEitherNa(a));
    }

    [Fact]
    public void EqualOrEitherNa_BothNa_ReturnsTrue()
    {
        var a = NaInt<int>.Na;
        var b = NaInt<int>.Na;
        Assert.True(a.EqualOrEitherNa(b));
    }

    [Fact]
    public void Add_WithValues_ReturnsSum()
    {
        NaInt<int> a = 40;
        NaInt<int> b = 2;
        var result = a + b;

        Assert.False(result.IsNa);
        Assert.Equal(42, (int)result);
    }

    [Fact]
    public void Add_WithOneNa_ReturnsNa()
    {
        NaInt<int> a = 40;
        var na = NaInt<int>.Na;

        Assert.True((a + na).IsNa);
        Assert.True((na + a).IsNa);
    }

    [Fact]
    public void Add_BothNa_ReturnsNa()
    {
        var a = NaInt<int>.Na;
        var b = NaInt<int>.Na;

        Assert.True((a + b).IsNa);
    }

    [Fact]
    public void Add_WithTValue_ReturnsSum()
    {
        NaInt<int> a = 40;
        var result = a + 2;

        Assert.False(result.IsNa);
        Assert.Equal(42, (int)result);
    }

    [Fact]
    public void Subtract_WithValues_ReturnsDifference()
    {
        NaInt<int> a = 44;
        NaInt<int> b = 2;
        var result = a - b;

        Assert.False(result.IsNa);
        Assert.Equal(42, (int)result);
    }

    [Fact]
    public void Subtract_WithOneNa_ReturnsNa()
    {
        NaInt<int> a = 40;
        var na = NaInt<int>.Na;

        Assert.True((a - na).IsNa);
        Assert.True((na - a).IsNa);
    }

    [Fact]
    public void Multiply_WithValues_ReturnsProduct()
    {
        NaInt<int> a = 21;
        NaInt<int> b = 2;
        var result = a * b;

        Assert.False(result.IsNa);
        Assert.Equal(42, (int)result);
    }

    [Fact]
    public void Multiply_WithOneNa_ReturnsNa()
    {
        NaInt<int> a = 40;
        var na = NaInt<int>.Na;

        Assert.True((a * na).IsNa);
        Assert.True((na * a).IsNa);
    }

    [Fact]
    public void Divide_WithValues_ReturnsQuotient()
    {
        NaInt<int> a = 84;
        NaInt<int> b = 2;
        var result = a / b;

        Assert.False(result.IsNa);
        Assert.Equal(42, (int)result);
    }

    [Fact]
    public void Divide_WithOneNa_ReturnsNa()
    {
        NaInt<int> a = 40;
        var na = NaInt<int>.Na;

        Assert.True((a / na).IsNa);
        Assert.True((na / a).IsNa);
    }

    [Fact]
    public void Divide_ByZero_ReturnsNa()
    {
        NaInt<int> a = 40;
        NaInt<int> zero = 0;

        Assert.True((a / zero).IsNa);
    }

    [Fact]
    public void UnaryMinus_WithValue_ReturnsNegated()
    {
        NaInt<int> a = 42;
        var result = -a;

        Assert.False(result.IsNa);
        Assert.Equal(-42, (int)result);
    }

    [Fact]
    public void UnaryMinus_WithNa_ReturnsNa()
    {
        var na = NaInt<int>.Na;
        var result = -na;

        Assert.True(result.IsNa);
    }

    [Fact]
    public void Parse_ValidString_ReturnsValue()
    {
        var result = NaInt<int>.Parse("42", null);
        Assert.False(result.IsNa);
        Assert.Equal(42, (int)result);
    }

    [Fact]
    public void TryParse_ValidString_ReturnsTrue()
    {
        var success = NaInt<int>.TryParse("42", null, out var result);
        Assert.True(success);
        Assert.False(result.IsNa);
        Assert.Equal(42, (int)result);
    }

    [Fact]
    public void TryParse_InvalidString_ReturnsFalse()
    {
        var success = NaInt<int>.TryParse("not a number", null, out _);
        Assert.False(success);
    }

    [Fact]
    public void GetByteCount_WithValue_ReturnsCount()
    {
        NaInt<int> value = 42;
        var count = ((IBinaryInteger<NaInt<int>>)value).GetByteCount();
        Assert.Equal(sizeof(int), count);
    }

    [Fact]
    public void TryWriteBytes_WithValue_WritesBytes()
    {
        NaInt<int> value = 42;
        Span<byte> buffer = new byte[sizeof(int)];

        var success = ((IBinaryInteger<NaInt<int>>)value).TryWriteLittleEndian(buffer, out var bytesWritten);

        Assert.True(success);
        Assert.Equal(sizeof(int), bytesWritten);

        // Verify the bytes represent 42
        var result = BitConverter.ToInt32(buffer);
        Assert.Equal(42, result);
    }

    [Fact]
    public void MaxValue_ReturnsExpectedValue()
    {
        Assert.Equal(int.MaxValue - 1, (int)NaInt<int>.MaxValue);
    }

    [Fact]
    public void MinValue_ReturnsExpectedValue()
    {
        Assert.Equal(int.MinValue, (int)NaInt<int>.MinValue);
    }

    [Fact]
    public void BitwiseAnd_WithValues_ReturnsResult()
    {
        NaInt<int> a = 6; // 110 in binary
        NaInt<int> b = 3; // 011 in binary
        var result = a & b;

        Assert.False(result.IsNa);
        Assert.Equal(2, (int)result); // 010 in binary
    }

    [Fact]
    public void BitwiseAnd_WithNa_ReturnsNa()
    {
        NaInt<int> a = 6;
        var na = NaInt<int>.Na;

        Assert.True((a & na).IsNa);
        Assert.True((na & a).IsNa);
    }

    [Fact]
    public void BitwiseOr_WithValues_ReturnsResult()
    {
        NaInt<int> a = 6; // 110 in binary
        NaInt<int> b = 3; // 011 in binary
        var result = a | b;

        Assert.False(result.IsNa);
        Assert.Equal(7, (int)result); // 111 in binary
    }

    [Fact]
    public void BitwiseXor_WithValues_ReturnsResult()
    {
        NaInt<int> a = 6; // 110 in binary
        NaInt<int> b = 3; // 011 in binary
        var result = a ^ b;

        Assert.False(result.IsNa);
        Assert.Equal(5, (int)result); // 101 in binary
    }

    [Fact]
    public void BitwiseNot_WithValue_ReturnsResult()
    {
        NaInt<int> a = 6; // 110 in binary
        var result = ~a;

        Assert.False(result.IsNa);
        Assert.Equal(~6, (int)result);
    }

    [Fact]
    public void BitwiseNot_WithNa_ReturnsNa()
    {
        var na = NaInt<int>.Na;
        var result = ~na;

        Assert.True(result.IsNa);
    }

    [Fact]
    public void LeftShift_WithValue_ReturnsResult()
    {
        NaInt<int> a = 1; // 0001 in binary
        var result = a << 2;

        Assert.False(result.IsNa);
        Assert.Equal(4, (int)result); // 0100 in binary
    }

    [Fact]
    public void RightShift_WithValue_ReturnsResult()
    {
        NaInt<int> a = 4; // 0100 in binary
        var result = a >> 2;

        Assert.False(result.IsNa);
        Assert.Equal(1, (int)result); // 0001 in binary
    }

    [Fact]
    public void GetHashCode_WithValue_ReturnsExpectedHashCode()
    {
        NaInt<int> value = 42;
        Assert.Equal(42.GetHashCode(), value.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithNa_ReturnsZero()
    {
        var na = NaInt<int>.Na;
        Assert.Equal(0, na.GetHashCode());
    }

    [Fact]
    public void CompareTo_WithValues_ReturnsComparison()
    {
        NaInt<int> a = 42;
        NaInt<int> b = 43;

        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(a) > 0);
        Assert.Equal(0, a.CompareTo(a));
    }

    [Fact]
    public void CompareTo_WithNa_ReturnsZero()
    {
        NaInt<int> a = 42;
        var na = NaInt<int>.Na;

        Assert.Equal(0, a.CompareTo(na));
        Assert.Equal(0, na.CompareTo(a));
        Assert.Equal(0, na.CompareTo(na));
    }

    [Fact]
    public void Parse_EmptyString_ThrowsFormatException()
    {
        _ = Assert.Throws<FormatException>(() => NaInt<int>.Parse("", null));
    }

    [Fact]
    public void Parse_SentinelValue_ThrowsException()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => NaInt<int>.Parse(int.MaxValue.ToString(), null));
    }

    [Fact]
    public void TryParse_EmptyString_ReturnsFalse()
    {
        var success = NaInt<int>.TryParse("", null, out _);
        Assert.False(success);
    }

    [Fact]
    public void UnaryPlus_WithValue_ReturnsSameValue()
    {
        NaInt<int> a = 42;
        var result = +a;

        Assert.False(result.IsNa);
        Assert.Equal(42, (int)result);
    }

    [Fact]
    public void UnaryPlus_WithNa_ReturnsNa()
    {
        var na = NaInt<int>.Na;
        var result = +na;

        Assert.True(result.IsNa);
    }
}
