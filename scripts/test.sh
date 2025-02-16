#!/bin/bash

# Strict error handling
set -euo pipefail

# Color codes for better readability
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Initialize failure tracking
FAILED=0

# Function to log with timestamp
log() {
    echo "[$(date '+%Y-%m-%d %H:%M:%S')] $1"
}

# Function to check command existence
check_command() {
    local cmd=$1
    local pkg=$2
    if ! command -v "$cmd" >/dev/null 2>&1; then
        log "${RED}❌ $cmd not found (package: $pkg)${NC}"
        return 1
    else
        log "${GREEN}✅ $cmd is available${NC}"
        return 0
    fi
}

# Function to check library existence
check_library() {
    local lib=$1
    local pattern=$2
    if ! ldconfig -p | grep -q "$pattern"; then
        log "${RED}❌ $lib not found in library path${NC}"
        return 1
    else
        log "${GREEN}✅ $lib is available${NC}"
        return 0
    fi
}

# Log test start
log "=== Starting Test ==="
log "Testing installation on $(cat /etc/os-release | grep PRETTY_NAME | cut -d= -f2 | tr -d '"')"
log "Working directory: $(pwd)"
log "System information:"
uname -a

# Pre-installation component check
log "=== Pre-Installation Component Check ==="
for cmd in aplay alsamixer openal-info pavucontrol; do
    check_command "$cmd" "${cmd}" || true
done

# Run installation script
log "=== Running Installation Script ==="
if ! ./install-linux-deps.sh; then
    log "${RED}❌ Installation script failed${NC}"
    FAILED=1
fi

# Verify installations
log "=== Verifying Installations ==="

# Check ALSA components
if ! check_command "aplay" "alsa-utils"; then
    FAILED=1
fi
if ! check_command "alsamixer" "alsa-utils"; then
    FAILED=1
fi

# Check OpenAL components
if ! check_command "openal-info" "openal-info"; then
    log "${YELLOW}⚠️  openal-info not available, checking library directly${NC}"
fi

# Check OpenAL library
if ! check_library "OpenAL" "libopenal.so"; then
    FAILED=1
fi

# Check PulseAudio components
if ! check_command "pavucontrol" "pavucontrol"; then
    FAILED=1
fi

# Check audio group configuration
log "=== Checking Audio Configuration ==="
if getent group audio >/dev/null 2>&1; then
    log "${GREEN}✅ 'audio' group exists${NC}"
else
    log "${RED}❌ 'audio' group not found${NC}"
    FAILED=1
fi

# Print test summary
log "=== Test Summary ==="
if [ $FAILED -eq 0 ]; then
    log "${GREEN}✅ All components verified successfully${NC}"
    exit 0
else
    log "${RED}❌ One or more components failed verification${NC}"
    exit 1
fi 