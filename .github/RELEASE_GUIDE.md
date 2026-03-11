# Release Guide - Grondo

Complete guide for releasing new versions of Grondo.

---

## Quick Start

**TL;DR:** Use the release script, GitHub Actions does everything else.

```bash
# Update CHANGELOG.md first, then:
./RELEASE.sh 1.1.0

# Confirm when prompted, then wait 2-3 minutes
# Package automatically published to NuGet.org
```

**Or manually:**
```bash
git tag -a v1.1.0 -m "Release v1.1.0"
git push origin v1.1.0
```

---

## Table of Contents

1. [How It Works](#how-it-works)
2. [Release Process](#release-process)
3. [GitHub Actions](#github-actions)
4. [Versioning with MinVer](#versioning-with-minver)
5. [CHANGELOG Management](#changelog-management)
6. [Troubleshooting](#troubleshooting)
7. [Examples](#examples)

---

## How It Works

### Fully Automated Release

GitHub Actions handles everything automatically:

1. ✅ **Detects Git tag** (e.g., `v1.1.0`)
2. ✅ **Reads version from tag** using MinVer
3. ✅ **Builds** the solution
4. ✅ **Runs all tests** (664 tests)
5. ✅ **Creates NuGet package** with correct version
6. ✅ **Publishes to NuGet.org** automatically
7. ✅ **Publishes to GitHub Packages** automatically

**You only need to:** Push a Git tag

---

## Release Process

### Step 1: Update CHANGELOG.md

Move `[Unreleased]` section to new version:

```markdown
## [Unreleased]

---

## [1.1.0] - 2026-03-11

### Added
- New features...

### Fixed
- Bug fixes...
```

Update version links at bottom:

```markdown
[Unreleased]: https://github.com/gt-downunder/dotnet-library/compare/v1.1.0...HEAD
[1.1.0]: https://github.com/gt-downunder/dotnet-library/compare/v1.0.42...v1.1.0
```

---

### Step 2: Commit Changes

```bash
git add CHANGELOG.md
git commit -m "Release v1.1.0"
```

---

### Step 3: Create and Push Tag

**Option A: Automated Script (Recommended)**

```bash
./RELEASE.sh <version>
```

**Examples:**
```bash
./RELEASE.sh 1.1.0  # Minor release (new features)
./RELEASE.sh 1.0.1  # Patch release (bug fixes)
./RELEASE.sh 2.0.0  # Major release (breaking changes)
```

**What it does:**
1. Validates version format (MAJOR.MINOR.PATCH)
2. Verifies build and tests locally
3. Shows git status
4. **Asks for confirmation** before committing
5. Commits changes
6. Creates tag (e.g., v1.1.0)
7. **Asks for confirmation** before pushing
8. Pushes to GitHub (main + tag)
9. GitHub Actions takes over!

**Safety features:**
- ✅ Two confirmation prompts (can abort anytime)
- ✅ Validates version format
- ✅ Runs tests before committing
- ✅ Shows what will be done before doing it

---

**Option B: Manual Commands**

```bash
# Create annotated tag
git tag -a v1.1.0 -m "Release v1.1.0

Major improvements:
- Added Either<L,R> and Validation<T> types
- LINQ query syntax support
- 60+ new methods
- Bug fixes and performance improvements"

# Push tag (triggers GitHub Actions)
git push origin main
git push origin v1.1.0
```

---

### Step 4: Monitor GitHub Actions

1. Go to: https://github.com/gt-downunder/dotnet-library/actions
2. Find the workflow triggered by your tag
3. Wait for green checkmark ✅ (2-3 minutes)
4. Verify on NuGet.org: https://www.nuget.org/packages/Grondo

**Done!** Package is live.

---

## GitHub Actions

### Workflow File

**Location:** `.github/workflows/build-and-publish.yml`

### Triggers

The workflow runs automatically on:

**1. Push to `main` branch:**
```bash
git push origin main
```
- Builds and tests
- Creates pre-release package (e.g., `1.1.1-preview.5`)
- Publishes to NuGet.org as pre-release

**2. Push version tag:**
```bash
git push origin v1.1.0
```
- Builds and tests
- Creates stable package (e.g., `1.1.0`)
- Publishes to NuGet.org as stable release

**3. Pull requests:**
- Builds and tests only
- Does NOT publish

**4. Manual trigger:**
- Via GitHub Actions UI
- Select branch and run

---

### What It Does

**Build Job (2-3 minutes):**
```
✅ Checkout code (with full Git history)
✅ Setup .NET 10 SDK
✅ Restore dependencies
✅ Build solution (Release mode)
✅ Display calculated version (MinVer)
✅ Run tests with coverage (664 tests)
✅ Pack NuGet package
✅ Upload artifact
```

**Publish Job (30 seconds):**
```
✅ Download artifact
✅ Setup .NET 10 SDK
✅ Push to NuGet.org
✅ Push to GitHub Packages
```

---

### Secrets Required

**NUGET_API_KEY:**
- **Required:** Yes
- **Purpose:** Publish to NuGet.org
- **Setup:**
  1. Get API key: https://www.nuget.org/account/apikeys
  2. Add to GitHub: Settings → Secrets → Actions
  3. Name: `NUGET_API_KEY`
  4. Value: (your API key)

**GITHUB_TOKEN:**
- **Required:** No (automatic)
- **Purpose:** Publish to GitHub Packages
- **Setup:** None needed

---

## Versioning with MinVer

### How MinVer Works

MinVer automatically calculates version from Git tags:

**On tag v1.1.0:**
```
MinVer: Calculated version 1.1.0
→ Package: Grondo.1.1.0.nupkg (stable)
```

**After v1.1.0 + 3 commits:**
```
MinVer: Calculated version 1.1.1-preview.3
→ Package: Grondo.1.1.1-preview.3.nupkg (pre-release)
```

**No manual version management needed!**

---

### Configuration

**File:** `src/Grondo.csproj`

```xml
<MinVerTagPrefix>v</MinVerTagPrefix>
<!-- Tags must start with 'v' (e.g., v1.1.0) -->

<MinVerMinimumMajorMinor>1.0</MinVerMinimumMajorMinor>
<!-- Minimum version is 1.0.x -->

<MinVerDefaultPreReleaseIdentifiers>preview</MinVerDefaultPreReleaseIdentifiers>
<!-- Pre-release versions use 'preview' -->
```

**File:** `Directory.Build.props`

```xml
<LangVersion>14</LangVersion>
<Deterministic>true</Deterministic>
<PublishRepositoryUrl>true</PublishRepositoryUrl>
```

---

### Semantic Versioning

Given a version number `MAJOR.MINOR.PATCH`:

- **MAJOR** (2.0.0) - Breaking changes
- **MINOR** (1.1.0) - New features, backward compatible
- **PATCH** (1.0.1) - Bug fixes only

**Examples:**
- Add `Either<L,R>` type → **MINOR** (v1.1.0)
- Fix thread-safety bug → **PATCH** (v1.0.1)
- Remove deprecated method → **MAJOR** (v2.0.0)

---

## CHANGELOG Management

### Format

We follow [Keep a Changelog](https://keepachangelog.com/) format:

```markdown
## [Unreleased]

### Added
- New features

### Changed
- Changes in existing functionality

### Deprecated
- Soon-to-be removed features

### Removed
- Removed features

### Fixed
- Bug fixes

### Security
- Security fixes

---

## [1.1.0] - 2026-03-11

### Added
- Feature description

### Fixed
- Bug fix description
```

---

### When to Update

**During Development:**
- Add entries to `[Unreleased]` as you develop
- Group related changes together
- Use clear, user-focused language

**Before Release:**
1. Move `[Unreleased]` changes to new version section
2. Add release date
3. Update version links at bottom
4. Create new empty `[Unreleased]` section

---

### Good vs Bad Entries

**✅ Good:**
```markdown
### Added
- `Either<L,R>` type for representing success/failure with typed errors
- LINQ query syntax support for `Result<T>` via `Select` and `SelectMany`
```

**❌ Bad:**
```markdown
### Added
- New stuff
```

**✅ Good:**
```markdown
### Fixed
- Thread-safety issue in `StringFactory.CreateRandomString` - now uses `Random.Shared`
```

**❌ Bad:**
```markdown
### Fixed
- Fixed bug in PR #123
```

---

## Troubleshooting

### Wrong Version Published

**Problem:** MinVer calculates wrong version  
**Cause:** Git history not fetched  
**Solution:**
- Verify `fetch-depth: 0` in `.github/workflows/build-and-publish.yml`
- Check Git tags exist: `git tag -l`
- Re-run workflow

---

### Publish Fails

**Problem:** "401 Unauthorized" when publishing  
**Cause:** Invalid or missing NUGET_API_KEY  
**Solution:**
1. Check secret exists: Settings → Secrets → Actions
2. Verify API key is valid on NuGet.org
3. Check API key has "Push" permission

---

### Package Not on NuGet

**Problem:** Workflow succeeds but package not visible  
**Cause:** NuGet indexing delay  
**Solution:**
1. Wait 5-10 minutes
2. Check NuGet.org package page
3. Verify publish logs show success

---

### Pre-release Instead of Stable

**Problem:** Package published as pre-release  
**Cause:** Not on a version tag  
**Solution:**
1. Verify you pushed a tag: `git tag -l`
2. Check workflow was triggered by tag
3. Look for `refs/tags/v1.1.0` in workflow logs

---

### Need to Rollback

**Problem:** Need to undo release  
**Solution:**

```bash
# Delete Git tag
git tag -d v1.1.0
git push origin :refs/tags/v1.1.0

# Unlist NuGet package (don't delete)
# Go to: https://www.nuget.org/packages/Grondo/1.1.0
# Click "Manage Package" → "Unlist"
```

---

## Examples

### Example 1: Minor Release (New Features)

```bash
# Current: v1.0.42
# Changes: Added Either<L,R>, Validation<T>, LINQ syntax, 60+ methods

# 1. Update CHANGELOG.md
## [1.1.0] - 2026-03-11
### Added
- Either<L,R> and Validation<T> types
- LINQ query syntax support
- 60+ new methods

# 2. Use automated script (recommended)
./RELEASE.sh 1.1.0

# OR manual commands:
# git add CHANGELOG.md
# git commit -m "Release v1.1.0"
# git tag -a v1.1.0 -m "Release v1.1.0"
# git push origin main
# git push origin v1.1.0

# 3. Result
# MinVer: 1.1.0
# Package: Grondo.1.1.0.nupkg (stable)
```

---

### Example 2: Patch Release (Bug Fixes)

```bash
# Current: v1.1.0
# Changes: Fixed thread-safety bug

# 1. Update CHANGELOG.md
## [1.1.1] - 2026-03-15
### Fixed
- Thread-safety issue in StringFactory

# 2. Use automated script (recommended)
./RELEASE.sh 1.1.1

# OR manual commands:
# git add CHANGELOG.md
# git commit -m "Release v1.1.1"
# git tag -a v1.1.1 -m "Release v1.1.1 - Bug fixes"
# git push origin main
# git push origin v1.1.1

# 3. Result
# MinVer: 1.1.1
# Package: Grondo.1.1.1.nupkg (stable)
```

---

### Example 3: Pre-release (No Tag)

```bash
# Current: v1.1.0 + 5 commits
# Changes: Working on new features

# 1. Push to main
git push origin main

# 2. Result
# MinVer: 1.1.1-preview.5
# Package: Grondo.1.1.1-preview.5.nupkg (pre-release)
```

---

## Summary

**Release process:**
1. Update CHANGELOG.md
2. Run release script: `./RELEASE.sh <version>`
3. Confirm when prompted
4. GitHub Actions handles the rest

**Examples:**
```bash
./RELEASE.sh 1.1.0  # Minor release (new features)
./RELEASE.sh 1.0.1  # Patch release (bug fixes)
./RELEASE.sh 2.0.0  # Major release (breaking changes)
```

**No manual build, test, or publish steps required!**

**Everything is automated with GitHub Actions and MinVer.** 🚀

