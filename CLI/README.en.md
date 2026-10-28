# dp — TDA Command-Line Tool (TDA.CLI)

> [中文](../README.md) | English
> **Doc version:** v1.0 · 2026-09 · applies to dp 2.x (TDA Framework 2.1.x)
> **Maintenance:** this document is maintained directly in the tda-learn repository (`CLI/`), not mirrored from elsewhere. If anything here disagrees with `dp --help`, **the actual CLI wins** — and please fix this file.

`dp` is the command-line interface of TDA Framework. It wraps the core capabilities of DataPrism — STDF parsing, multi-sublot / retest correlation, statistical analysis, CSV export, and Setup config generation — into **one-line commands**, so common data-analysis tasks need no C# code at all.

It was designed for **AI agents and scripts**: an agent only has to assemble the right command line and read back the CSV / JSON output to fulfil requests like "analyze this STDF file".

## Examples or CLI?

| Channel | For whom | Entry point |
| --- | --- | --- |
| `Examples/` | Learners / integrators who want to **write C#** against TDA | [Examples/README.md](../Examples/README.md) |
| `CLI/` (this folder) | Engineers / AI agents / scripts who want to **analyze STDF files quickly without writing code** | this file |

Both are powered by the same DataPrism engine, so their capabilities overlap. To understand what a command does internally, read the matching example project under `Examples/`.

## Features

- **Zero code**: `dp export lsum test.stdf` prints a yield summary.
- **Standardized output**: results are written as CSV files by default, easy for agents and scripts to parse.
- **Wildcard batches**: `*`, `**` (recursive) and `?` handle a whole directory of lot files.
- **Retest / multi-sublot correlation**: `dp create setup` generates a reusable config from multiple STDF files.
- **Background caching**: data commands run through a resident daemon by default; an STDF file is loaded once and later commands hit the cache (see [ai-guide.md](ai-guide.md)).
- **Windows / .NET**: built on .NET 10, Windows 10/11 only.

## Getting dp

dp is currently distributed through internal channels; public distribution is planned as an **MSI installer hosted in this repository's Releases page**.

- Users who obtained the extracted (zero-install) build need Windows 10/11 plus the **.NET 10 Desktop Runtime (x64)**; add the folder to `PATH` and `dp` is ready.
- Once the MSI is published, this section is updated with standard steps (Release → download → install → verify).

> To try the full command set before public release, contact the maintainer for the current internal build.

## Quick start (30 seconds)

Below assumes you already have an STDF file (always quote paths containing spaces):

```powershell
# 1. Verify the install: show dp and TDA framework versions
dp show version

# 2. Lot yield summary (writes <dir>\<file>.lsum.csv)
dp export lsum "D:\data\demo_lot.stdf"

# 3. Statistics for all test items (mean / range / Cp / Cpk / quantiles)
dp export tsum "D:\data\demo_lot.stdf" -o "D:\result\tsum.csv"

# 4. Batch: every lot file under a directory (** = recursive)
dp export lsum "D:\data\**\*.std"
```

> The first data command starts the background daemon and loads the file, so it is **noticeably slower — this is normal**; repeats on the same file hit the cache.
>
> **If a command is unexpectedly slow** (or the terminal prints `falling back to standalone`), the daemon was most likely killed and caching is temporarily off. Run `dp daemon status`; if it answers `Daemon is not running.`, run `dp daemon restart` (see the "dp daemon" section of [commands.md](commands.md)).

## Command reference

Full per-command reference lives in [commands.md](commands.md). Quick lookup:

| Task | Command |
| --- | --- |
| Yield / Pass / Fail / per-Site summary | `dp export lsum` |
| Just the four yield rows (global + per site, wide table) | `dp export yield` |
| Test-item statistics (Cp / Cpk / quantiles, 30+ columns) | `dp export tsum` |
| Per-part detail (Bin / coordinates / Pass / round) | `dp export psum` |
| HardBin / SoftBin distribution (with per-site counts) | `dp export hbin` / `dp export sbin` |
| yield + hbin + sbin in one report (with per-site columns) | `dp export sum` |
| Test-item list (reusable as a filter file) | `dp export tidset` |
| Test-item list + empty rename columns | `dp export tidrenameset` |
| Raw test values (large — mind the size) | `dp export raw` |
| Lot basics (LotID / product / program / site) | `dp export info` |
| Infer retest relations from multiple STDF files → Setup config | `dp create setup` |
| Program versions: TestID coverage + limit diffs (2..N files) | `dp task testidchange` |
| Version information | `dp show version` |
| Daemon lifecycle / cache management | `dp daemon status / stop / restart / flush` |

Input conventions: bare paths accept `.stdf` / `.std` / `.stdf.gz` / `.std.gz`, or a `.json` config produced by `create setup`. Other formats (e.g. `.summ`) are not supported.

Predefined task pipelines:

| Task | Command |
| --- | --- |
| Retest SBin flow and recovery rate | `dp task retestanalysis <setup.json>` |
| Statistics long-table over files × TestIDSet | `dp task paramstat <files> --tidset <csv>` |
| Excel-driven check pipeline | `dp task checkflow <config.xlsx>` |
| TestID coverage + limit diffs across programs | `dp task testidchange <files...>` |

Not implemented — do not expect them: `dp plot tmap` / `dp plot wmap`.

## Scenario → command

(Full decision tables and output-size guidance: [ai-guide.md](ai-guide.md))

| User request | Recommended command | Notes |
| --- | --- | --- |
| "What is the yield of this STDF?" | `dp export lsum` / `dp export yield` | tiny output (< 1 KB); `yield` is a wide table, easier for site-by-site comparison |
| "Which test items have a high fail rate?" | `dp export tsum`, then sort by `FailedRate` | ~100 KB |
| "What is the bin distribution per die?" | `dp export psum` / `hbin` | |
| "This lot has retests — export final data only" | `dp export raw` (default: final rounds only) | **heavy output, always pass `-o`** |
| "Export only parameters in my item list" | `dp export tidset` first, then `dp export raw -t <list>` | |
| "Run 100 files in one go" | `"D:\data\**\*.stdf"` | globbing is done by dp itself |

## Behavior at a glance

- **Data commands write files**: results are not printed to the terminal (it only shows status and warnings). Default output path is "same directory as the first input file" + `<filename>.<subcommand>.csv`; use `-o/--output` to override.
- **Daemon is automatic**: data commands try the resident daemon first (it caches loaded DataPrism instances) and fall back to standalone when unavailable; results are identical. If the daemon is killed externally, caching is temporarily off — `dp daemon status` shows the state and `dp daemon restart` restores it.
- **Exit codes**: `0` = success, non-zero = failure (`1` argument error / `2` file missing / `3` invalid STDF … full table in [ai-guide.md](ai-guide.md)).
- **File types are exclusive**: within one command you cannot mix STDF with `.json`, compressed with uncompressed files, or unrelated types such as `.summ`.

## Documentation map

| File | Contents |
| --- | --- |
| `README.md` (Chinese) | Positioning, how to get dp, quick start, scenario entry points |
| [README.en.md](README.en.md) (this file) | English version of the above |
| [commands.md](commands.md) | Per-command reference: syntax / options / inputs and outputs / output columns and samples |
| [ai-guide.md](ai-guide.md) | Integration guide for AI / scripts: command selection, output-size tiers, exit codes, failure modes |

> `commands.md` and `ai-guide.md` are currently **Chinese-only**.

## Maintainer conventions (important)

1. **This folder is maintained directly in the public repository** — it is not part of the `Examples/` mirror and no sync script overwrites it.
2. **Content must match actual dp behavior**: when adding or changing commands, update `commands.md` (look-up table + section) and the scenario table here in the same change.
3. Document only **implemented** behavior (`dp --help` is the reference); mark unimplemented features explicitly (e.g. `plot tmap/wmap`).
4. Use **placeholder values** in all examples (`demo_lot.stdf`, `DEMO-001`) — never real product lots or file names.
5. Reference examples in `Examples/` with relative links; do not copy code blocks into these documents.
6. Keep binaries, installers and real data files out of this folder (repository rule, see top-level [AGENTS.md](../AGENTS.md)).
