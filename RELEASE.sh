#!/bin/bash
# Grondo Release Script
# This script commits, tags, and pushes to GitHub
# GitHub Actions will automatically build, test, and publish to NuGet
#
# Usage: ./RELEASE.sh <version>
# Example: ./RELEASE.sh 1.1.0

set -e  # Exit on error

# Check if version argument is provided
if [ -z "$1" ]; then
    echo "❌ Error: Version number required"
    echo ""
    echo "Usage: ./RELEASE.sh <version>"
    echo "Example: ./RELEASE.sh 1.1.0"
    echo ""
    echo "Version format: MAJOR.MINOR.PATCH"
    echo "  - MAJOR: Breaking changes (e.g., 2.0.0)"
    echo "  - MINOR: New features (e.g., 1.1.0)"
    echo "  - PATCH: Bug fixes (e.g., 1.0.1)"
    echo ""
    exit 1
fi

VERSION="$1"

# Validate version format (basic check)
if ! [[ $VERSION =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
    echo "❌ Error: Invalid version format: $VERSION"
    echo "Expected format: MAJOR.MINOR.PATCH (e.g., 1.1.0)"
    exit 1
fi

echo "🚀 Grondo Release Script"
echo "================================"
echo "Version: v$VERSION"
echo ""
echo "✨ GitHub Actions will automatically:"
echo "   - Build the solution"
echo "   - Run all tests"
echo "   - Create NuGet package"
echo "   - Publish to NuGet.org"
echo ""
echo "This script only commits and pushes the tag."
echo ""

# Step 1: Verify everything is ready
echo "Step 1: Running final verification..."
dotnet clean
dotnet build -c Release
dotnet test -c Release --verbosity minimal
dotnet pack -c Release

echo ""
echo "✅ Verification complete!"
echo ""

# Step 2: Show current status
echo "Step 2: Current Git status..."
git status

echo ""
read -p "Ready to commit? (y/n) " -n 1 -r
echo ""
if [[ ! $REPLY =~ ^[Yy]$ ]]
then
    echo "Aborted."
    exit 1
fi

# Step 3: Commit changes
echo "Step 3: Committing changes..."
git add .
git commit -m "Release v$VERSION

See CHANGELOG.md for full details."

echo ""
echo "✅ Changes committed!"
echo ""

# Step 4: Create tag
echo "Step 4: Creating Git tag..."
git tag -a v$VERSION -m "Release v$VERSION

See CHANGELOG.md for full details."

echo ""
echo "✅ Tag created!"
echo ""

# Step 5: Show tag
echo "Step 5: Verifying tag..."
git tag -l -n9 v$VERSION

echo ""
read -p "Ready to push? (y/n) " -n 1 -r
echo ""
if [[ ! $REPLY =~ ^[Yy]$ ]]
then
    echo "Aborted. To delete tag: git tag -d v$VERSION"
    exit 1
fi

# Step 6: Push to GitHub
echo "Step 6: Pushing to GitHub..."
git push origin main
git push origin v$VERSION

echo ""
echo "✅ Pushed to GitHub!"
echo ""

# Step 7: Verify version locally
echo "Step 7: Verifying MinVer version locally..."
dotnet build -c Release 2>&1 | grep MinVer

echo ""
echo "✅ Version verified!"
echo ""

echo "================================"
echo "🎉 Tag Pushed Successfully!"
echo "================================"
echo ""
echo "✨ GitHub Actions is now running:"
echo "   1. Building the solution"
echo "   2. Running all tests"
echo "   3. Creating NuGet package"
echo "   4. Publishing to NuGet.org"
echo ""
echo "Next steps:"
echo "1. Monitor GitHub Actions: https://github.com/gt-downunder/Grondo/actions"
echo "2. Wait for green checkmark ✅"
echo "3. Verify on NuGet.org: https://www.nuget.org/packages/Grondo"
echo "4. (Optional) Create GitHub release: https://github.com/gt-downunder/Grondo/releases/new"
echo ""
echo "See .github/RELEASE_GUIDE.md for complete instructions."
echo ""

