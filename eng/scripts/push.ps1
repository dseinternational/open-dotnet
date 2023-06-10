## Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
## Down Syndrome Education International and Contributors licence this file to you under the MIT license.

param (
    [string]$api_key
)

Set-StrictMode -version 2.0
$ErrorActionPreference = "Stop"

function Push() {

    . (Join-Path $PSScriptRoot "utils.ps1")

    Write-Host
    Write-Host "Pushing packages..."
    Write-Host

    if (!$api_key) {
        throw "API key is required."
    }

    $packages = Get-ChildItem ./**/*.nupkg -Recurse;

    foreach ($pkg in $packages) {

        Write-Host "Publishing $($pkg)"

        [int] $attempts = 0;
        [int] $max_attempts = 3;
        
        for ($i = 0; $i -lt $max_attempts; $i++) {
            
            try {
                Write-Host "Attempt $($i)"
                
                & dotnet nuget push $pkg.FullName --source "dseinternational" --api-key $($api_key) --skip-duplicate

                if ($LASTEXITCODE -ne 0) {
                    Write-Host    
                    Write-Host -ForegroundColor Red "Error executing 'dotnet nuget push'";
                    Write-Host
                    throw "Error executing 'dotnet nuget push'";
                }

                break;
            }
            catch {
                if ($attempts -eq ($max_attempts - 1)) {
                    throw $_.Exception;
                }
                $attempts++;
                Write-Host "Error: waiting 5 seconds." -ForegroundColor Red
                Start-Sleep -Seconds 5;
            }
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
