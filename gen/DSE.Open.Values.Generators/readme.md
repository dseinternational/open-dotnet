# DSE.Open.Values.Generators

A Roslyn incremental source generator that emits the boilerplate required to turn a small partial `struct` declaration into a fully-featured value type wrapping a single underlying value.

The generator targets `netstandard2.0` (`IsRoslynComponent=true`) and is referenced by `src/DSE.Open.Values/DSE.Open.Values.csproj` as an `Analyzer` (`ReferenceOutputAssembly="false"`). It is also packaged under `analyzers/dotnet/cs` so consumers of the NuGet package get the generator automatically.

## What it generates

For each annotated struct, the generator emits a partial declaration containing some or all of the following, depending on the marker attribute and the interfaces present:

- A private `readonly` backing field (`_value`) and a constructor that calls `EnsureIsValidValue`.
- `[TypeConverter(typeof(ValueTypeConverter<TSelf, T>))]`.
- `IEquatable<TSelf>` members: `Equals(TSelf)`, `Equals(object?)`, `GetHashCode()`.
- Equality operators `==` / `!=`.
- `IComparable<TSelf>`, `IComparable` members and `<`, `<=`, `>`, `>=` operators (Comparable kind and above).
- Arithmetic operators `+`, `-`, `++`, `--`, unary `+`, unary `-` (Addable kind and above; unary negation is skipped for unsigned underlying types).
- `*`, `/`, `%` operators (Divisible kind).
- `ISpanFormattable` / `IFormattable` members: `ToString()`, `ToString(string?, IFormatProvider?)`, `TryFormat(...)`.
- `IParsable<TSelf>` and `ISpanParsable<TSelf>` members: `Parse` / `TryParse` (string and `ReadOnlySpan<char>` overloads).
- Optional `IUtf8SpanFormattable`, `IUtf8SpanParsable<TSelf>` and `IUtf8SpanSerializable<TSelf>` members when the underlying type implements the matching UTF-8 interface and the value type itself declares `IUtf8SpanSerializable<TSelf>`.
- `MaxSerializedCharLength` static property when supplied via the attribute.
- Conversion helpers: explicit conversion from `T`, explicit conversion to `T`, and (by default) implicit conversion to `T`.

The generator never emits a member the user has already defined on the partial struct; it inspects the syntax for `Equals`, `CompareTo`, `GetHashCode`, `TryFormat`, `ToString`, `Parse` and `TryParse` and skips those it finds.

## How to use it

The marker attributes and supporting interfaces live in `DSE.Open.Values.Abstractions`. Add `[EquatableValue]`, `[ComparableValue]`, `[AddableValue]` or `[DivisibleValue]` to a `partial readonly struct` and declare the matching `IEquatableValue<TSelf, T>` (or sub-interface).

```csharp
using DSE.Open.Values;

[ComparableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<AlphaCode, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct AlphaCode
    : IComparableValue<AlphaCode, AsciiString>,
      IUtf8SpanSerializable<AlphaCode>
{
    public const int MaxLength = 32;

    public static int MaxSerializedCharLength => MaxLength;

    public static bool IsValidValue(AsciiString value) => /* ... */;
}
```

The kinds form an inheritance chain — each adds members on top of the previous one:

| Attribute               | Required interface             | Adds                                   |
| ----------------------- | ------------------------------ | -------------------------------------- |
| `[EquatableValue]`      | `IEquatableValue<TSelf, T>`    | Equality, formatting, parsing          |
| `[ComparableValue]`     | `IComparableValue<TSelf, T>`   | + `CompareTo`, comparison operators    |
| `[AddableValue]`        | `IAddableValue<TSelf, T>`      | + `+`, `-`, `++`, `--`, unary `+`, `-` |
| `[DivisibleValue]`      | `IDivisibleValue<TSelf, T>`    | + `*`, `/`, `%`                        |

### Attribute properties

Both are defined on the `ValueAttribute` base class:

- `MaxSerializedCharLength` — emits `public static int MaxSerializedCharLength => <n>;` when set to a positive value.
- `ImplicitDowncast` — when `false`, suppresses the implicit conversion to `T`. Defaults to `true`.

### Customisation hooks

- Define a static `GetString(string)` or `GetString(ReadOnlySpan<char>)` method on the struct to route formatted strings through interning or pooling (see `CodeStringPool`).
- Define `Equals(TSelf)`, `CompareTo(TSelf)`, `GetHashCode()`, `TryFormat`, `ToString`, `Parse` or `TryParse` on the struct to opt out of generating that member. The generator matches by signature, not just name.
- Implementing `IUtf8SpanFormattable` / `IUtf8SpanParsable<TSelf>` on the underlying type enables UTF-8 method emission; you must also declare `IUtf8SpanSerializable<TSelf>` on the value type to opt in.

## Project layout

```
ValueTypesGenerator.cs           # IIncrementalGenerator entry point and spec construction
ValueTypesGenerator.Emitter.cs   # Emits source from a ValueTypeSpec
SourceWriter.cs                  # Indented writer used by the emitter
Model/                           # ValueTypeSpec hierarchy: Equatable -> Comparable -> Addable -> Divisible
Extensions/                      # Syntax helpers used to recognise interface method shapes
TypeNames.cs / TargetNames.cs    # Attribute, interface and member name constants
Namespaces.cs / GlobalTypes.cs   # Fully-qualified names used in emitted source
ParentClass.cs                   # Captures nesting for partial declarations inside types
AccessibilityHelper.cs           # Maps Roslyn Accessibility values to keywords
```

## Pipeline

1. `IsSyntaxStructWithAttributes` — cheap syntactic filter for any `struct` with at least one attribute list.
2. `GetSemanticTargetForGeneration` — keeps structs whose attributes resolve to one of the four marker types.
3. `GetValueTypeSpecs` — for each surviving struct, resolves the wrapped type from the value-type interface, inspects existing members to decide what to skip, reads attribute arguments, and builds a `ValueTypeSpec` of the appropriate subtype.
4. `Emitter.GenerateValueStruct` — walks the spec and writes one `<TypeName>.g.cs` file per value type via `RegisterSourceOutput`.

## Building and debugging

The project builds as part of the solution:

```pwsh
dotnet build ./DSE.Open.slnx
```

In `Debug`, the `__LAUNCH_DEBUGGER` constant is defined; uncomment the `#if LAUNCH_DEBUGGER` block in `ValueTypesGenerator.Initialize` to attach a debugger when the generator runs.

Tests live in `test/DSE.Open.Values.Generators.Tests/`. Run with:

```pwsh
dotnet run --project test/DSE.Open.Values.Generators.Tests/DSE.Open.Values.Generators.Tests.csproj --configuration Debug
```

## References

- [Microsoft.Extensions.Logging.Abstractions source generator](https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.Logging.Abstractions/gen)
- [System.Text.Json source generator](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Text.Json/gen/JsonSourceGenerator.Emitter.cs)
- [CommunityToolkit.Mvvm source generators](https://github.com/CommunityToolkit/dotnet/tree/main/src/CommunityToolkit.Mvvm.SourceGenerators)
- [Andrew Lock — Creating a source generator series](https://andrewlock.net/series/creating-a-source-generator/)
