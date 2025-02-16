#!/bin/bash

# Color codes for better readability
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}LofiBeats Linux Dependency Installer${NC}"
echo "This script will check and install required dependencies for LofiBeats"

# Check if running as root
if [ "$EUID" -ne 0 ]; then
    echo -e "${YELLOW}Please run as root to install dependencies:${NC}"
    echo "sudo $0"
    exit 1
fi

# Function to detect package manager
detect_package_manager() {
    if command -v apt-get &> /dev/null; then
        echo "apt"
    elif command -v dnf &> /dev/null; then
        echo "dnf"
    elif command -v pacman &> /dev/null; then
        echo "pacman"
    else
        echo "unknown"
    fi
}

# Function to check if a package is installed
check_package() {
    local pkg_manager=$1
    local package=$2
    
    case $pkg_manager in
        "apt")
            dpkg -l "$package" &> /dev/null
            ;;
        "dnf")
            dnf list installed "$package" &> /dev/null
            ;;
        "pacman")
            pacman -Qi "$package" &> /dev/null
            ;;
        *)
            return 1
            ;;
    esac
    
    if [ $? -eq 0 ]; then
        echo -e "✅ ${GREEN}$package is installed${NC}"
        return 0
    else
        echo -e "❌ ${RED}$package is not installed${NC}"
        return 1
    fi
}

# Function to install a package
install_package() {
    local pkg_manager=$1
    local package=$2
    
    echo -e "${YELLOW}Installing $package...${NC}"
    case $pkg_manager in
        "apt")
            apt-get install -y "$package"
            ;;
        "dnf")
            dnf install -y "$package"
            ;;
        "pacman")
            pacman -S --noconfirm "$package"
            ;;
        *)
            echo -e "${RED}Unsupported package manager${NC}"
            return 1
            ;;
    esac
}

# Detect package manager
PKG_MANAGER=$(detect_package_manager)
if [ "$PKG_MANAGER" = "unknown" ]; then
    echo -e "${RED}Unable to detect package manager. Please install dependencies manually:${NC}"
    echo "- OpenAL (libopenal)"
    echo "- ALSA utilities"
    echo "- .NET dependencies (libgdiplus, libc6)"
    exit 1
fi

# Update package list
echo -e "\n${YELLOW}Updating package list...${NC}"
case $PKG_MANAGER in
    "apt")
        apt-get update
        ;;
    "dnf")
        dnf check-update
        ;;
    "pacman")
        pacman -Sy
        ;;
esac

# Define packages for different package managers
declare -A PACKAGES
case $PKG_MANAGER in
    "apt")
        PACKAGES=(
            ["openal"]="libopenal1"
            ["alsa"]="alsa-utils"
            ["gdiplus"]="libgdiplus"
            ["libc"]="libc6"
            ["pulseaudio"]="pulseaudio"
            ["pavucontrol"]="pavucontrol"
        )
        ;;
    "dnf")
        PACKAGES=(
            ["openal"]="openal-soft"
            ["alsa"]="alsa-utils"
            ["gdiplus"]="libgdiplus"
            ["libc"]="glibc"
            ["pulseaudio"]="pulseaudio"
            ["pavucontrol"]="pavucontrol"
        )
        ;;
    "pacman")
        PACKAGES=(
            ["openal"]="openal"
            ["alsa"]="alsa-utils"
            ["gdiplus"]="libgdiplus"
            ["libc"]="glibc"
            ["pulseaudio"]="pulseaudio"
            ["pavucontrol"]="pavucontrol"
        )
        ;;
esac

# Check and install required packages
echo -e "\n${GREEN}Checking required packages...${NC}"
FAILED=0

for pkg in "${!PACKAGES[@]}"; do
    if ! check_package "$PKG_MANAGER" "${PACKAGES[$pkg]}"; then
        install_package "$PKG_MANAGER" "${PACKAGES[$pkg]}"
        # Verify installation
        if ! check_package "$PKG_MANAGER" "${PACKAGES[$pkg]}"; then
            echo -e "${RED}Failed to install ${PACKAGES[$pkg]}${NC}"
            FAILED=1
        fi
    fi
done

# Test audio setup
echo -e "\n${GREEN}Testing audio setup...${NC}"
echo "Available audio devices:"
if ! command -v aplay &> /dev/null; then
    echo -e "${RED}aplay not found - ALSA installation failed${NC}"
    FAILED=1
else
    aplay -l || FAILED=1
fi

# Test OpenAL
if command -v openal-info &> /dev/null; then
    echo -e "\n${GREEN}Testing OpenAL...${NC}"
    openal-info || FAILED=1
else
    echo -e "${YELLOW}openal-info not available. OpenAL is installed but the test utility is not.${NC}"
    # Check for OpenAL library
    if ! test -e /usr/lib*/libopenal.so*; then
        echo -e "${RED}OpenAL library not found${NC}"
        FAILED=1
    fi
fi

# Create LofiBeats directory
LOFI_DIR="$HOME/.local/share/LofiBeats"
mkdir -p "$LOFI_DIR" || FAILED=1

echo -e "\n${YELLOW}IMPORTANT: .NET Runtime Requirement${NC}"
echo "LofiBeats requires the .NET runtime to be installed. If you haven't installed it yet:"
echo "1. Visit https://dotnet.microsoft.com/download/dotnet"
echo "2. Follow the installation instructions for your Linux distribution"
echo "3. Run 'dotnet --info' to verify the installation"
echo ""
echo "For detailed instructions, visit:"
echo "https://learn.microsoft.com/dotnet/core/install/linux"
echo ""

if [ $FAILED -eq 0 ]; then
    echo -e "${GREEN}Setup complete!${NC}"
    echo "You can now run LofiBeats. If you experience audio issues:"
    echo "1. Run 'alsamixer' to check volume levels"
    echo "2. Run 'pavucontrol' to configure PulseAudio settings"
    echo "3. Ensure your user is in the 'audio' group: sudo usermod -a -G audio $USER"
    exit 0
else
    echo -e "${RED}Setup failed - some components could not be installed or verified${NC}"
    exit 1
fi 