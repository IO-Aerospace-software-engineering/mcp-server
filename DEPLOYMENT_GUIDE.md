# MCP Server Production Deployment Guide

## 🚀 Deployment Checklist

### ✅ Pre-deployment (Completed)
- [x] Docker image built and pushed to Docker Hub: `ioaerospace/astrodynamics-mcp:latest`
- [x] Production Docker Compose configuration updated
- [x] Deployment script prepared for remote server
- [x] Volume mount configuration verified

### 📦 Files to Transfer to Remote Server
Copy these files to your remote production server:

1. **docker-compose.prod.yml** - Updated production configuration
2. **deploy-production.sh** - Automated deployment script

### 🖥️ Remote Server Setup

1. **Connect to your remote server:**
   ```bash
   ssh your-user@your-production-server
   ```

2. **Navigate to your project directory** (where your existing docker-compose files are)

3. **Backup current setup** (recommended):
   ```bash
   cp docker-compose.yml docker-compose.yml.backup-$(date +%Y%m%d)
   ```

4. **Transfer the new files** (from your local machine):
   ```bash
   # Option 1: Using scp
   scp docker-compose.prod.yml your-user@your-server:/path/to/project/
   scp deploy-production.sh your-user@your-server:/path/to/project/

   # Option 2: Using git (if your files are in git)
   git pull origin main
   ```

5. **Verify kernel data exists on server:**
   ```bash
   ls -la ./data/solarsystem/
   # Should show your kernel files (de440s.bsp, latest_leapseconds.tls, etc.)
   ```

6. **Make deployment script executable:**
   ```bash
   chmod +x deploy-production.sh
   ```

7. **Run the deployment:**
   ```bash
   ./deploy-production.sh
   ```

### 🔍 What the Deployment Will Do

1. Pull the latest image from Docker Hub
2. Backup current configuration
3. Verify kernel data directory exists
4. Update only the MCP server container (zero-downtime)
5. Health check the new container
6. Show deployment logs
7. Clean up old Docker images

### 🌐 Expected Result

After successful deployment:
- Your MCP server will be running the new version
- Available at: http://mcp.io-aerospace.org
- API server remains unchanged and running
- Traefik continues routing traffic normally

### 🚨 Rollback Plan (if needed)

If something goes wrong, you can rollback:
```bash
# Stop the new container
docker-compose -f docker-compose.prod.yml stop mcp.server

# Use your backup configuration
docker-compose -f docker-compose.yml.backup-YYYYMMDD up -d mcp.server
```

### 📋 Post-Deployment Verification

1. Check service status: `docker-compose -f docker-compose.prod.yml ps`
2. View logs: `docker-compose -f docker-compose.prod.yml logs mcp.server`
3. Test the service: `curl http://mcp.io-aerospace.org`

---

## 🎯 Ready to Deploy!

Everything is prepared. You can now proceed with the deployment on your remote server.
