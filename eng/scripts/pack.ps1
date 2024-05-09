## Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
## Down Syndrome Education International and Contributors licence this file to you under the MIT license.

Set-StrictMode -version 2.0
$ErrorActionPreference = "Stop"

function Push() {

  . (Join-Path $PSScriptRoot "utils.ps1")

  Write-Host
  Write-Host "Pack"
  Write-Host

  $libraries = Get-ChildItem ./src/**/*.csproj -Recurse;

  foreach ($lib in $libraries) {

    Write-Host "Packing $($lib)"

    & dotnet pack $lib.FullName --configuration Release

    if ($LASTEXITCODE -ne 0) {
      Write-Host
      Write-Host -ForegroundColor Red "Error executing 'dotnet pack $lib.FullName'";
      Write-Host
      throw "Error executing 'dotnet pack $lib.FullName'";
    }
  }

  $generators = Get-ChildItem ./gen/**/*.csproj -Recurse;

  foreach ($gen in $generators) {

    Write-Host "Packing $($lib)"

    & dotnet pack $gen.FullName --configuration Release --no-build

    if ($LASTEXITCODE -ne 0) {
      Write-Host
      Write-Host -ForegroundColor Red "Error executing 'dotnet pack $gen.FullName'";
      Write-Host
      throw "Error executing 'dotnet pack $gen.FullName'";
    }
  }
}

try {
  Push
}
catch {
  Write-Host $_
  Write-Host $_.Exception
  exit 1
}
