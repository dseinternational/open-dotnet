## Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
## Information contained herein is Proprietary and Confidential.

[CmdletBinding(PositionalBinding = $false)]
param (
  [string]$target,
  [string]$configuration = "Debug"
)

Set-StrictMode -version 2.0
$ErrorActionPreference = "Stop"

try {
  . (Join-Path $PSScriptRoot "utils.ps1")

  if (-not (Test-Path $target)) {
    throw "Path does not exist: $target"
  }

  $exit_code = 0;

  $item = Get-Item $target

  if ($item -is [System.IO.DirectoryInfo]) {
    $tests = Get-ChildItem -Path $item -Recurse -Filter *Tests.csproj
    foreach ($test in $tests) {
      Write-Host
      Write-Host "------------------------------------------------------------------------------------------------------------------------"
      Write-Host "Running tests in $test"
      Write-Host "------------------------------------------------------------------------------------------------------------------------"
      Write-Host "dotnet run `"$test`" --configuration $configuration"
      Write-Host
      &dotnet run --project "$($test.FullName)" `
        --configuration $configuration

      if ($LASTEXITCODE -ne 0) {
        $exit_code = 1;
      }
    }
  }
  elseif ($item -is [System.IO.FileInfo]) {
    Write-Host
    Write-Host "------------------------------------------------------------------------------------------------------------------------"
    Write-Host "Running tests in $item"
    Write-Host "------------------------------------------------------------------------------------------------------------------------"
    Write-Host "dotnet run `"$item`" --configuration $configuration"
    Write-Host
    &dotnet run --project "$($item.FullName)" `
      --configuration $configuration

    if ($LASTEXITCODE -ne 0) {
      $exit_code = 1;
    }
  }
  else {
    throw "Invalid path: $target"
  }


  exit $exit_code
}
catch {
  Write-Host $_
  Write-Host $_.Exception
  exit 1
}
