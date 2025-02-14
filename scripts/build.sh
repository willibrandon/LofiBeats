#!/bin/bash

# Color codes
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
GRAY='\033[0;37m'
RED='\033[0;31m'
GREEN='\033[0;32m'
NC='\033[0m' # No Color

# Script arguments
RELEASE=0
NO_BUILD=0
NO_TEST=0

# Parse arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --release)
            RELEASE=1
            shift
            ;;
        --no-build)
            NO_BUILD=1
            shift
            ;;
        --no-test)
            NO_TEST=1
            shift
            ;;
        *)
            echo "Unknown option: $1"
            exit 1
            ;;
    esac
done

# Get root directory (one level up from scripts)
ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

echo -e "${CYAN}🎵 Building LofiBeats...${NC}"

# Try to gracefully shutdown any running service
echo -e "${YELLOW}Ensuring no service is running...${NC}"
if dotnet run --project "$ROOT_DIR/src/LofiBeats.Cli" -- shutdown > /dev/null 2>&1; then
    echo -e "${GRAY}Service stopped gracefully${NC}"
else
    echo -e "${GRAY}No running service found${NC}"
fi

# Force kill any remaining dotnet processes that might be our service
# Note: This is more targeted on Linux/macOS to avoid killing unrelated processes
ps aux | grep "LofiBeats.Service" | grep -v grep | awk '{print $2}' | while read -r pid ; do
    if kill -9 "$pid" 2>/dev/null; then
        echo -e "${GRAY}Cleaned up process $pid${NC}"
    else
        echo -e "${RED}Failed to clean up process $pid${NC}"
    fi
done

if [ $NO_BUILD -eq 0 ]; then
    # Clean
    echo -e "${YELLOW}Cleaning solution...${NC}"
    if ! dotnet clean "$ROOT_DIR/LofiBeats.sln" --verbosity minimal; then
        echo -e "${RED}Clean failed${NC}"
        exit 1
    fi

    # Restore
    echo -e "${YELLOW}Restoring packages...${NC}"
    if ! dotnet restore "$ROOT_DIR/LofiBeats.sln"; then
        echo -e "${RED}Restore failed${NC}"
        exit 1
    fi

    # Build
    CONFIG=$([ $RELEASE -eq 1 ] && echo "Release" || echo "Debug")
    echo -e "${YELLOW}Building solution ($CONFIG)...${NC}"
    if ! dotnet build "$ROOT_DIR/LofiBeats.sln" --configuration $CONFIG --no-restore; then
        echo -e "${RED}Build failed${NC}"
        exit 1
    fi
fi

if [ $NO_TEST -eq 0 ]; then
    # Run tests
    echo -e "${YELLOW}Running tests...${NC}"
    if ! dotnet test "$ROOT_DIR/LofiBeats.sln" --no-build; then
        echo -e "${RED}Tests failed${NC}"
        exit 1
    fi
fi

echo -e "${GREEN}🎉 Build completed successfully!${NC}" 