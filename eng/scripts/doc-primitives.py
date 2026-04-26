#!/usr/bin/env python3
"""Insert XML doc summaries above public methods in VectorPrimitives.* and
SeriesPrimitives.* files. The patterns are highly uniform (Add, Subtract,
Multiply, Divide all share the same overload shape); this script avoids
hand-writing 1,500+ near-identical doc-strings.

Usage: python doc-primitives.py <file>... — operates in place.
"""

from __future__ import annotations

import re
import sys
from pathlib import Path

# Match a method declaration line that lacks a doc comment immediately above it.
# Captures the method name and a coarse signature. The generic argument list
# `<...>` is optional so non-generic methods (e.g. ConvertToHalf, Equals(object,
# object)) also match.
PUBLIC_METHOD_RE = re.compile(
    r"^(?P<indent>\s*)(?P<sig>public\s+(?:static\s+)?(?:partial\s+)?[^\n;{]+?\s+(?P<name>[A-Z][A-Za-z0-9_]*)\s*(?:<[^>]*>)?\s*\([^)]*\))",
    re.MULTILINE,
)

def _make_in_place(name: str, summary: str) -> tuple[str, str]:
    """Derive an InPlace summary from a paired non-InPlace name."""
    return f"{name}InPlace", f"{summary[:-1]} (in place)."


SUMMARY_TEMPLATES = {
    # binary ops
    "Add": "Element-wise <c>x + y</c>.",
    "Subtract": "Element-wise <c>x - y</c>.",
    "Multiply": "Element-wise <c>x * y</c>.",
    "Divide": "Element-wise <c>x / y</c>.",
    "AddInPlace": "Element-wise <c>x += y</c> in place.",
    "SubtractInPlace": "Element-wise <c>x -= y</c> in place.",
    "MultiplyInPlace": "Element-wise <c>x *= y</c> in place.",
    "DivideInPlace": "Element-wise <c>x /= y</c> in place.",

    # comparisons
    "GreaterThan": "Element-wise <c>x &gt; y</c> comparison.",
    "GreaterThanOrEqual": "Element-wise <c>x &gt;= y</c> comparison.",
    "LessThan": "Element-wise <c>x &lt; y</c> comparison.",
    "LessThanOrEqual": "Element-wise <c>x &lt;= y</c> comparison.",
    "Equals": "Element-wise <c>x == y</c> comparison.",
    "SequenceEqual": "Returns <see langword=\"true\"/> when the two sequences contain the same elements in order.",

    # unary math
    "Abs": "Element-wise absolute value.",
    "Negate": "Element-wise unary negation.",
    "Sqrt": "Element-wise square root.",
    "Sign": "Element-wise sign (-1, 0, or 1).",
    "Exp": "Element-wise <c>e^x</c>.",
    "Exp2": "Element-wise <c>2^x</c>.",
    "Exp10": "Element-wise <c>10^x</c>.",
    "ExpM1": "Element-wise <c>e^x - 1</c>.",
    "Exp2M1": "Element-wise <c>2^x - 1</c>.",
    "Exp10M1": "Element-wise <c>10^x - 1</c>.",
    "Log": "Element-wise natural log.",
    "Log2": "Element-wise base-2 log.",
    "Log10": "Element-wise base-10 log.",
    "LogP1": "Element-wise <c>log(1 + x)</c>.",
    "Log2P1": "Element-wise <c>log2(1 + x)</c>.",
    "Log10P1": "Element-wise <c>log10(1 + x)</c>.",
    "Pow": "Element-wise <c>x^y</c>.",
    "Cbrt": "Element-wise cube root.",
    "Hypot": "Element-wise <c>sqrt(x*x + y*y)</c>.",
    "RootN": "Element-wise <c>n</c>-th root.",
    "ReciprocalEstimate": "Element-wise approximate reciprocal (<c>1/x</c>).",
    "ReciprocalSqrtEstimate": "Element-wise approximate reciprocal of the square root (<c>1/sqrt(x)</c>).",

    # trig
    "Sin": "Element-wise sine.",
    "Cos": "Element-wise cosine.",
    "Tan": "Element-wise tangent.",
    "Asin": "Element-wise inverse sine.",
    "Acos": "Element-wise inverse cosine.",
    "Atan": "Element-wise inverse tangent.",
    "Atan2": "Element-wise <c>atan2(x, y)</c>.",
    "SinCos": "Element-wise sine and cosine.",
    "SinPi": "Element-wise <c>sin(pi*x)</c>.",
    "CosPi": "Element-wise <c>cos(pi*x)</c>.",
    "TanPi": "Element-wise <c>tan(pi*x)</c>.",
    "AsinPi": "Element-wise <c>asin(x)/pi</c>.",
    "AcosPi": "Element-wise <c>acos(x)/pi</c>.",
    "AtanPi": "Element-wise <c>atan(x)/pi</c>.",
    "Atan2Pi": "Element-wise <c>atan2(x, y)/pi</c>.",
    "SinCosPi": "Element-wise <c>(sin(pi*x), cos(pi*x))</c>.",
    "Sinh": "Element-wise hyperbolic sine.",
    "Cosh": "Element-wise hyperbolic cosine.",
    "Tanh": "Element-wise hyperbolic tangent.",
    "Asinh": "Element-wise inverse hyperbolic sine.",
    "Acosh": "Element-wise inverse hyperbolic cosine.",
    "Atanh": "Element-wise inverse hyperbolic tangent.",
    "DegreesToRadians": "Element-wise conversion from degrees to radians.",
    "RadiansToDegrees": "Element-wise conversion from radians to degrees.",

    # rounding
    "Floor": "Element-wise floor.",
    "Ceiling": "Element-wise ceiling.",
    "Round": "Element-wise rounding.",
    "Truncate": "Element-wise truncation toward zero.",

    # reductions
    "Sum": "Returns the sum of all elements.",
    "SumChecked": "Returns the overflow-checked sum of all elements.",
    "Product": "Returns the product of all elements.",
    "Mean": "Returns the arithmetic mean of all elements.",
    "Median": "Returns the median of all elements.",
    "Mode": "Returns the most frequently occurring element.",
    "Min": "Returns the minimum element.",
    "Max": "Returns the maximum element.",
    "IndexOfMin": "Returns the index of the minimum element.",
    "IndexOfMax": "Returns the index of the maximum element.",
    "GeometricMean": "Returns the geometric mean of all elements.",
    "HarmonicMean": "Returns the harmonic mean of all elements.",
    "SumOfSquares": "Returns the sum of <c>x_i * x_i</c>.",
    "SumOfMagnitudes": "Returns the sum of <c>|x_i|</c>.",
    "Variance": "Returns the sample variance.",
    "PopulationVariance": "Returns the population variance.",
    "StandardDeviation": "Returns the sample standard deviation.",
    "PopulationStandardDeviation": "Returns the population standard deviation.",
    "Norm": "Returns the Euclidean (L2) norm.",
    "Distance": "Returns the Euclidean distance between <paramref name=\"x\"/> and <paramref name=\"y\"/>.",
    "Dot": "Returns the dot product of <paramref name=\"x\"/> and <paramref name=\"y\"/>.",
    "CosineSimilarity": "Returns the cosine similarity between <paramref name=\"x\"/> and <paramref name=\"y\"/>.",

    # bitwise
    "And": "Element-wise bitwise AND.",
    "Or": "Element-wise bitwise OR.",
    "Xor": "Element-wise bitwise XOR.",
    "OnesComplement": "Element-wise bitwise NOT (one's complement).",
    "PopCount": "Element-wise population count (number of set bits).",
    "LeadingZeroCount": "Element-wise count of leading zero bits.",
    "TrailingZeroCount": "Element-wise count of trailing zero bits.",
    "ShiftLeft": "Element-wise left shift.",
    "ShiftRightArithmetic": "Element-wise arithmetic right shift.",
    "ShiftRightLogical": "Element-wise logical (unsigned) right shift.",
    "RotateLeft": "Element-wise rotate-left.",
    "RotateRight": "Element-wise rotate-right.",

    # predicates
    "IsCanonical": "Element-wise <c>IsCanonical</c> predicate.",
    "IsComplexNumber": "Element-wise <c>IsComplexNumber</c> predicate.",
    "IsEvenInteger": "Element-wise <c>IsEvenInteger</c> predicate.",
    "IsFinite": "Element-wise <c>IsFinite</c> predicate.",
    "IsImaginaryNumber": "Element-wise <c>IsImaginaryNumber</c> predicate.",
    "IsInfinity": "Element-wise <c>IsInfinity</c> predicate.",
    "IsInteger": "Element-wise <c>IsInteger</c> predicate.",
    "IsNaN": "Element-wise <c>IsNaN</c> predicate.",
    "IsNan": "Element-wise <c>IsNaN</c> predicate.",
    "IsNegative": "Element-wise <c>IsNegative</c> predicate.",
    "IsNegativeInfinity": "Element-wise <c>IsNegativeInfinity</c> predicate.",
    "IsNormal": "Element-wise <c>IsNormal</c> predicate.",
    "IsOddInteger": "Element-wise <c>IsOddInteger</c> predicate.",
    "IsPositive": "Element-wise <c>IsPositive</c> predicate.",
    "IsPositiveInfinity": "Element-wise <c>IsPositiveInfinity</c> predicate.",
    "IsPow2": "Element-wise <c>IsPow2</c> predicate.",
    "IsPow2InPlace": "Element-wise <c>IsPow2</c> predicate written back to <paramref name=\"x\"/>.",
    "IsRealNumber": "Element-wise <c>IsRealNumber</c> predicate.",
    "IsSubnormal": "Element-wise <c>IsSubnormal</c> predicate.",
    "IsZero": "Element-wise <c>IsZero</c> predicate.",
    "AnyIsCanonical": "Returns <see langword=\"true\"/> when any element matches <c>IsCanonical</c>.",
    "AnyIsFinite": "Returns <see langword=\"true\"/> when any element matches <c>IsFinite</c>.",
    "AnyIsInfinity": "Returns <see langword=\"true\"/> when any element matches <c>IsInfinity</c>.",
    "AnyIsInteger": "Returns <see langword=\"true\"/> when any element matches <c>IsInteger</c>.",
    "AnyIsNaN": "Returns <see langword=\"true\"/> when any element matches <c>IsNaN</c>.",
    "AnyIsNan": "Returns <see langword=\"true\"/> when any element matches <c>IsNaN</c>.",
    "AnyIsNegative": "Returns <see langword=\"true\"/> when any element matches <c>IsNegative</c>.",
    "AnyIsNegativeInfinity": "Returns <see langword=\"true\"/> when any element matches <c>IsNegativeInfinity</c>.",
    "AnyIsNormal": "Returns <see langword=\"true\"/> when any element matches <c>IsNormal</c>.",
    "AnyIsPositive": "Returns <see langword=\"true\"/> when any element matches <c>IsPositive</c>.",
    "AnyIsPositiveInfinity": "Returns <see langword=\"true\"/> when any element matches <c>IsPositiveInfinity</c>.",
    "AnyIsPow2": "Returns <see langword=\"true\"/> when any element is a power of two.",
    "AnyIsRealNumber": "Returns <see langword=\"true\"/> when any element matches <c>IsRealNumber</c>.",
    "AnyIsSubnormal": "Returns <see langword=\"true\"/> when any element matches <c>IsSubnormal</c>.",
    "AnyIsZero": "Returns <see langword=\"true\"/> when any element is zero.",
    "AnyIsEvenInteger": "Returns <see langword=\"true\"/> when any element is an even integer.",
    "AnyIsOddInteger": "Returns <see langword=\"true\"/> when any element is an odd integer.",
    "AnyIsImaginaryNumber": "Returns <see langword=\"true\"/> when any element is imaginary.",
    "AnyIsComplexNumber": "Returns <see langword=\"true\"/> when any element is complex.",
    "AllIsCanonical": "Returns <see langword=\"true\"/> when every element matches <c>IsCanonical</c>.",
    "AllIsFinite": "Returns <see langword=\"true\"/> when every element matches <c>IsFinite</c>.",
    "AllIsInfinity": "Returns <see langword=\"true\"/> when every element matches <c>IsInfinity</c>.",
    "AllIsInteger": "Returns <see langword=\"true\"/> when every element matches <c>IsInteger</c>.",
    "AllIsNaN": "Returns <see langword=\"true\"/> when every element matches <c>IsNaN</c>.",
    "AllIsNan": "Returns <see langword=\"true\"/> when every element matches <c>IsNaN</c>.",
    "AllIsNegative": "Returns <see langword=\"true\"/> when every element matches <c>IsNegative</c>.",
    "AllIsNegativeInfinity": "Returns <see langword=\"true\"/> when every element matches <c>IsNegativeInfinity</c>.",
    "AllIsNormal": "Returns <see langword=\"true\"/> when every element matches <c>IsNormal</c>.",
    "AllIsPositive": "Returns <see langword=\"true\"/> when every element matches <c>IsPositive</c>.",
    "AllIsPositiveInfinity": "Returns <see langword=\"true\"/> when every element matches <c>IsPositiveInfinity</c>.",
    "AllIsPow2": "Returns <see langword=\"true\"/> when every element is a power of two.",
    "AllIsRealNumber": "Returns <see langword=\"true\"/> when every element matches <c>IsRealNumber</c>.",
    "AllIsSubnormal": "Returns <see langword=\"true\"/> when every element matches <c>IsSubnormal</c>.",
    "AllIsZero": "Returns <see langword=\"true\"/> when every element is zero.",
    "AllIsEvenInteger": "Returns <see langword=\"true\"/> when every element is an even integer.",
    "AllIsOddInteger": "Returns <see langword=\"true\"/> when every element is an odd integer.",
    "AllIsImaginaryNumber": "Returns <see langword=\"true\"/> when every element is imaginary.",
    "AllIsComplexNumber": "Returns <see langword=\"true\"/> when every element is complex.",

    # other
    "Clamp": "Element-wise clamp into <c>[low, high]</c>.",
    "ConvertChecked": "Element-wise conversion to <typeparamref name=\"TTo\"/> with overflow checking.",
    "ConvertSaturating": "Element-wise conversion to <typeparamref name=\"TTo\"/>, saturating on overflow.",
    "ConvertTruncating": "Element-wise conversion to <typeparamref name=\"TTo\"/>, truncating on overflow.",
    "ConvertToInteger": "Element-wise conversion to <typeparamref name=\"TInt\"/> with rounding.",
    "ConvertToIntegerNative": "Element-wise conversion to <typeparamref name=\"TInt\"/> using native rounding.",
    "FusedMultiplyAdd": "Element-wise fused multiply-add: <c>(x * y) + addend</c>.",
    "MultiplyAdd": "Element-wise <c>(x * y) + addend</c>.",
    "MultiplyAddEstimate": "Element-wise approximate <c>(x * y) + addend</c>.",
    "Lerp": "Element-wise linear interpolation.",
    "MaxMagnitude": "Element-wise maximum by magnitude.",
    "MaxMagnitudeNumber": "Element-wise maximum by magnitude (NaN-aware).",
    "MinMagnitude": "Element-wise minimum by magnitude.",
    "MinMagnitudeNumber": "Element-wise minimum by magnitude (NaN-aware).",
    "MaxNumber": "Element-wise maximum (NaN-aware).",
    "MinNumber": "Element-wise minimum (NaN-aware).",
    "Quantile": "Returns the requested quantile.",
    "Quantiles": "Returns the requested quantiles.",
    "Wrap": "Wraps a primitive into the receiving type.",
    "WrapBinary": "Internal helper used by binary primitives.",
    "WrapUnary": "Internal helper used by unary primitives.",
    "WrapTypeChange": "Internal helper used by type-changing primitives.",
    "ConvertCheckedInPlace": "In-place conversion equivalent of <see cref=\"ConvertChecked\"/>.",
    "Activations": "Element-wise activation function.",
    "Sigmoid": "Element-wise logistic sigmoid: <c>1 / (1 + e^-x)</c>.",
    "SoftMax": "Computes the softmax of <paramref name=\"x\"/>.",
    "Hardmax": "Computes the hardmax of <paramref name=\"x\"/>.",

    # *All / *Any predicates (the actual naming convention used in the codebase)
    "IsFiniteAll": "Returns <see langword=\"true\"/> when every element is finite.",
    "IsFiniteAny": "Returns <see langword=\"true\"/> when any element is finite.",
    "IsInfinityAll": "Returns <see langword=\"true\"/> when every element is infinity.",
    "IsInfinityAny": "Returns <see langword=\"true\"/> when any element is infinity.",
    "IsPositiveInfinityAll": "Returns <see langword=\"true\"/> when every element is positive infinity.",
    "IsPositiveInfinityAny": "Returns <see langword=\"true\"/> when any element is positive infinity.",
    "IsNegativeInfinityAll": "Returns <see langword=\"true\"/> when every element is negative infinity.",
    "IsNegativeInfinityAny": "Returns <see langword=\"true\"/> when any element is negative infinity.",
    "IsNaNAll": "Returns <see langword=\"true\"/> when every element is NaN.",
    "IsNaNAny": "Returns <see langword=\"true\"/> when any element is NaN.",
    "IsNormalAll": "Returns <see langword=\"true\"/> when every element is normal.",
    "IsNormalAny": "Returns <see langword=\"true\"/> when any element is normal.",
    "IsSubnormalAll": "Returns <see langword=\"true\"/> when every element is subnormal.",
    "IsSubnormalAny": "Returns <see langword=\"true\"/> when any element is subnormal.",
    "IsZeroAll": "Returns <see langword=\"true\"/> when every element is zero.",
    "IsZeroAny": "Returns <see langword=\"true\"/> when any element is zero.",
    "IsNegativeAll": "Returns <see langword=\"true\"/> when every element is negative.",
    "IsNegativeAny": "Returns <see langword=\"true\"/> when any element is negative.",
    "IsPositiveAll": "Returns <see langword=\"true\"/> when every element is positive.",
    "IsPositiveAny": "Returns <see langword=\"true\"/> when any element is positive.",
    "IsIntegerAll": "Returns <see langword=\"true\"/> when every element is an integer.",
    "IsIntegerAny": "Returns <see langword=\"true\"/> when any element is an integer.",
    "IsEvenIntegerAll": "Returns <see langword=\"true\"/> when every element is an even integer.",
    "IsEvenIntegerAny": "Returns <see langword=\"true\"/> when any element is an even integer.",
    "IsOddIntegerAll": "Returns <see langword=\"true\"/> when every element is an odd integer.",
    "IsOddIntegerAny": "Returns <see langword=\"true\"/> when any element is an odd integer.",
    "IsCanonicalAll": "Returns <see langword=\"true\"/> when every element is canonical.",
    "IsCanonicalAny": "Returns <see langword=\"true\"/> when any element is canonical.",
    "IsComplexNumberAll": "Returns <see langword=\"true\"/> when every element is complex.",
    "IsComplexNumberAny": "Returns <see langword=\"true\"/> when any element is complex.",
    "IsImaginaryNumberAll": "Returns <see langword=\"true\"/> when every element is imaginary.",
    "IsImaginaryNumberAny": "Returns <see langword=\"true\"/> when any element is imaginary.",
    "IsRealNumberAll": "Returns <see langword=\"true\"/> when every element is real.",
    "IsRealNumberAny": "Returns <see langword=\"true\"/> when any element is real.",
    "EqualsAll": "Returns <see langword=\"true\"/> when every element equals the corresponding element of the other sequence (or scalar).",
    "EqualsAny": "Returns <see langword=\"true\"/> when any element equals the corresponding element of the other sequence (or scalar).",

    # InPlace overloads — mirror their non-InPlace counterparts
    "AbsInPlace": "Element-wise absolute value (in place).",
    "NegateInPlace": "Element-wise unary negation (in place).",
    "SqrtInPlace": "Element-wise square root (in place).",
    "CbrtInPlace": "Element-wise cube root (in place).",
    "ExpInPlace": "Element-wise <c>e^x</c> (in place).",
    "LogInPlace": "Element-wise natural log (in place).",
    "PowInPlace": "Element-wise <c>x^y</c> (in place).",
    "RoundInPlace": "Element-wise rounding (in place).",
    "FloorInPlace": "Element-wise floor (in place).",
    "CeilingInPlace": "Element-wise ceiling (in place).",
    "TruncateInPlace": "Element-wise truncation toward zero (in place).",
    "ClampInPlace": "Element-wise clamp into <c>[low, high]</c> (in place).",
    "SinInPlace": "Element-wise sine (in place).",
    "CosInPlace": "Element-wise cosine (in place).",
    "TanInPlace": "Element-wise tangent (in place).",

    # additional integer/float helpers
    "AddMultiply": "Element-wise <c>(x + y) * z</c>.",
    "BitDecrement": "Element-wise <c>BitDecrement</c> (next-down floating-point value).",
    "BitIncrement": "Element-wise <c>BitIncrement</c> (next-up floating-point value).",
    "BitwiseAnd": "Element-wise bitwise AND.",
    "BitwiseOr": "Element-wise bitwise OR.",
    "CopySign": "Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.",
    "Decrement": "Element-wise <c>x - 1</c>.",
    "Increment": "Element-wise <c>x + 1</c>.",
    "DivRem": "Element-wise <c>(x / y, x % y)</c>.",
    "Remainder": "Element-wise <c>x % y</c>.",
    "ILogB": "Element-wise integer base-2 logarithm.",
    "ScaleB": "Element-wise <c>x * 2^n</c>.",
    "Reciprocal": "Element-wise <c>1 / x</c>.",
    "ReciprocalSqrt": "Element-wise <c>1 / sqrt(x)</c>.",
    "IndexOfMaxMagnitude": "Returns the index of the element with the largest magnitude.",
    "IndexOfMinMagnitude": "Returns the index of the element with the smallest magnitude.",

    # log helpers
    "Log2InPlace": "Element-wise base-2 log (in place).",
    "Log10InPlace": "Element-wise base-10 log (in place).",
    "LogP1InPlace": "Element-wise <c>log(1 + x)</c> (in place).",

    # IEEE-754
    "Ieee754Remainder": "Element-wise IEEE 754 remainder.",
    "IsPow2All": "Returns <see langword=\"true\"/> when every element is a power of two.",
    "IsPow2Any": "Returns <see langword=\"true\"/> when any element is a power of two.",

    # conversions to non-generic primitive types
    "ConvertToHalf": "Element-wise conversion to <see cref=\"Half\"/>.",
    "ConvertToSingle": "Element-wise conversion to <see cref=\"float\"/>.",
}


def already_documented(text: str, match_start: int) -> bool:
    """Return True if the line above match_start already has a /// doc comment."""
    # walk back to the start of the previous non-empty line
    i = match_start
    # consume current method's preceding whitespace/newline
    while i > 0 and text[i - 1] in " \t":
        i -= 1
    if i > 0 and text[i - 1] == "\n":
        i -= 1
    # now i is at the end of the prev line; scan back to its start
    line_end = i
    while i > 0 and text[i - 1] != "\n":
        i -= 1
    prev_line = text[i:line_end].lstrip()
    if prev_line.startswith("///"):
        return True
    # also check for attribute lines — walk further back
    if prev_line.startswith("[") or prev_line.startswith("#"):
        # recurse
        return already_documented(text, i)
    return False


def insert_summary(text: str) -> str:
    """Insert a one-line <summary> doc above each public method that lacks one."""
    out = []
    last = 0
    for m in PUBLIC_METHOD_RE.finditer(text):
        start = m.start()
        if already_documented(text, start):
            continue
        name = m.group("name")
        summary = SUMMARY_TEMPLATES.get(name)
        if summary is None:
            continue
        indent = m.group("indent")
        out.append(text[last:start])
        out.append(f"{indent}/// <summary>{summary}</summary>\n")
        last = start
    out.append(text[last:])
    return "".join(out)


def main(paths: list[str]) -> None:
    for path in paths:
        p = Path(path)
        original = p.read_text(encoding="utf-8")
        updated = insert_summary(original)
        if updated != original:
            p.write_text(updated, encoding="utf-8")
            print(f"updated: {path}")


if __name__ == "__main__":
    main(sys.argv[1:])
