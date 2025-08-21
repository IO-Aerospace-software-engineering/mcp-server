# IO Aerospace MCP Server

A Model Context Protocol (MCP) server for aerospace and astrodynamics calculations, providing tools for celestial body ephemeris, orbital mechanics, and space mission analysis.

## Overview

This MCP server provides two transport options:
- **STDIO Transport**: Standard input/output communication (recommended for MCP clients)
- **SSE Transport**: HTTP Server-Sent Events for web-based integrations

The server includes comprehensive tools for:
- Celestial body ephemeris and state vector calculations
- Orbital mechanics and geometry computations
- Deep Space Network (DSN) ground station operations
- Solar system object properties and characteristics
- Mathematical conversions for aerospace calculations
- Time system conversions and utilities

## Prerequisites

- .NET 9.0 SDK or runtime
- Solar system kernels data (SPICE kernels)

## Available Tools

### CelestialBodyTools
- **GetEphemerisAsStateVectors**: Calculate state vectors (position and velocity) of celestial bodies
- **GetCelestialBodyProperties**: Retrieve geophysical properties of planets and moons

### OrbitalParametersTools
- **ConvertStateVectorToKeplerianElements**: Convert state vectors to Keplerian orbital elements
- **ConvertStateVectorToEquinoctialElements**: Convert state vectors to equinoctial elements
- **ConvertStateVectorToEquatorialCoordinates**: Convert state vectors to equatorial coordinates
- **ConvertKeplerianElementsToStateVector**: Convert Keplerian elements back to state vectors
- **ConvertEquinoctialElementsToStateVector**: Convert equinoctial elements back to state vectors
- **ConvertStateVectorToTheGivenFrame**: Transform state vectors between reference frames

### GeometryFinderTools
- **FindCoordinateConstraint**: Find time windows when coordinate constraints are met
- **FindDistanceConstraint**: Find time windows when distance constraints are satisfied
- **FindOccultingConstraint**: Find occultation and eclipse events

### SiteTools
- **GetDeepSpaceStationPlanetodeticCoordinates**: Get latitude, longitude, and altitude of DSS stations
- **GetDeepSpaceStationStateVector**: Calculate state vectors for ground stations
- **GetHorizontalCoordinates**: Get azimuth and elevation from ground stations
- **GetDSSFrame**: Retrieve reference frame information for DSS stations

### TimeTools
- **ConvertDateTime**: Convert between different time systems (UTC, TDB, TAI, TDT, GPS)
- **CurrentDateTime**: Get current UTC date and time

### MathTools
- **DegreesToRadians** / **RadiansToDegrees**: Angular unit conversions
- **ConvertDegreesToHours** / **ConvertHoursToDegrees**: Time-angle conversions
- **DegreesToArcseconds** / **ArcsecondsToDegrees**: Angular precision conversions
- **RadiansToArcseconds** / **ArcsecondsToRadians**: Angular unit conversions
- **MetersToMiles** / **MilesToMeters**: Distance unit conversions
- **MetersToFeet** / **FeetToMeters**: Distance unit conversions
- **MetersToKilometers** / **KilometersToMeters**: Metric distance conversions
- **MetersToAstronomicalUnits** / **AstronomicalUnitsToMeters**: Astronomical distance conversions
- **MetersToParsec** / **ParsecToMeters**: Stellar distance conversions
- **MetersToLightYears** / **LightYearsToMeters**: Cosmic distance conversions

## Setup Instructions

### 1. Clone and Build

```bash
git clone https://github.com/IO-Aerospace-software-engineering/mcp-server
cd mcp-server
dotnet build
```

### 2. Solar System Data Setup

The server requires SPICE kernels for solar system calculations. You can configure the kernels location in two ways:

#### Option A: Environment Variable (Recommended)
Set the `IO_DATA_DIR` environment variable to point to your kernels directory:

```bash
# Linux/macOS
export IO_DATA_DIR="/path/to/your/spice/kernels"

# Windows
set IO_DATA_DIR=C:\path\to\your\spice\kernels
```

#### Option B: Configuration File
Place your kernel files in a `SolarSystem` directory relative to the executable, or edit the `KernelsPath` in `appsettings.json`:

```
mcp-server/
??? Data/SolarSystem/          # Default location
?   ??? de440.bsp             # Planetary ephemeris
?   ??? pck00011.tpc          # Planetary constants
?   ??? ...                   # Other kernel files
```

**Note**: Environment variable `IO_DATA_DIR` takes priority over the configuration file setting.

## Usage Options

### Option 1: STDIO Transport (Recommended)

The STDIO transport is ideal for MCP clients and command-line usage.

#### Development Mode
```bash
cd Server.Stdio
dotnet run
```

#### Production Mode
```bash
cd Server.Stdio
dotnet run --configuration Release
```

#### Published Executable
```bash
# Publish the application
dotnet publish Server.Stdio/Server.Stdio.csproj -c Release -o ./publish-stdio

# Run the published executable
./publish-stdio/Server.Stdio
```

#### Configuration
Edit `Server.Stdio/appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "KernelsPath": "SolarSystem"
}
```

### Option 2: SSE Transport (Web/HTTP)

The SSE transport provides HTTP endpoints for web-based integrations.

#### Development Mode
```bash
cd Server.Sse
dotnet run
```

#### Production Mode
```bash
cd Server.Sse
dotnet run --configuration Release
```

#### URLs
- Development: `http://localhost:5000` and `https://localhost:5001`
- Production: `http://localhost:8080`

#### Configuration
Edit `Server.Sse/appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "KernelsPath": "SolarSystem"
}
```

### Option 3: Docker (SSE Transport)

Use the provided Dockerfile to run the SSE server in a container.

#### Build and Run
```bash
# Build the Docker image
docker build -t io-aerospace-mcp-server .

# Run the container
docker run -p 8080:8080 -p 8081:8081 io-aerospace-mcp-server
```

#### Access
- Server will be available at `http://localhost:8080`
- SSL available at `http://localhost:8081`

## MCP Client Integration

### Universal MCP Configuration

Here's a comprehensive MCP configuration supporting both local and remote server deployments:

```json
{
  "servers": {
    "io-astrodynamics": {
      "type": "stdio",
      "command": "/opt/io/io-mcp-stdio",
      "args": [],
      "env": {
        "IO_DATA_DIR": "/opt/io/data",
        "LOG_LEVEL": "info"
      }
    },
    "io-astrodynamics-remote": {
      "type": "http",
      "url": "https://mcp.io-aerospace.org/"
    },
    "io-astrodynamics-local-http": {
      "type": "http", 
      "url": "http://localhost:8080/mcp"
    }
  },
  "inputs": []
}
```

### Claude Desktop Configuration

#### Local STDIO Server with Custom Data Path
```json
{
  "mcpServers": {
    "io-aerospace": {
      "command": "C:\\path\\to\\your\\Server.Stdio.exe",
      "args": [],
      "env": {
        "IO_DATA_DIR": "C:\\path\\to\\your\\spice\\kernels",
        "LOG_LEVEL": "Information"
      }
    }
  }
}
```

#### Local HTTP Server with Environment Override
```json
{
  "mcpServers": {
    "io-aerospace-http": {
      "command": "C:\\path\\to\\your\\Server.Sse.exe",
      "args": [],
      "env": {
        "ASPNETCORE_URLS": "http://localhost:8080",
        "IO_DATA_DIR": "C:\\path\\to\\your\\spice\\kernels",
        "LOG_LEVEL": "Information"
      }
    }
  }
}
```

#### Remote HTTP Server
```json
{
  "mcpServers": {
    "io-aerospace-remote": {
      "type": "sse",
      "url": "https://your-domain.com/mcp/sse"
    }
  }
}
```

### Self-Contained Deployment for MCP Clients

Create a self-contained executable for easy distribution:

```bash
# Windows x64
dotnet publish Server.Stdio/Server.Studio.csproj -c Release -r win-x64 --self-contained -o ./dist/win-x64

# Linux x64
dotnet publish Server.Studio/Server.Studio.csproj -c Release -r linux-x64 --self-contained -o ./dist/linux-x64

# macOS x64
dotnet publish Server.Studio/Server.Studio.csproj -c Release -r osx-x64 --self-contained -o ./dist/osx-x64

# macOS ARM64
dotnet publish Server.Studio/Server.Studio.csproj -c Release -r osx-arm64 --self-contained -o ./dist/osx-arm64
```

### Custom MCP Client Integration

#### STDIO Transport
```python
import subprocess
import json

# Start the MCP server process
process = subprocess.Popen(
    ['./Server.Stdio.exe'],
    stdin=subprocess.PIPE,
    stdout=subprocess.PIPE,
    stderr=subprocess.PIPE,
    text=True
)

# Send MCP messages via stdin
message = {
    "jsonrpc": "2.0",
    "id": 1,
    "method": "tools/list",
    "params": {}
}
process.stdin.write(json.dumps(message) + '\n')
process.stdin.flush()
```

#### SSE Transport
```javascript
// Connect via Server-Sent Events
const eventSource = new EventSource('http://localhost:8080/sse');

eventSource.onmessage = function(event) {
    const data = JSON.parse(event.data);
    console.log('Received:', data);
};

// Send HTTP requests for tool calls
async function callTool(toolName, parameters) {
    const response = await fetch('http://localhost:8080/mcp/tools/call', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            name: toolName,
            arguments: parameters
        })
    });
    return await response.json();
}
```

## Configuration Options

### Kernels Path Configuration
The server requires SPICE kernels for solar system calculations. You can configure the path in multiple ways (in order of priority):

1. **Environment Variable** (highest priority): `IO_DATA_DIR`
2. **Configuration File**: `KernelsPath` in `appsettings.json`

#### Examples:
```bash
# Using environment variable
export IO_DATA_DIR="/opt/io/data"
./Server.Stdio

# Using configuration file
# Edit appsettings.json: "KernelsPath": "SolarSystem"
```

### Logging
Both servers support comprehensive logging with Serilog:
- Console output
- File logging (STDIO version writes to `logs/` directory)
- Configurable log levels via `LOG_LEVEL` environment variable or configuration

### Environment Variables
- `IO_DATA_DIR`: Override kernels directory path (takes priority over appsettings.json)
- `LOG_LEVEL`: Set logging level (Trace, Debug, Information, Warning, Error, Fatal)
- `ASPNETCORE_URLS`: Server URLs for SSE transport (default: `http://+:8080`)
- `ASPNETCORE_ENVIRONMENT`: Environment (Development/Production)

## Example Usage

Once connected via MCP client, you can use tools like:

### Calculate Mars Position from Earth
```json
{
  "tool": "GetEphemerisAsStateVectors",
  "parameters": {
    "observerName": "EARTH",
    "targetName": "MARS",
    "frame": "ICRF",
    "startTime": "2024-01-01T00:00:00",
    "endTime": "2024-01-02T00:00:00",
    "timeStep": 3600,
    "aberrationCorrection": "LT"
  }
}
```

### Convert Orbital Elements
```json
{
  "tool": "ConvertStateVectorToKeplerianElements",
  "parameters": {
    "stateVector": {
      "centerOfMotion": "EARTH",
      "epoch": {"dateTime": "2024-01-01T00:00:00", "kind": "UTC"},
      "frame": "ICRF",
      "position": {"x": 7000000, "y": 0, "z": 0},
      "velocity": {"x": 0, "y": 7500, "z": 0}
    }
  }
}
```

### Unit Conversions
```json
{
  "tool": "DegreesToRadians",
  "parameters": {
    "degrees": 45.0
  }
}
```

## Troubleshooting

### Common Issues

1. **"KernelsPath is not set"**: Ensure the `KernelsPath` is configured in `appsettings.json`
2. **"Kernels directory does not exist"**: Verify the kernels path exists and contains SPICE files
3. **Port conflicts (SSE)**: Change ports in `appsettings.json` or via environment variables
4. **MCP client connection issues**: Verify the executable path and permissions
5. **Remote server connectivity**: Check network connectivity and URL accessibility

### Logs Location
- STDIO: `logs/IO-MCP-Server_YYYYMMDD.log`
- SSE: Console output and configured logging providers

### Debugging MCP Integration
- Enable verbose logging in `appsettings.json`
- Check MCP client logs for connection errors
- Verify tool schemas match expected formats
- Test with simple tools first (e.g., `CurrentDateTime`)

## Development

### Project Structure
- `AI/`: Core astrodynamics tools and MCP integration
- `Data/`: Solar system data and utilities
- `Server.Stdio/`: STDIO transport implementation
- `Server.Sse/`: HTTP/SSE transport implementation

### Building from Source
```bash
# Restore dependencies
dotnet restore

# Build all projects
dotnet build

# Run tests (if available)
dotnet test
```

### Adding New Tools
1. Create a new class in `AI/Tools/` with `[McpServerToolType]` attribute
2. Add methods with `[McpServerTool]` and `[Description]` attributes
3. Register in Program.cs with `WithToolsFromAssembly(typeof(YourToolClass).Assembly)`

## Deployment

### Production Deployment (Self-Contained)
```bash
# Create production build
dotnet publish Server.Stdio/Server.Stdio.csproj \
  -c Release \
  -r linux-x64 \
  --self-contained \
  -p:PublishSingleFile=true \
  -o ./deploy

# Copy kernels data
cp -r Data/SolarSystem ./deploy/
```

### Systemd Service (Linux)
```ini
[Unit]
Description=IO Aerospace MCP Server
After=network.target

[Service]
Type=exec
ExecStart=/opt/io-aerospace/Server.Stdio
WorkingDirectory=/opt/io-aerospace
User=io-aerospace
Environment=ASPNETCORE_ENVIRONMENT=Production
Restart=always

[Install]
WantedBy=multi-user.target
```

## Support

For issues and questions:
- GitHub Issues: [IO-Aerospace-software-engineering/mcp-server](https://github.com/IO-Aerospace-software-engineering/mcp-server)
- Ensure you have the latest .NET 9 runtime/SDK installed
- Verify your SPICE kernels are properly configured
