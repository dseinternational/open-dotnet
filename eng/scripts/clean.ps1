## Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
## Down Syndrome Education International and Contributors licence this file to you under the MIT license.

Set-StrictMode -version 2.0
$ErrorActionPreference="Stop"

function Clean()
{
    . (Join-Path $PSScriptRoot "utils.ps1")
    Get-ChildItem $repo_dir -include bin,obj,AppPackages,BundleArtifacts,node_modules,TestResults,.tools,_temp -Recurse | ForEach-Object { Remove-Item $_.FullName -Force -Recurse }
}

try
{
    Clean
}
catch
{
    Write-Host $_
    Write-Host $_.Exception
    exit 1
}
