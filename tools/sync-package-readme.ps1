<#
    sync-package-readme.ps1 -- mirror NuGet package READMEs from TDA into packages/

    Source of truth : TDA private repo   Src/<pkg>/README.md + README.zh-CN.md
    Mirror here     : tda-learn          packages/<pkg>/

    Why two copies exist:
      The same README.md is consumed twice -- packed into the .nupkg as the NuGet
      package landing page (<PackageReadmeFile> in Directory.Build.props), and read
      from packages/ through the English/Chinese switch links. Since packing requires
      the file to sit next to the csproj, the README cannot move out of Src/; the
      public repo therefore holds a synced copy.

    Scope: only the three published NuGet packages -- TDA / TDA.GUI / TDA.Report.
      TDA.CLI (dp)     : user manual lives in CLI/ and is maintained here -> opposite direction.
      TDA.Drawing      : internal design stage, not published -> never mirrored.

    Discipline: read-only on TDA, writes only packages/, never commits or pushes.

    NOTE: keep this file ASCII-only. Windows PowerShell 5.1 decodes a BOM-less .ps1
    as ANSI, which mangles non-ASCII bytes and can break parsing.

    Usage:
      powershell -ExecutionPolicy Bypass -File tools/sync-package-readme.ps1
      powershell -ExecutionPolicy Bypass -File tools/sync-package-readme.ps1 -WhatIfOnly
      powershell -ExecutionPolicy Bypass -File tools/sync-package-readme.ps1 `
        -TdaRepo C:\Users\eddy.000\Music\TDAWksps\TDA `
        -PubRepo C:\Users\eddy.000\Music\TDAWksps\tda-learn
#>
[CmdletBinding()]
param(
    [string]$TdaRepo = 'C:\Users\eddy.000\Music\TDAWksps\TDA',
    [string]$PubRepo = 'C:\Users\eddy.000\Music\TDAWksps\tda-learn',
    [switch]$WhatIfOnly
)

$ErrorActionPreference = 'Stop'
$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
$Packages = @('TDA', 'TDA.GUI', 'TDA.Report')
$Files    = @('README.md', 'README.zh-CN.md')

Write-Host '=== packages/ README sync (TDA -> tda-learn) ===' -ForegroundColor Cyan
Write-Host "TDA repo : $TdaRepo"
Write-Host "Public   : $PubRepo"
if ($WhatIfOnly) { Write-Host '(WhatIfOnly: report differences only, nothing written)' -ForegroundColor Yellow }

$copied = 0; $skipped = 0; $missing = 0

foreach ($pkg in $Packages) {
    $from = Join-Path $TdaRepo ('Src\' + $pkg)
    $to   = Join-Path $PubRepo ('packages\' + $pkg)

    if (-not (Test-Path $from)) {
        Write-Host ('[MISS] source folder not found: ' + $from) -ForegroundColor Red
        $missing++
        continue
    }

    foreach ($f in $Files) {
        $src = Join-Path $from $f
        if (-not (Test-Path $src)) {
            Write-Host ('[MISS] ' + $pkg + '\' + $f + ' not found (mirror left untouched)') -ForegroundColor Red
            $missing++
            continue
        }

        $dst = Join-Path $to $f
        $label = $pkg + '\' + $f

        # compare by content: identical files are skipped to avoid pointless mtime churn
        if (Test-Path $dst) {
            $h1 = (Get-FileHash $src -Algorithm MD5).Hash
            $h2 = (Get-FileHash $dst -Algorithm MD5).Hash
            if ($h1 -eq $h2) { Write-Host ('[SKIP] ' + $label + ' unchanged'); $skipped++; continue }
        }

        if ($WhatIfOnly) {
            Write-Host ('[DIFF] ' + $label + ' needs update') -ForegroundColor Yellow
            $copied++
            continue
        }

        New-Item -ItemType Directory -Force -Path $to | Out-Null
        # write UTF-8 without BOM: a BOM shows up as a stray character on NuGet/GitHub
        $text = [System.IO.File]::ReadAllText($src, [System.Text.Encoding]::UTF8)
        [System.IO.File]::WriteAllText($dst, $text, $utf8NoBom)
        Write-Host ('[COPY] ' + $label) -ForegroundColor Green
        $copied++
    }
}

Write-Host '--- summary ---'
Write-Host ('updates: ' + $copied + '   unchanged: ' + $skipped + '   missing: ' + $missing)

if (-not $WhatIfOnly) {
    Write-Host ''
    Write-Host 'Next (manual): review -> commit packages/ separately -> push both remotes.' -ForegroundColor Cyan
    Write-Host 'Do not mix this commit with Examples/ mirror changes or CLI/ edits.'
    Write-Host 'Sanity check: .gitignore once swallowed this folder (**/packages/*) and *.ps1 -'
    Write-Host 'verify with "git status" that packages/ and tools/*.ps1 actually show up.' -ForegroundColor Yellow
}
