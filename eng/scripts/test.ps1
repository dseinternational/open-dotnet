## Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
## Information contained herein is Proprietary and Confidential.

[CmdletBinding(PositionalBinding = $false)]
param (
  [string]$target,
  [string]$configuration = "Debug",
  [bool]$coverage = $false,
  [string]$coverage_output,
  [string]$coverage_output_format = "cobertura"
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

  $failed_count = 0
  $failed_executions = @()

  if ($item -is [System.IO.DirectoryInfo]) {
    $tests = Get-ChildItem -Path $item -Recurse -Filter *Tests.csproj
    foreach ($test in $tests) {
      Write-Host
      Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
      Write-Host "Running tests in $test" -ForegroundColor Green
      Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
      Write-Host
      $dotnet_args = @("run", "--project", "$($test.FullName)", "--configuration", $configuration);
      if ($coverage) {
        $dotnet_args += "--coverage";
        if (-not [string]::IsNullOrWhiteSpace($coverage_output)) {
          $dotnet_args += "--coverage-output";
          $dotnet_args += $coverage_output;
        }
        if (-not [string]::IsNullOrWhiteSpace($coverage_output_format)) {
          $dotnet_args += "--coverage-output-format";
          $dotnet_args += $coverage_output_format;
        }
      }
      &dotnet $dotnet_args

      if ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne 8) {
        Write-Host
        Write-Host "********************************************************************************" -ForegroundColor Red
        Write-Host "Test execution FAILED with exit code $LASTEXITCODE" -ForegroundColor Red
        Write-Host "********************************************************************************" -ForegroundColor Red
        Write-Host
        $exit_code = $LASTEXITCODE;
        $failed_count += 1;
        $failed_executions += $test.FullName;
      }
    }
  }
  elseif ($item -is [System.IO.FileInfo]) {
    Write-Host
    Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
    Write-Host "Running tests in $item" -ForegroundColor Green
    Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
    Write-Host
    $dotnet_args = @("run", "--project", "$($test.FullName)", "--configuration", $configuration);
    if ($coverage) {
      $dotnet_args += "--coverage";
      if (-not [string]::IsNullOrWhiteSpace($coverage_output)) {
        $dotnet_args += "--coverage-output";
        $dotnet_args += $coverage_output;
      }
      if (-not [string]::IsNullOrWhiteSpace($coverage_output_format)) {
        $dotnet_args += "--coverage-output-format";
        $dotnet_args += $coverage_output_format;
      }
    }

    if ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne 8) {
      Write-Host
      Write-Host "********************************************************************************" -ForegroundColor Red
      Write-Host "Test execution FAILED with exit code $LASTEXITCODE" -ForegroundColor Red
      Write-Host "********************************************************************************" -ForegroundColor Red
      Write-Host
      $exit_code = $LASTEXITCODE;
      $failed_count += 1;
      $failed_executions += $test.FullName;
    }
  }
  else {
    throw "Invalid path: $target"
  }


  if ($failed_count -ne 0) {
    Write-Host
    Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Red
    Write-Host "A total of $failed_count test executions failed" -ForegroundColor Red
    Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Red
    Write-Host
    foreach ($fe in $failed_executions) {
      Write-Host " - $fe" -ForegroundColor Red
    }
  }
  else {
    Write-Host
    Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
    Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
    Write-Host "All test executions passed" -ForegroundColor Green
    Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
    Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
    Write-Host
  }

  exit $exit_code
}
catch {
  Write-Host $_
  Write-Host $_.Exception
  exit 1
}
