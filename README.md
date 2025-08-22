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

## Project Structure

```
mcp-server/
├── AI/                           # AI tools and models
│   ├── Tools/                    # Core calculation tools
│   ├── Models/                   # Data models and types
│   └── Converters/              # Type converters
├── Data/                         # Data providers and solar system kernels
│   ├── SolarSystem/             # SPICE kernel files
│   └── SolarSystemObjects/      # Celestial body definitions
├── Server.Sse/                  # HTTP/SSE transport server
├── Server.Stdio/                # STDIO transport server
├── docker-compose.yml           # Development Docker configuration
├── docker-compose.prod.example.yml  # Production template
└── deploy-production.sh         # Production deployment script
```

## Prerequisites

- .NET 9.0 SDK or runtime
- Docker (for containerized deployment)
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

## Quick Start

### Docker Deployment (Recommended)

#### Development
```bash
git clone https://github.com/IO-Aerospace-software-engineering/mcp-server
cd mcp-server
docker-compose up
```

The SSE server will be available at `http://localhost:8080`

#### Production
1. Copy `docker-compose.prod.example.yml` to `docker-compose.prod.yml`
2. Update the domain names in the production file
3. Ensure kernel data exists at `./data/solarsystem/`
4. Deploy using the automated script:

```bash
./deploy-production.sh
```

### Native .NET Deployment

#### 1. Clone and Build
```bash
git clone https://github.com/IO-Aerospace-software-engineering/mcp-server
cd mcp-server
dotnet build
```

#### 2. Solar System Data Setup

The server requires SPICE kernels for solar system calculations. You can configure the path in multiple ways (in order of priority):

1. **Environment Variable** (Recommended):
```bash
# Linux/macOS
export IO_DATA_DIR="/path/to/your/spice/kernels"

# Windows
set IO_DATA_DIR=C:\path\to\your\spice\kernels
```

2. **Configuration File**: Edit `KernelsPath` in `appsettings.json`
3. **Default Location**: Place files in `Data/SolarSystem/` directory

**Required Kernel Files:**
```
kernels/
├── de440s.bsp              # Planetary ephemeris
├── latest_leapseconds.tls  # Leap seconds
├── pck00011.tpc           # Planetary constants
├── earth_latest_high_prec.bpc  # Earth orientation
└── ...                    # Additional kernel files
```

- `IO_DATA_DIR`: Override kernels directory path (takes priority over appsettings.json)

#### 3. Choose Transport Method

##### STDIO Transport (For MCP Clients)
```bash
cd Server.Stdio
dotnet run
```

##### SSE Transport (For Web/HTTP)
```bash
cd Server.Sse
dotnet run
# Server available at http://localhost:8080
```

## Docker Configuration

### Development Environment
- **File**: `docker-compose.yml`
- **Ports**: 8080 (HTTP), 8081 (HTTPS)
- **Data**: Mounted from `./Data/SolarSystem`
- **Usage**: `docker-compose up`

### Production Environment
- **File**: `docker-compose.prod.yml` (create from example)
- **Features**: Traefik reverse proxy, optimized images
- **Data**: Host-mounted from `./data/solarsystem`
- **Deployment**: Automated via `deploy-production.sh`

## MCP Client Integration

### Claude Desktop Configuration
Add to your Claude Desktop configuration:

```json
{
  "mcpServers": {
    "astrodynamics": {
      "command": "/path/to/Server.Stdio",
      "args": [],
      "env": {
        "IO_DATA_DIR": "/path/to/kernels"
      }
    }
  }
}
```

### HTTP/SSE Integration
For web-based integrations, connect to the SSE endpoint:

```javascript
const eventSource = new EventSource('http://your-domain/sse');
```

## Troubleshooting

### Common Issues

1. **"Kernels directory does not exist"**: Verify the kernels path exists and contains SPICE files
2. **"Failed to load kernel"**: Ensure all required kernel files are present and accessible
3. **Connection errors**: Check firewall settings and port availability

### Log Monitoring
```bash
# Development
docker-compose logs -f

# Production
docker logs -f container-name
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For support and questions:
- Create an issue on GitHub
- Check the troubleshooting section above
- Review the deployment guide in [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md)
