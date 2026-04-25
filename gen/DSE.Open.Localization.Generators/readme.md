# DSE.Open.Localization.Generators

A Roslyn incremental source generator that turns a `.restext` resource file plus a `[ResourceProvider(typeof(...))]`-annotated partial class into a strongly-typed accessor over the underlying `System.Resources.ResourceManager`.

The generator targets `netstandard2.0` (`IsRoslynComponent`-style packaging) and is shipped under `analyzers/dotnet/cs` so consumers of `DSE.Open.Localization.Resources` get it automatically. Marker attributes and the `PackagedLocalizedResourceProvider` base class live in `src/DSE.Open.Localization.Resources/`.

## What it generates

For every partial class marked `[ResourceProvider(typeof(TStrings))]` the generator emits a partial declaration that:

- Inherits `global::DSE.Open.Localization.Resources.PackagedLocalizedResourceProvider`.
- Exposes a `static readonly Default` instance.
- Wires up a `static readonly ResourceManager s_stringsResourceManager` bound to `typeof(TStrings)` and overrides `protected override ResourceManager ResourceManager`.
- Adds `internal void SetLookupCulture(CultureInfo?)` for tests to swap cultures.
- For every key in the matching `.restext` file:
  - **No holes** — emits `public string <Key>(CultureInfo? cultureInfo = null)` returning `GetString("<Key>", cultureInfo)`.
  - **With holes** — emits a method whose parameters are derived from each hole, looks up the format string via `GetString`, and builds the result using a C# interpolated string. The lookup culture is forwarded to the base class; the trailing `cultureInfo` parameter is appended to whatever holes the resource declares.

Output file name is `<ProviderName>.g.cs`.

## How to use it

1. Add a partial designer class to mark a logical resource group:

   ```csharp
   namespace MyApp.Resources;

   [EditorBrowsable(EditorBrowsableState.Never)]
   public sealed class Strings;
   ```

2. Place a `Strings.restext` (and optional culture-suffixed siblings such as `Strings.en-US.restext`) next to the designer class. Each line is a `key = value` pair:

   ```restext
   Hello = Hello!
   Greeting = Hello, {name}
   Counter = {count:int} items remaining
   ```

3. Reference both the resource file and its localised siblings in the `.csproj`:

   ```xml
   <ItemGroup>
     <EmbeddedResource Include="Resources\*.restext" />
     <AdditionalFiles Include="Resources\Strings.restext" />
   </ItemGroup>
   ```

   `EmbeddedResource` is what `ResourceManager` reads at runtime; `AdditionalFiles` is what the generator reads at compile time. **Both entries are required.**

4. Declare a partial provider class and attach `[ResourceProvider(typeof(Strings))]`:

   ```csharp
   [ResourceProvider(typeof(Strings))]
   public partial class ResourceProvider;
   ```

5. Use the generated members:

   ```csharp
   var greeting = ResourceProvider.Default.Greeting("Ada");
   var counter  = ResourceProvider.Default.Counter(7);
   ```

### Hole syntax

Hole syntax mirrors C# interpolation:

- `{name}` — defaults the parameter type to `ReadOnlySpan<char>` (see `GlobalTypes.ReadOnlySpanChar`).
- `{name:type}` — types resolve through `TypeConstraints.Lookup` (C# keywords like `int`, `bool`, `string`, plus `Guid`, `Uri`, `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, `TimeSpan`, `ReadOnlySpan<char>`). Anything not in the lookup is emitted verbatim.
- Names are normalised to camelCase, language keywords are escaped with `@`, and purely numeric names like `{0}` become `arg0`.
- Repeating a name across holes is permitted **only if the type is consistent**; conflicting types report `DSERG0009`.

### Resource file rules

- One `key = value` pair per line.
- `=` is the only delimiter; multiple `=` characters on the same line are rejected (`DSERG0002`).
- Keys cannot contain spaces or tabs (`DSERG0007`); duplicate keys are rejected (`DSERG0006`).
- Empty holes `{}` (`DSERG0003`), holes missing a name `{:int}` (`DSERG0004`) and holes ending in `:` (`DSERG0005`) all report errors.

### Diagnostics (`DSERGxxxx`)

| ID            | Severity | Meaning                                                   |
| ------------- | -------- | --------------------------------------------------------- |
| `DSERG0001`   | Warning  | Failed to read the resource file                          |
| `DSERG0002`   | Warning  | Multiple `=` delimiters on a line                         |
| `DSERG0003`   | Error    | Empty hole `{}`                                           |
| `DSERG0004`   | Error    | Hole missing an identifier (e.g. `{:int}`)                |
| `DSERG0005`   | Error    | Hole missing a type after `:` (e.g. `{name:}`)            |
| `DSERG0006`   | Error    | Duplicate key                                             |
| `DSERG0007`   | Error    | Invalid key (contains whitespace)                         |
| `DSERG0008`   | Warning  | No matching `.restext` `AdditionalFile` was found         |
| `DSERG0009`   | Error    | A hole name reuses a different type elsewhere in the line |

## Project layout

```
DisplayFormats.cs                                        # SymbolDisplayFormat presets used during emission
Resources/
  GlobalTypes.cs                                         # Fully-qualified runtime type names used in emission
  TypeConstraints.cs                                     # Hole-type lookup and C# keyword set
  Diagnostics.cs                                         # DSERGxxxx diagnostic descriptors and factories
  ResourceProviderInformation.cs                         # Per-provider model populated during transform
  ResourceProviderSourceGenerator.cs                     # IIncrementalGenerator entry point
  ResourceProviderEmitter.cs                             # Emits the partial provider class and per-key methods
  InterpolatedStringBuilder.cs                           # Shared helper for assembling interpolated output
  Parsing/
    ResourceValueParser.cs                               # .restext line/value tokeniser, hole detection
    ResourceItem.cs                                      # Parsed line model (key + holes + format length)
    Hole.cs                                              # Single hole (index, length, name, optional type)
    ParameterDefinition.cs                               # Parameter projection from a hole
```

## Pipeline

1. `SyntaxProvider.ForAttributeWithMetadataName("DSE.Open.Localization.Resources.ResourceProviderAttribute", ...)` — picks up every annotated class.
2. `SyntaxProviderTransform` — for each attribute application, captures the provider's name, namespace and accessibility, plus the `Type` argument's name, fully qualified name and source-tree path. Multiple `[ResourceProvider(...)]` attributes on the same class produce one model each.
3. `AdditionalTextsProvider.Where(... .restext)` — gathers `.restext` files declared as `<AdditionalFiles>`.
4. `Execute` — for each model, locates the matching `.restext` (same directory as the designer class, same file name without extension), parses every line via `ResourceValueParser.TryParseLine`, deduplicates by key, and feeds the `ResourceItem` set to `ResourceProviderEmitter.Emit`.
5. The emitter walks each item: keys without holes turn into a one-line accessor; keys with holes assemble an interpolated string using `format[start..end]` slicing into the live resource value (so localised translations can reorder placeholders without recompiling consumers).

## Building and debugging

The project builds as part of the solution:

```pwsh
dotnet build ./DSE.Open.slnx
```

To attach a debugger when the generator runs, uncomment the `_ = System.Diagnostics.Debugger.Launch();` line at the top of `ResourceProviderSourceGenerator.Initialize`.

Tests live in two projects:

- `test/DSE.Open.Localization.Generators.Tests/` — unit tests for the parser (`ResourceValueParserTests`) and emitter (`ResourceProviderEmitterTests`), with Verify snapshots.
- `test/DSE.Open.Localization.Generators.Tests.Functional/` — end-to-end run that consumes the generator through a project reference and exercises the generated code.

Run them with:

```pwsh
dotnet run --project test/DSE.Open.Localization.Generators.Tests/DSE.Open.Localization.Generators.Tests.csproj --configuration Debug
dotnet run --project test/DSE.Open.Localization.Generators.Tests.Functional/DSE.Open.Localization.Generators.Tests.Functional.csproj --configuration Debug
```
