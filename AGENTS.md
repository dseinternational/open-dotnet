# AGENTS.md

This file provides guidance to AI coding agents (e.g. OpenAI Codex, Cursor) when working with code in this repository.

> **Keep in sync:** This file, `CLAUDE.md`, and `.github/copilot-instructions.md` contain the same core guidance. Update all three when making changes.

## Overview

DSE Open is a collection of .NET libraries for DSE Open systems. The solution targets **.NET 10.0** with C# preview language features, nullable reference types enabled, and `TreatWarningsAsErrors: true`.

Solution file: `DSE.Open.slnx`

## Build & Test Commands

```bash
# Build the source generator first (required before building the solution)
dotnet build ./gen/DSE.Open.Values.Generators/DSE.Open.Values.Generators.csproj

# Build everything
dotnet build ./DSE.Open.slnx --configuration Release

# Run all tests (cross-platform PowerShell)
./eng/scripts/test.ps1 -target ./test -configuration Debug

# Run a single test project — use dotnet run, NOT dotnet test
dotnet run --project test/DSE.Open.Tests/DSE.Open.Tests.csproj --configuration Debug

# Run tests with coverage
./eng/scripts/test.ps1 -coverage true

# Pack NuGet packages
./eng/scripts/pack.ps1
```

> **Important:** Test projects target **Microsoft.Testing.Platform** with `OutputType=Exe`. Always run them with `dotnet run --project`, not `dotnet test`.

Tests tagged `Category="ActiveIssue"` are filtered out by default (see `.runsettings`).

## Project Layout

- `src/` — Library source projects (50+)
- `test/` — Matching test projects (one per src project)
- `gen/` — Source generators (`DSE.Open.Values.Generators`, `DSE.Open.Localization.Generators`)
- `benchmarks/` — BenchmarkDotNet performance projects
- `eng/scripts/` — PowerShell build/publish scripts
- `pkg/` — NuGet packaging assets

Most features follow an **Abstractions-first** pattern: `DSE.Open.Foo.Abstractions` contains interfaces, then `DSE.Open.Foo` has implementations.

## Architecture

### Domain Model (`DSE.Open.DomainModel`)

- `Entity<TId>` — base for domain entities with typed IDs
- `StoredObject` / `TimestampedStoredObject` — persistence base classes
- `DomainEvent<TData>` / `IDomainEvent<TData>` — typed domain events
- `DomainEventDispatcher` — dispatches events raised by entities

### Value Types (`DSE.Open.Values`)

Extensive library of strongly-typed value objects (e.g., `Identifier`, `AsciiCode`, `Label`, `Tag`). New value types should use the **source generator** (`DSE.Open.Values.Generators`) — annotate structs with the appropriate attribute and the generator produces parsing, formatting, JSON, and comparison code.

### Results Pattern

Explicit error handling via result types in `DSE.Open` — avoid throwing for expected failure cases.

### Ternary Logic

`Trilean` and `Ternary` types provide three-valued logic (true/false/unknown).

### EF Core Integration

Multiple EF projects for different databases: `DSE.Open.EntityFrameworkCore`, `DSE.Open.EntityFrameworkCore.SqlServer`, `DSE.Open.EntityFrameworkCore.Sqlite`, `DSE.Open.EntityFrameworkCore.Relational`.

### NLP / Python Interop

`DSE.Open.Interop.Python` integrates Stanza NLP via **CSnakes**. Requires **Python 3.14** and the packages in `requirements.txt`. CI caches Stanza resources; local setup may require running `./eng/scripts/init_python.ps1`.

## Code Standards

- **Line length**: 120 characters (enforced via `.editorconfig`)
- **Indentation**: 4 spaces for C#, 2 spaces for XML/props files
- **Nullable**: fully enabled — all new code must handle nullability
- **Analysis**: `AllEnabledByDefault` at `preview` level — warnings are errors
- **Unsafe**: allowed but use sparingly

### Centralized Package Management

All NuGet package versions are declared in `Directory.Package.props`. Do not specify versions in individual project files — add new packages there only.

## Testing Patterns

- **AwesomeAssertions** for fluent assertions
- **Verify.XunitV3** for approval/snapshot testing
- **Moq** for mocking
- Value type tests extend `ValueTestsBase<T>`
- Long-running test timeout: 60 seconds (`.runsettings`)

## Versioning

Uses **Nerdbank.GitVersioning** with a calver scheme (`2026.x`). Do not manually set version numbers — version is derived from `version.json` and git history.
