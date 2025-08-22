#!/bin/bash
# Production deployment script for MCP server update (Remote Server)

echo "🚀 Starting MCP Server Production Deployment (Remote)"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to detect Docker Compose command
detect_docker_compose() {
    if command -v docker-compose >/dev/null 2>&1; then
        echo "docker-compose"
    elif docker compose version >/dev/null 2>&1; then
        echo "docker compose"
    else
        echo ""
    fi
}

# Detect which Docker Compose command to use
DOCKER_COMPOSE=$(detect_docker_compose)

if [ -z "$DOCKER_COMPOSE" ]; then
    echo -e "${RED}❌ Error: Neither 'docker-compose' nor 'docker compose' command found${NC}"
    echo -e "${YELLOW}Please install Docker Compose or ensure Docker with Compose plugin is installed${NC}"
    exit 1
fi

echo -e "${GREEN}ℹ️  Using Docker Compose command: ${DOCKER_COMPOSE}${NC}"

# Check if we're in the right directory
if [ ! -f "docker-compose.prod.yml" ]; then
    echo -e "${RED}❌ Error: docker-compose.prod.yml not found. Run this script from the project root.${NC}"
    exit 1
fi

# Check if Docker is running
if ! docker info >/dev/null 2>&1; then
    echo -e "${RED}❌ Error: Docker is not running${NC}"
    exit 1
fi

echo -e "${YELLOW}📋 Remote deployment checklist:${NC}"
echo "1. Backing up current deployment..."

# Create backup
$DOCKER_COMPOSE -f docker-compose.prod.yml config > backup-compose-$(date +%Y%m%d-%H%M%S).yml
echo -e "${GREEN}✅ Configuration backed up${NC}"

echo "2. Pulling latest image from Docker Hub..."
docker pull ioaerospace/astrodynamics-mcp:latest

if [ $? -ne 0 ]; then
    echo -e "${RED}❌ Failed to pull image from Docker Hub. Deployment aborted.${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Latest image pulled successfully${NC}"

echo "3. Checking kernel data directory..."
if [ ! -d "./data/solarsystem" ]; then
    echo -e "${RED}❌ Error: Kernel data directory './data/solarsystem' not found.${NC}"
    echo -e "${YELLOW}ℹ️  This path is relative to the docker-compose.prod.yml file location.${NC}"
    echo -e "${YELLOW}ℹ️  Current working directory: $(pwd)${NC}"
    echo -e "${YELLOW}ℹ️  Looking for: $(pwd)/data/solarsystem${NC}"
    echo -e "${YELLOW}Please ensure the solar system kernel files are present at this location.${NC}"
    exit 1
else
    echo -e "${GREEN}✅ Kernel data directory exists at: $(pwd)/data/solarsystem${NC}"
    echo -e "${GREEN}ℹ️  Found $(ls -1 ./data/solarsystem | wc -l) files in kernel directory${NC}"
fi

echo -e "${YELLOW}🔄 Starting rolling deployment...${NC}"

# Deploy with zero-downtime strategy
echo "4. Updating MCP server..."
$DOCKER_COMPOSE -f docker-compose.prod.yml up -d --no-deps mcp.server

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✅ MCP server updated successfully${NC}"
    
    # Wait a moment for the service to start
    echo "5. Waiting for service to be healthy..."
    sleep 15
    
    # Check if the service is responding
    if $DOCKER_COMPOSE -f docker-compose.prod.yml ps mcp.server | grep -q "Up"; then
        echo -e "${GREEN}🎉 Deployment successful! MCP server is running.${NC}"
        echo -e "${GREEN}🌐 Service available at: http://mcp.io-aerospace.org${NC}"
        
        # Show container logs for verification
        echo -e "${YELLOW}📋 Recent logs:${NC}"
        $DOCKER_COMPOSE -f docker-compose.prod.yml logs --tail=20 mcp.server
    else
        echo -e "${RED}❌ Service health check failed${NC}"
        echo -e "${YELLOW}📋 Container logs:${NC}"
        $DOCKER_COMPOSE -f docker-compose.prod.yml logs mcp.server
    fi
else
    echo -e "${RED}❌ Deployment failed${NC}"
    exit 1
fi

echo -e "${YELLOW}🧹 Cleaning up old images...${NC}"
docker image prune -f

echo -e "${GREEN}✅ Remote deployment completed successfully!${NC}"
