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

# Function to check audio setup
check_audio_setup() {
    log "=== Testing Audio Setup ==="
    local failed=0
    
    # Check ALSA configuration
    log "Checking ALSA configuration..."
    if [ -f "/etc/alsa/conf.d/99-dummy.conf" ]; then
        log "${GREEN}✅ ALSA dummy device config exists${NC}"
        
        # Verify the dummy device is properly configured
        if grep -q "pcm.!default" "/etc/alsa/conf.d/99-dummy.conf" && \
           grep -q "ctl.!default" "/etc/alsa/conf.d/99-dummy.conf"; then
            log "${GREEN}✅ ALSA dummy device properly configured${NC}"
        else
            log "${RED}❌ ALSA dummy device configuration is incomplete${NC}"
            failed=1
        fi
    else
        log "${RED}❌ ALSA dummy device config missing${NC}"
        failed=1
    fi
    
    # Test ALSA functionality
    log "Testing ALSA configuration..."
    if aplay -l 2>&1 | grep -q "no soundcards found"; then
        log "${RED}❌ ALSA cannot detect any devices (including dummy)${NC}"
        failed=1
    else
        log "${GREEN}✅ ALSA device detection working${NC}"
    fi
    
    # Verify ALSA libraries
    log "Checking ALSA libraries..."
    if ldconfig -p | grep -q "libasound.so"; then
        log "${GREEN}✅ ALSA libraries installed${NC}"
    else
        log "${RED}❌ ALSA libraries missing${NC}"
        failed=1
    fi
    
    # Test OpenAL
    log "Testing OpenAL..."
    if ! command -v openal-info >/dev/null 2>&1; then
        log "${RED}❌ openal-info not available${NC}"
        failed=1
    else
        local openal_output
        openal_output=$(openal-info 2>&1)
        
        # Check for basic OpenAL functionality
        if echo "$openal_output" | grep -q "Available playback devices"; then
            log "${GREEN}✅ OpenAL device enumeration working${NC}"
        else
            log "${RED}❌ OpenAL cannot enumerate devices${NC}"
            failed=1
        fi
        
        # Check for specific OpenAL components
        if echo "$openal_output" | grep -q "ALC version"; then
            log "${GREEN}✅ OpenAL ALC subsystem working${NC}"
        else
            log "${RED}❌ OpenAL ALC subsystem not working${NC}"
            failed=1
        fi
    fi
    
    # Check OpenAL libraries
    log "Checking OpenAL libraries..."
    if ldconfig -p | grep -q "libopenal.so"; then
        log "${GREEN}✅ OpenAL libraries installed${NC}"
    else
        log "${RED}❌ OpenAL libraries missing${NC}"
        failed=1
    fi
    
    # Check PulseAudio setup
    log "Testing PulseAudio configuration..."
    if command -v pulseaudio >/dev/null 2>&1; then
        log "${GREEN}✅ PulseAudio installed${NC}"
        
        # Check for PulseAudio ALSA plugin
        if ldconfig -p | grep -q "libasound_module_conf_pulse.so\|libpulse.so"; then
            log "${GREEN}✅ PulseAudio ALSA integration available${NC}"
        else
            log "${RED}❌ PulseAudio ALSA integration missing${NC}"
            failed=1
        fi
    else
        log "${RED}❌ PulseAudio not installed${NC}"
        failed=1
    fi
    
    return $failed
}

# Log test start
log "=== Starting Test ==="
log "Testing installation on $(cat /etc/os-release | grep PRETTY_NAME | cut -d= -f2 | tr -d '"')"
log "Working directory: $(pwd)"
log "System information:"
uname -a

# Check .NET installation
log "=== Checking .NET Installation ==="
if ! command -v dotnet >/dev/null 2>&1; then
    log "${RED}❌ .NET SDK not found${NC}"
    FAILED=1
else
    dotnet_version=$(dotnet --version)
    log "${GREEN}✅ .NET SDK version: $dotnet_version${NC}"
    
    # Verify .NET runtime
    log "Checking .NET runtime..."
    if dotnet --list-runtimes | grep -q "Microsoft.NETCore.App 9.0"; then
        log "${GREEN}✅ .NET 9.0 runtime is installed${NC}"
    else
        log "${RED}❌ .NET 9.0 runtime not found${NC}"
        FAILED=1
    fi
fi

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

# Test audio setup
check_audio_setup || FAILED=1

# Print test summary
log "=== Test Summary ==="
if [ $FAILED -eq 0 ]; then
    log "${GREEN}✅ All components verified successfully${NC}"
    exit 0
else
    log "${RED}❌ One or more components failed verification${NC}"
    exit 1
fi 