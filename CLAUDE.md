# CLAUDE.md

> **Keep in sync:** This file, `CLAUDE.md`, and `AGENTS.md` contain the same core guidance. Update all three when making changes.

## Overview

This repository contains a variety of libraries and tools useful for building .NET applications. They provide commoon functionality for DSE projects, but may have wider applicability, including, in particular, third parties integrating with DSE projects and services.

## Build and Test Commands

**Requires .NET SDK 10.0.200+** (pinned in `global.json`).

```pwsh
# Restore
dotnet restore ./DSE.Open.slnx

# Build everything
dotnet build ./DSE.Open.slnx

# Run a single test project
dotnet run --project test/<ProjectName>/<ProjectName>.csproj --configuration Debug

# Run all tests (uses eng/scripts/test.ps1 internally)
./eng/scripts/test.ps1
```

> **Important:** Tests use **Microsoft Testing Platform**, not the classic `dotnet test` runner. Always use `dotnet run --project` to run tests, not `dotnet test`.

## Repository Structure

```
src/          # Packable library projects (all AOT-compatible)
gen/          # Roslyn source generators
test/         # Test projects
benchmarks/   # BenchmarkDotNet projects
eng/scripts/  # PowerShell CI/build scripts
pkg/          # NuGet package assets (icon, readme)
```

The solution file is `DSE.Open.slnx`.

## Package Management

- **Central Package Management** is enabled (`Directory.Packages.props`); never add `Version="..."` to `<PackageReference>` in `.csproj` files
- `DSE.*` packages are sourced exclusively from the private GitHub NuGet feed (`dseinternational`); all other packages come from `nuget.org` — this is enforced via source mapping in `NuGet.config`

## Requirements

### AOT Compatibility

- All `src/` projects must remain AOT-compatible (`IsAotCompatible=true` is set globally)
- Avoid reflection-based APIs; prefer source-generated alternatives
