# Linux Installation

For Linux users, we provide a dependency installation script that supports multiple distributions (Debian/Ubuntu, Fedora, Arch):

```bash
# Download and run the dependency installer
curl -sSL https://raw.githubusercontent.com/willibrandon/LofiBeats/main/scripts/install-linux-deps.sh | sudo bash

# Or if you've cloned the repository:
sudo ./scripts/install-linux-deps.sh

# Verify dependencies are installed correctly
curl -sSL https://raw.githubusercontent.com/willibrandon/LofiBeats/main/scripts/verify-linux-setup.sh | bash
```

The script will:
1. Detect your package manager (apt, dnf, pacman)
2. Install required dependencies:
   - OpenAL audio library
   - ALSA utilities
   - Required .NET dependencies
3. Offer to install optional audio utilities
4. Test your audio setup
5. Configure user permissions

After installation, you can verify your setup using our verification script:
```bash
./scripts/verify-linux-setup.sh
```

The verification script will:
- Check all required dependencies
- Test audio system configuration
- Verify permissions and directory structure
- Offer to run an audio playback test
- Provide troubleshooting tips if issues are found

If you prefer manual installation, ensure you have these dependencies:
- OpenAL (libopenal1 on Debian/Ubuntu, openal-soft on Fedora, openal on Arch)
- ALSA utilities
- libgdiplus
- Basic system libraries

After installing dependencies, follow the Quick Start guide above to install and run LofiBeats.