#!/bin/bash

# Color codes for better readability
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Track overall status
ERRORS=0
WARNINGS=0

# Helper functions
print_header() {
    echo -e "\n${BLUE}=== $1 ===${NC}"
}

print_success() {
    echo -e "✅ ${GREEN}$1${NC}"
}

print_warning() {
    echo -e "⚠️ ${YELLOW}$1${NC}"
    ((WARNINGS++))
}

print_error() {
    echo -e "❌ ${RED}$1${NC}"
    ((ERRORS++))
}

# Start verification
echo -e "${GREEN}LofiBeats Linux Setup Verification${NC}"
echo "This script will verify your system setup for running LofiBeats"

# Check if running as regular user (not root)
if [ "$EUID" -eq 0 ]; then
    print_error "This script should not be run as root. Please run as regular user."
    exit 1
fi

# System Information
print_header "System Information"
echo "Distribution: $(cat /etc/os-release | grep "PRETTY_NAME" | cut -d'"' -f2)"
echo "Kernel: $(uname -r)"
echo "Architecture: $(uname -m)"

# Package Manager Detection
print_header "Package Manager"
if command -v apt-get &> /dev/null; then
    print_success "Using apt package manager (Debian/Ubuntu)"
    PKG_MANAGER="apt"
elif command -v dnf &> /dev/null; then
    print_success "Using dnf package manager (Fedora)"
    PKG_MANAGER="dnf"
elif command -v pacman &> /dev/null; then
    print_success "Using pacman package manager (Arch)"
    PKG_MANAGER="pacman"
else
    print_error "Unable to detect package manager"
fi

# Required Package Verification
print_header "Required Packages"

# Define packages based on package manager
case $PKG_MANAGER in
    "apt")
        OPENAL_PKG="libopenal1"
        ALSA_PKG="alsa-utils"
        GDIPLUS_PKG="libgdiplus"
        ;;
    "dnf")
        OPENAL_PKG="openal-soft"
        ALSA_PKG="alsa-utils"
        GDIPLUS_PKG="libgdiplus"
        ;;
    "pacman")
        OPENAL_PKG="openal"
        ALSA_PKG="alsa-utils"
        GDIPLUS_PKG="libgdiplus"
        ;;
    *)
        print_error "Unknown package manager, skipping package verification"
        OPENAL_PKG=""
        ALSA_PKG=""
        GDIPLUS_PKG=""
        ;;
esac

# Check OpenAL
if [ -n "$OPENAL_PKG" ]; then
    if command -v openal-info &> /dev/null; then
        print_success "OpenAL info utility found"
        if openal-info | grep -q "OpenAL version"; then
            print_success "OpenAL is working correctly"
        else
            print_error "OpenAL not working correctly"
        fi
    else
        print_warning "OpenAL info utility not found (not critical)"
    fi
fi

# Check ALSA
print_header "ALSA Audio System"
if command -v aplay &> /dev/null; then
    print_success "ALSA utilities installed"
    
    # Check for audio devices
    if aplay -l | grep -q "card"; then
        print_success "ALSA detects audio devices:"
        aplay -l | grep "card" | sed 's/^/  /'
    else
        print_error "No ALSA audio devices found"
    fi
    
    # Check if default device is set
    if aplay -L | grep -q "^default"; then
        print_success "ALSA default device is configured"
    else
        print_warning "No ALSA default device configured"
    fi
else
    print_error "ALSA utilities not found"
fi

# Check audio permissions
print_header "User Permissions"
if groups | grep -q "audio"; then
    print_success "User has audio group permissions"
else
    print_error "User not in audio group (run: sudo usermod -a -G audio $USER)"
fi

# Test PulseAudio
print_header "PulseAudio"
if command -v pulseaudio &> /dev/null; then
    print_success "PulseAudio installed"
    if pulseaudio --check; then
        print_success "PulseAudio is running"
    else
        print_warning "PulseAudio is not running"
    fi
    
    if command -v pactl &> /dev/null; then
        if pactl list sinks | grep -q "State: RUNNING\|State: IDLE"; then
            print_success "PulseAudio sink available"
        else
            print_warning "No active PulseAudio sink found"
        fi
    fi
else
    print_warning "PulseAudio not installed (optional)"
fi

# Directory Structure
print_header "LofiBeats Directory Structure"
LOFI_DIR="$HOME/.local/share/LofiBeats"
if [ -d "$LOFI_DIR" ]; then
    print_success "LofiBeats directory exists: $LOFI_DIR"
    if [ -w "$LOFI_DIR" ]; then
        print_success "Directory is writable"
    else
        print_error "Directory is not writable"
    fi
else
    print_warning "LofiBeats directory not found (will be created on first run)"
fi

# Audio Playback Test
print_header "Audio Playback Test"
echo -e "${YELLOW}Would you like to test audio playback? This will play a short test tone. (y/N)${NC}"
read -r response
if [[ "$response" =~ ^([yY][eE][sS]|[yY])+$ ]]; then
    echo "Playing test tone... (1 second)"
    if speaker-test -t sine -f 440 -l 1 &> /dev/null; then
        print_success "Audio playback successful"
    else
        print_error "Audio playback failed"
    fi
else
    echo "Skipping audio playback test"
fi

# Summary
print_header "Verification Summary"
echo -e "Errors: ${RED}$ERRORS${NC}"
echo -e "Warnings: ${YELLOW}$WARNINGS${NC}"

if [ $ERRORS -eq 0 ]; then
    if [ $WARNINGS -eq 0 ]; then
        echo -e "\n${GREEN}✅ All checks passed! Your system is ready to run LofiBeats.${NC}"
    else
        echo -e "\n${YELLOW}⚠️ Setup looks good, but there are some warnings to review.${NC}"
    fi
else
    echo -e "\n${RED}❌ Some checks failed. Please review the errors above.${NC}"
fi

# Provide troubleshooting tips if there were issues
if [ $ERRORS -gt 0 ] || [ $WARNINGS -gt 0 ]; then
    print_header "Troubleshooting Tips"
    echo "1. For audio group issues:"
    echo "   sudo usermod -a -G audio $USER"
    echo "   (Log out and back in for changes to take effect)"
    echo
    echo "2. For ALSA/audio device issues:"
    echo "   - Check physical connections"
    echo "   - Run 'alsamixer' to check volume levels"
    echo "   - Run 'pavucontrol' for PulseAudio settings"
    echo
    echo "3. For OpenAL issues:"
    echo "   - Try reinstalling OpenAL: sudo apt-get reinstall libopenal1"
    echo "   - Check OpenAL config: ~/.alsoftrc"
    echo
    echo "For more help, visit: https://github.com/willibrandon/LofiBeats/wiki/Linux-Troubleshooting"
fi 