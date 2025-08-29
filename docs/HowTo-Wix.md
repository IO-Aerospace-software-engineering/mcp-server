Title: How to Use the IO Aerospace MCP Server

Summary: Quick steps to use the hosted server, connect MCP clients, or self-host with Docker/.NET. Includes SPICE kernels info, tool calls, and troubleshooting.

Section: Use the hosted server (no setup)
- Base URL: https://mcp.io-aerospace.org/
- SSE stream (manual/web): https://mcp.io-aerospace.org/sse
- Most MCP clients that support HTTP/SSE only need the base URL.



Section: Use with MCP clients
- Schemas vary by client; examples below use Claude Desktop.

Claude Desktop (STDIO):
```json
{
  "mcpServers": {
    "astrodynamics": {
      "command": "/path/to/Server.Stdio",
      "args": ["-k", "/path/to/kernels"]
    }
  }
}
```

Claude Desktop (STDIO with env):
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

Claude Desktop (HTTP/SSE to hosted):
```json
{
  "mcpServers": {
    "astrodynamics": {
      "transport": {
        "type": "http",
        "url": "https://mcp.io-aerospace.org"
      }
    }
  }
}
```

Section: Self-host with Docker
Development:
```bash
git clone https://github.com/IO-Aerospace-software-engineering/mcp-server
cd mcp-server
docker-compose up
```
- HTTP server: http://localhost:8080
- Kernels mounted from ./Data/SolarSystem

Production:
1) Copy and customize: `cp docker-compose.prod.example.yml docker-compose.prod.yml`
2) Ensure kernels at ./data/solarsystem/
3) Deploy: `./deploy-production.sh`

Section: Self-host with .NET
Build:
```bash
git clone https://github.com/IO-Aerospace-software-engineering/mcp-server
cd mcp-server
dotnet build
```

Provide SPICE kernels for STDIO:
```bash
# CLI flag (highest priority)
./Server.Stdio -k /path/to/your/spice/kernels

# Environment (Linux/macOS)
export IO_DATA_DIR="/path/to/your/spice/kernels"
./Server.Stdio

# Windows (PowerShell)
$env:IO_DATA_DIR="C:\\path\\to\\your\\spice\\kernels"
./Server.Stdio.exe
```

Choose transport:
- STDIO (recommended): `./Server.Stdio -k /path/to/kernels`
- HTTP/SSE (web): `cd Server.Sse && dotnet run` → http://localhost:8080

Section: Call tools (Node.js MCP SDK)
```ts
import { Client } from "@modelcontextprotocol/sdk/client/index.js";
import { HttpClientTransport } from "@modelcontextprotocol/sdk/client/transport/http.js";

const transport = new HttpClientTransport(new URL("https://mcp.io-aerospace.org"));
const client = new Client(
  { name: "example-client", version: "1.0.0" },
  { capabilities: { tools: {}, prompts: {}, resources: {} } },
  transport
);

await client.connect();
const tools = await client.listTools();
console.log("Tools:", tools);
```

Code (optional quick check in browser/Node):
```js
const eventSource = new EventSource('https://mcp.io-aerospace.org/sse');

eventSource.onmessage = (event) => {
  console.log('message', event.data);
};

eventSource.onerror = (err) => {
  console.error('sse error', err);
};
```

Section: Required kernel files (typical set)
- de440s.bsp (planetary ephemeris)
- latest_leapseconds.tls (leap seconds)
- pck00011.tpc (planetary constants)
- earth_latest_high_prec.bpc (Earth orientation)
- Additional kernels as needed

Section: Troubleshooting
- "Kernels directory does not exist": Check -k or IO_DATA_DIR path.
- "Failed to load kernel": Ensure required files are present and readable.
- HTTP connection errors: Check ports, firewall, proxies.
- Logs: `docker-compose logs -f` (dev) or `docker logs -f <container>` (prod)

Section: Support & links
- Hosted: https://mcp.io-aerospace.org/
- Source: https://github.com/IO-Aerospace-software-engineering/mcp-server
- Astrodynamics: https://github.com/IO-Aerospace-software-engineering/Astrodynamics
- Deployment guide: ./DEPLOYMENT_GUIDE.md
- Sponsor: https://github.com/sponsors/IO-Aerospace-software-engineering

