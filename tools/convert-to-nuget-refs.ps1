<#
.SYNOPSIS
    Convert ProjectReference (to TDA Src projects) into NuGet PackageReference.

.DESCRIPTION
    Inside the private TDA repo, Examples/*.csproj reference Src projects via
    ProjectReference. In the public tda-examples repo they must reference the
    same-named packages published on nuget.org instead.
    This script rewrites Examples/**/*.csproj in place. It is idempotent:
    already-converted projects are skipped.

    Intended usage: run it right after a sync copy (robocopy) from the TDA
    repo, with -Version set to the TDA NuGet version that was just published.

    Example:
        powershell -ExecutionPolicy Bypass -File tools/convert-to-nuget-refs.ps1 -Version 2.1.1
#>
param(
    [string]$Version = '2.1.1'
)

$repoRoot = Split-Path -Parent $PSScriptRoot          # ...\tda-examples
$examplesDir = Join-Path $repoRoot 'Examples'
if (-not (Test-Path $examplesDir)) { throw "Examples directory not found: $examplesDir" }

# Relative path of Src project (as written in Examples csproj) -> NuGet package id (same as project name)
$map = [ordered]@{
    '..\..\Src\TDA\TDA.csproj'         = 'TDA'
    '..\..\Src\TDA.GUI\TDA.GUI.csproj' = 'TDA.GUI'
    '..\..\Src\TDA.Report\TDA.Report.csproj' = 'TDA.Report'
}

$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
$changed = @()

Get-ChildItem $examplesDir -Recurse -Filter *.csproj | ForEach-Object {
    $path = $_.FullName
    $content = [System.IO.File]::ReadAllText($path)
    $orig = $content

    foreach ($key in $map.Keys) {
        $pkg = $map[$key]
        # Skip if already converted to the target package
        if ($content -match "<PackageReference Include=`"$([regex]::Escape($pkg))`"") { continue }

        $pattern = [regex]::Escape("<ProjectReference Include=`"$key`" />")
        if ($content -match $pattern) {
            $content = $content -replace $pattern, "<PackageReference Include=`"$pkg`" Version=`"$Version`" />"
        }
    }

    if ($content -ne $orig) {
        [System.IO.File]::WriteAllText($path, $content, $utf8NoBom)
        $changed += $_.Name
    }
}

if ($changed.Count -eq 0) {
    Write-Output 'No csproj needed conversion (all already use PackageReference).'
} else {
    Write-Output ("Converted {0} project(s): {1}" -f $changed.Count, ($changed -join ', '))
}
