Set-StrictMode -version 2.0
$ErrorActionPreference = "Stop"

function TestEnvironment-Init() {

  . (Join-Path $PSScriptRoot "utils.ps1")

  Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
  Write-Host "Initializing Python environment" -ForegroundColor Green
  Write-Host "------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Green
  Write-Host


  if ($IsWindows) {
    $py = Get-Command python -ErrorAction SilentlyContinue
  }
  else {
    $py = Get-Command python3 -ErrorAction SilentlyContinue
  }

  Write-Host "Creating virtual environment '.venv'" -ForegroundColor Yellow
  Write-Host

  &$py -m venv .venv

  Write-Host "Activating virtual environment '.venv'" -ForegroundColor Yellow
  Write-Host

  if ($IsWindows) {
    .venv/Scripts/Activate.ps1
  }
  else {
    .venv/bin/Activate.ps1
  }

  Write-Host "Installing packages" -ForegroundColor Yellow
  Write-Host

  python -m pip install --upgrade pip
  python -m pip --version

  if ($IsWindows) {
    if ($env:USE_CUDA -eq "True") {
      Write-Host "Using CUDA 12.9"
      python -m pip install --upgrade torch --index-url https://download.pytorch.org/whl/cu129
    }
    else {
      Write-Host "Using CPU only"
      python -m pip install --upgrade torch
    }
  }
  elseif ($IsLinux) {
    if ($env:USE_CUDA -eq "True") {
      Write-Host "Using CUDA 12.9"
      python -m pip install --upgrade torch --index-url https://download.pytorch.org/whl/cu129
    }
    else {
      Write-Host "Using CPU only"
      python -m pip install --upgrade torch --index-url https://download.pytorch.org/whl/cpu
    }
  }
  else {
    python -m pip install torch
  }

  python -m pip install -r ./requirements.txt

  python ./eng/scripts/init_stanza.py
}

try {
  TestEnvironment-Init
}
catch {
  Write-Host $_
  Write-Host $_.Exception
  exit 1
}
