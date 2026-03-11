# Copilot Instructions

## Build & Test Commands

**Requires .NET SDK 10.0.200+** (pinned in `global.json`).

```pwsh
# Restore
dotnet restore ./DSE.Open.slnx

# Build the source generator first (required before building the solution)
dotnet build ./gen/DSE.Open.Values.Generators/DSE.Open.Values.Generators.csproj

# Build everything
dotnet build ./DSE.Open.slnx

# Run a single test project
dotnet run --project test/<ProjectName>/<ProjectName>.csproj --configuration Debug

# Run all tests (uses eng/scripts/test.ps1 internally)
./eng/scripts/test.ps1

# Run tests with coverage
./eng/scripts/test.ps1 -coverage true
```

> **Important:** Tests use **Microsoft Testing Platform**, not the classic `dotnet test` runner. Always use `dotnet run --project` to run tests, not `dotnet test`.

## Repository Structure

```
src/          # Packable library projects (all AOT-compatible)
gen/          # Roslyn source generators (targets netstandard2.0)
test/         # Test projects (not packable, OutputType=Exe)
benchmarks/   # BenchmarkDotNet projects
eng/scripts/  # PowerShell CI/build scripts
pkg/          # NuGet package assets (icon, readme)
```

The solution file is `DSE.Open.slnx` (new-format `.slnx`).

## Architecture Overview

This is a collection of independent NuGet libraries sharing a common infrastructure. The key cross-cutting concerns are:

### Strongly-Typed Value Types (the central pattern)

Most domain types are **`readonly partial struct`** values backed by the `DSE.Open.Values.Generators` Roslyn source generator. The generator emits all equality, comparison, formatting, and parsing boilerplate. You only write the struct declaration and the validation logic.

**Choose the right attribute:**

| Attribute | Interface | Adds Over Previous |
|---|---|---|
| `[EquatableValue]` | `IEquatableValue<TSelf, T>` | Equality, hashing, format, parse |
| `[ComparableValue]` | `IComparableValue<TSelf, T>` | `CompareTo`, `<`, `>`, `<=`, `>=` |
| `[AddableValue]` | `IAddableValue<TSelf, T>` | `+`, `-`, `++`, `--` |
| `[DivisibleValue]` | `IDivisibleValue<TSelf, T>` | `*`, `/`, `%` |

**Minimal value type:**

```csharp
[ComparableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonStringValueConverter<MyValue, long>))]
public readonly partial struct MyValue : IComparableValue<MyValue, long>
{
    public static int MaxSerializedCharLength => 20;

    public static bool IsValidValue(long value) => value >= 0;
}
```

**Generator suppression:** If you define any of these members yourself, the generator skips emitting them:
- Constructor `(T value, bool skipValidation = false)`
- `bool Equals(TSelf other)`
- `override int GetHashCode()`
- `bool TryFormat(Span<char>, out int, ReadOnlySpan<char>, IFormatProvider?)`
- `static TSelf Parse(string, IFormatProvider?)`
- `static bool TryParse(string?, IFormatProvider?, out TSelf)`

**String interning hooks:** Define either of these and `ToString` will use them:
```csharp
private static string GetString(string s) => ...;
private static string GetString(ReadOnlySpan<char> s) => ...;
```

**UTF-8 support:** Implement `IUtf8SpanSerializable<TSelf>` on the struct and the generator will also emit UTF-8 format/parse methods (only if `T` supports `IUtf8SpanFormattable`/`IUtf8SpanParsable`).

**Referencing the generator from a library project:**
```xml
<ProjectReference Include="..\..\gen\DSE.Open.Values.Generators\DSE.Open.Values.Generators.csproj"
                  OutputItemType="Analyzer"
                  ReferenceOutputAssembly="false" />
```

### Project Configuration Inheritance

MSBuild props cascade in layers — never duplicate settings that are already inherited:

- **Root `Directory.Build.props`** → all projects: `LangVersion=preview`, `Nullable=enable`, `TreatWarningsAsErrors=true`, `AllowUnsafeBlocks=true`, full analyzer suite, `AnalysisMode=AllEnabledByDefault`
- **`src/Directory.Build.props`** → all library projects: `IsAotCompatible=true`, `IsPackable=true`, XML docs, `CommunityToolkit.Diagnostics` package reference added automatically, global usings for `CommunityToolkit.Diagnostics` and `System.Globalization`
- **`test/Directory.Build.props`** → all test projects: xUnit v3, AwesomeAssertions, Moq, CodeCoverage added automatically; global usings for `System.Globalization` and `Xunit`

### Versioning

- Calendar versioning: `YYYY.MINOR.BUILD` (e.g., `2026.3.0`), managed by **Nerdbank.GitVersioning**
- `version.props` is **auto-generated** — never edit it manually; run `./eng/scripts/versions.ps1` to regenerate
- Do not specify `<Version>` in any `.csproj` — it comes from `version.props`

### Package Management

- **Central Package Management** is enabled (`Directory.Packages.props`); never add `Version="..."` to `<PackageReference>` in `.csproj` files
- `DSE.*` packages are sourced exclusively from the private GitHub NuGet feed (`dseinternational`); all other packages come from `nuget.org` — this is enforced via source mapping in `NuGet.config`

## Key Conventions

### Value Types
- Always `readonly partial struct` (generator-backed) or `readonly record struct` (fully manual)
- Always `[StructLayout(LayoutKind.Sequential)]`
- Always `[JsonConverter(...)]` on every value type
- Provide a `public static readonly TSelf Empty;` zero-value sentinel
- Use `value is <= X` not `value is >= 0 and <= X` for unsigned types (`byte`, `uint`, etc.) — the `>= 0` subpattern is redundant and triggers CS9335

### Argument Validation
- Use `Guard.*` from `CommunityToolkit.Diagnostics` (available as a global using in all `src/` projects)
- Use `ThrowHelper.ThrowFormatException(...)` for parse failures in value types

### Stack Allocation
- Always guard with `MemoryThresholds.CanStackalloc<T>(count)` before `stackalloc`
- Apply `[SkipLocalsInit]` to methods that `stackalloc`

### Testing
- Test projects are executables; run with `dotnet run --project`, not `dotnet test`
- Base class for source generator tests: `ValueTypeGenerationTests` — provides `AssertDiagnosticsCount` and `WriteSyntax`
- `AssertDiagnosticsCount` counts **all** diagnostic severities including `hidden` — ensure generated and input code produce zero diagnostics
- Use **AwesomeAssertions** (FluentAssertions successor) for assertions; avoid `Assert.*` from xUnit except where AwesomeAssertions doesn't cover it
- One test project per library, colocated in `test/`

### AOT Compatibility
- All `src/` projects must remain AOT-compatible (`IsAotCompatible=true` is set globally)
- Avoid reflection-based APIs; prefer source-generated alternatives
- JSON serialization: use `System.Text.Json` with source generation or `IJsonOnDeserialized`-style hooks

### Generated Code
- The source generator emits `#nullable enable annotations` + `#nullable disable warnings` + `#pragma warning disable CA2225` at the top of every generated file
- Generated files are named `{TypeName}.g.cs`
- Do not modify `.g.cs` files — change the generator or the partial struct declaration
