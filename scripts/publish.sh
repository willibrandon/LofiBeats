#!/bin/bash

# Color codes
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Get root directory (one level up from scripts)
ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

# Default version
VERSION="1.0.0"

# Help text
show_help() {
    echo "Usage: $0 --runtime <runtime> [--version <version>]"
    echo
    echo "Options:"
    echo "  --runtime   Runtime to publish for (win-x64, linux-x64, osx-x64)"
    echo "  --version   Version number (default: 1.0.0)"
    exit 1
}

# Parse arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --runtime)
            RUNTIME="$2"
            shift 2
            ;;
        --version)
            VERSION="$2"
            shift 2
            ;;
        *)
            show_help
            ;;
    esac
done

# Validate runtime
if [[ ! "$RUNTIME" =~ ^(win-x64|linux-x64|osx-x64)$ ]]; then
    echo -e "${RED}Error: Invalid runtime. Must be win-x64, linux-x64, or osx-x64${NC}"
    show_help
fi

echo -e "${CYAN}🎵 Publishing LofiBeats v$VERSION for $RUNTIME...${NC}"

# First run our build script
echo -e "${YELLOW}Running build script...${NC}"
if ! "$ROOT_DIR/scripts/build.sh" --release; then
    echo -e "${RED}Build failed${NC}"
    exit 1
fi

# Create publish directory
PUBLISH_DIR="$ROOT_DIR/publish/$RUNTIME"
rm -rf "$PUBLISH_DIR"
mkdir -p "$PUBLISH_DIR"

# Publish CLI and Service
PROJECTS=("src/LofiBeats.Cli" "src/LofiBeats.Service")

for PROJECT in "${PROJECTS[@]}"; do
    echo -e "${YELLOW}Publishing $PROJECT...${NC}"
    if ! dotnet publish "$ROOT_DIR/$PROJECT" \
        --configuration Release \
        --runtime "$RUNTIME" \
        --self-contained true \
        --output "$PUBLISH_DIR" \
        -p:PublishSingleFile=true \
        -p:Version="$VERSION" \
        -p:IncludeNativeLibrariesForSelfExtract=true; then
        echo -e "${RED}Publish failed for $PROJECT${NC}"
        exit 1
    fi
done

# Create archive
ARCHIVE_NAME="lofibeats-$VERSION-$RUNTIME"
ARCHIVE_PATH="$ROOT_DIR/publish/$ARCHIVE_NAME"

echo -e "${YELLOW}Creating archive...${NC}"
if [[ "$RUNTIME" == win-* ]]; then
    # For Windows, create zip
    if command -v zip >/dev/null 2>&1; then
        (cd "$PUBLISH_DIR" && zip -r "$ARCHIVE_PATH.zip" .)
        echo -e "${GREEN}Created $ARCHIVE_PATH.zip${NC}"
    else
        echo -e "${RED}Warning: zip command not found. Archive not created.${NC}"
    fi
else
    # For Linux/macOS, create tar.gz
    tar -czf "$ARCHIVE_PATH.tar.gz" -C "$PUBLISH_DIR" .
    echo -e "${GREEN}Created $ARCHIVE_PATH.tar.gz${NC}"
fi

echo -e "${GREEN}🎉 Publish completed successfully!${NC}" 