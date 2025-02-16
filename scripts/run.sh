#!/bin/bash

# Exit on error
set -e

# Get the root directory (parent of scripts directory)
ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

# Check if we need to build first
CLI_PATH="$ROOT_DIR/src/LofiBeats.Cli/bin/Debug/net9.0/LofiBeats.Cli.dll"
if [ ! -f "$CLI_PATH" ]; then
    echo "🔨 Building LofiBeats..."
    dotnet build "$ROOT_DIR/src/LofiBeats.Cli" --verbosity minimal
fi

# Run the CLI with any provided arguments
echo "🎵 Running LofiBeats..."
dotnet exec "$CLI_PATH" "$@"

# Preserve the exit code
exit $? 