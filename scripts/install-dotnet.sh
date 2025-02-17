#!/bin/bash

# Color codes for better readability
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Default values
DOTNET_VERSION="9.0.2"
INSTALL_DIR="$HOME/.dotnet"
ARCH=$(uname -m)

# Map architecture to .NET architecture
case $ARCH in
    "aarch64")
        DOTNET_ARCH="arm64"
        ;;
    "armv7l")
        DOTNET_ARCH="arm"
        ;;
    "x86_64")
        DOTNET_ARCH="x64"
        ;;
    *)
        echo -e "${RED}Unsupported architecture: $ARCH${NC}"
        exit 1
        ;;
esac

# Detect OS and version
if [ -f /etc/os-release ]; then
    . /etc/os-release
    OS=$ID
    VERSION_ID=$VERSION_ID
else
    echo -e "${RED}Cannot detect OS version${NC}"
    exit 1
fi

echo -e "${GREEN}Installing .NET $DOTNET_VERSION for $OS $VERSION_ID ($DOTNET_ARCH)${NC}"

# Create temporary directory for downloads
TMP_DIR=$(mktemp -d)
cd $TMP_DIR

# Download .NET Runtime
echo "Downloading .NET Runtime..."
wget https://download.visualstudio.microsoft.com/download/pr/7f0a9ec1-5cd7-4d47-b0b1-4c32e7c17133/0c9d5cf4ab5cb4a16b32d99fcc1e8cdb/dotnet-runtime-9.0.2-linux-arm64.tar.gz -O dotnet-runtime.tar.gz

# Download ASP.NET Core Runtime
echo "Downloading ASP.NET Core Runtime..."
wget https://download.visualstudio.microsoft.com/download/pr/744cd467-ac89-4656-9633-ed22e3afb35e/4277cdc84219d6515cb14220ddc0bde3/aspnetcore-runtime-9.0.2-linux-arm64.tar.gz -O aspnet-runtime.tar.gz

# Create installation directory
mkdir -p $INSTALL_DIR

# Extract runtimes
echo "Extracting .NET Runtime..."
tar xzf dotnet-runtime.tar.gz -C $INSTALL_DIR
echo "Extracting ASP.NET Core Runtime..."
tar xzf aspnet-runtime.tar.gz -C $INSTALL_DIR

# Cleanup
cd - > /dev/null
rm -rf $TMP_DIR

# Set up environment variables
PROFILE_FILE="$HOME/.profile"
if [ ! -f "$PROFILE_FILE" ]; then
    PROFILE_FILE="$HOME/.bash_profile"
fi

# Add .NET to PATH if not already present
if ! grep -q 'export PATH="$PATH:$HOME/.dotnet"' "$PROFILE_FILE"; then
    echo 'export PATH="$PATH:$HOME/.dotnet"' >> "$PROFILE_FILE"
    echo 'export DOTNET_ROOT="$HOME/.dotnet"' >> "$PROFILE_FILE"
fi

# Make dotnet executable
chmod +x $INSTALL_DIR/dotnet

# Source the profile
source $PROFILE_FILE

# Verify installation
if [ -f "$INSTALL_DIR/dotnet" ]; then
    echo -e "\n${GREEN}Testing .NET installation...${NC}"
    $INSTALL_DIR/dotnet --info
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✓ .NET installation verified${NC}"
        echo -e "${YELLOW}Please run: source $PROFILE_FILE${NC}"
        echo "Or log out and back in for the changes to take effect"
    else
        echo -e "${RED}× .NET installation verification failed${NC}"
        exit 1
    fi
else
    echo -e "${RED}× .NET installation failed${NC}"
    exit 1
fi 