// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Generators;

public sealed class GlobalTypes
{
    // System
    public const string Span = "global::System.Span";
    public const string ReadOnlySpan = "global::System.ReadOnlySpan";

    public const string IFormattable = "global::System.IFormattable";
    public const string IParsable = "global::System.IParsable";

    public const string IComparable = "global::System.IComparable";
    public const string IEquatable = "global::System.IEquatable";

    public const string ISpanFormattable = "global::System.ISpanFormattable";
    public const string ISpanParsable = "global::System.ISpanParsable";

    public const string IUtf8SpanFormattable = "global::System.IUtf8SpanFormattable";
    public const string IUtf8SpanParsable = "global::System.IUtf8SpanParsable";

    public const string IFormatProvider = "global::System.IFormatProvider";

    public const string ArgumentOutOfRangeException = "global::System.ArgumentOutOfRangeException";
    public const string NotImplementedException = "global::System.NotImplementedException";

    // System.Memory
    public const string MemoryExtensions = "global::System.MemoryExtensions";

    // System.Buffers
    public const string ArrayPool = "global::System.Buffers.ArrayPool";

    // System.Globalization
    public const string NumberStyles = "global::System.Globalization.NumberStyles";

    public const string CultureInfo = "global::System.Globalization.CultureInfo";

    public const string CultureInfoInvariantCulture = $"{CultureInfo}.InvariantCulture";

    // System.ComponentModel
    public const string TypeConverter = "global::System.ComponentModel.TypeConverter";

    // System.Runtime.CompilerServices
    public const string SkipLocalsInit = "global::System.Runtime.CompilerServices.SkipLocalsInit";

    // DSE.Open.Runtime.Helpers
    public const string MemoryThresholds = "global::DSE.Open.Runtime.Helpers.MemoryThresholds";
}
