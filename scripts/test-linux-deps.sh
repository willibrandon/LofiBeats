#!/bin/bash

# Color codes for better readability
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check if Docker is available
if ! command -v docker &> /dev/null; then
    echo -e "${RED}Docker is not installed or not in PATH${NC}"
    exit 1
fi

# Check if Docker daemon is running
if ! docker info &> /dev/null; then
    echo -e "${RED}Docker daemon is not running${NC}"
    exit 1
fi

# Ensure we're in the scripts directory
cd "$(dirname "$0")"

# Test matrix - Distribution:Tag pairs
declare -A DISTROS
# Debian-based
DISTROS["debian:12"]="Debian Bookworm"
DISTROS["debian:11"]="Debian Bullseye"
DISTROS["ubuntu:22.04"]="Ubuntu LTS"
DISTROS["ubuntu:23.10"]="Ubuntu Latest"

# RHEL-based
DISTROS["fedora:39"]="Fedora Latest"
DISTROS["fedora:38"]="Fedora Previous"
DISTROS["almalinux:9"]="RHEL 9 Compatible"
DISTROS["rockylinux:9"]="RHEL 9 Compatible"

# Arch-based
DISTROS["archlinux:latest"]="Arch Linux"
DISTROS["manjarolinux/base:latest"]="Manjaro"

# Function to create test Dockerfile
create_dockerfile() {
    local distro=$1
    
    cat > Dockerfile << EOF
FROM ${distro}

# Copy installation script
COPY install-linux-deps.sh /install-linux-deps.sh
RUN chmod +x /install-linux-deps.sh

# Create test script
RUN echo '#!/bin/bash' > /test.sh && \
    echo 'echo "Testing installation on ${distro}"' >> /test.sh && \
    echo './install-linux-deps.sh' >> /test.sh && \
    echo 'exit $?' >> /test.sh && \
    chmod +x /test.sh

CMD ["/test.sh"]
EOF
}

# Function to run test for a distribution
test_distribution() {
    local distro=$1
    local description=$2
    local test_name=$(echo "$distro" | tr ':/' '-')
    
    echo -e "\n${YELLOW}Testing ${distro} (${description})${NC}"
    echo "================================"
    
    # Create Dockerfile for this distribution
    create_dockerfile "$distro"
    
    # Build the test container
    echo -e "${YELLOW}Building test container...${NC}"
    if docker build -t "lofibeats-test-${test_name}" . > "logs/${test_name}-build.log" 2>&1; then
        echo -e "${GREEN}Build successful${NC}"
    else
        echo -e "${RED}Build failed - see logs/${test_name}-build.log${NC}"
        cat "logs/${test_name}-build.log"
        return 1
    fi
    
    # Run the test
    echo -e "${YELLOW}Running installation test...${NC}"
    if docker run --rm "lofibeats-test-${test_name}" > "logs/${test_name}-run.log" 2>&1; then
        echo -e "${GREEN}Test passed${NC}"
        return 0
    else
        echo -e "${RED}Test failed - see logs/${test_name}-run.log${NC}"
        cat "logs/${test_name}-run.log"
        return 1
    fi
}

# Create logs directory
mkdir -p logs

# Track results
declare -A RESULTS
PASSED=0
FAILED=0

# Run tests for each distribution
for distro in "${!DISTROS[@]}"; do
    if test_distribution "$distro" "${DISTROS[$distro]}"; then
        RESULTS["$distro"]="✅ PASSED"
        ((PASSED++))
    else
        RESULTS["$distro"]="❌ FAILED"
        ((FAILED++))
    fi
done

# Print summary
echo -e "\n${GREEN}Test Summary${NC}"
echo "=============="
for distro in "${!RESULTS[@]}"; do
    echo -e "${distro} (${DISTROS[$distro]}): ${RESULTS[$distro]}"
done
echo "=============="
echo -e "Total: $((PASSED + FAILED))"
echo -e "${GREEN}Passed: ${PASSED}${NC}"
echo -e "${RED}Failed: ${FAILED}${NC}"

# Cleanup
echo -e "\n${YELLOW}Cleaning up...${NC}"
rm -f Dockerfile

# Offer to clean up images
read -p "Do you want to remove the test Docker images? (y/N) " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo -e "${YELLOW}Removing test images...${NC}"
    for distro in "${!DISTROS[@]}"; do
        test_name=$(echo "$distro" | tr ':/' '-')
        docker rmi "lofibeats-test-${test_name}" &> /dev/null
    done
    echo -e "${GREEN}Cleanup complete${NC}"
fi

# Exit with status based on results
if [ "$FAILED" -eq 0 ]; then
    echo -e "\n${GREEN}All tests passed!${NC}"
    exit 0
else
    echo -e "\n${RED}Some tests failed. Check logs directory for details.${NC}"
    exit 1
fi 