## Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
## Information contained herein is Proprietary and Confidential.

[CmdletBinding(PositionalBinding = $false)]
param (
    [bool]$publicRelease = $false
)

Set-StrictMode -version 2.0
$ErrorActionPreference = "Stop"

function Update-Dependencies() {

    . (Join-Path $PSScriptRoot "utils.ps1")

    Write-Host
    Write-Host "--------------------------------------------------------------------------------" -ForegroundColor Cyan
    Write-Host "Update-Dependencies" -ForegroundColor Cyan
    Write-Host "--------------------------------------------------------------------------------" -ForegroundColor Cyan
    Write-Host

    &dotnet-outdated -pre auto --upgrade --no-restore ./DSE.Open.slnx
}

try {
    Update-Dependencies
}
catch {
    Write-Host $_
    Write-Host $_.Exception
    exit 1
}
