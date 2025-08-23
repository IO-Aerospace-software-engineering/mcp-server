# CI/CD

- CI: Builds the repository on every push and PR (.github/workflows/ci.yml).
- CD: Publishes a single-file executable for `server.Stdio` and uploads only that file as an artifact (.github/workflows/release.yml).

How to trigger deployment:
- Tag-based: push a tag like `v1.0.0`.
- Manual: Actions → "Release single-file (server.Stdio)" → Run workflow and choose a RID (default `linux-x64`).

Change target runtime:
- Provide a different RID when running the workflow (examples: `linux-x64`, `win-x64`, `osx-arm64`).

Result:
- Artifact name: `io-aerosapce-mcp-<tag or branch>-<RID>`
- Contains only the produced executable.

Note on cross-publishing (ubuntu-latest):
- Yes, you can generate Windows (.exe) or macOS binaries from Ubuntu by setting RID to `win-x64` or `osx-arm64`.
- macOS outputs will be unsigned; signing/notarization must happen on macOS if required.

Local single-file publish examples:
```bash
# Linux
dotnet publish ./Server.Stdio/Server.Stdio.csproj -c Release -r linux-x64 -p:PublishSingleFile=true -p:SelfContained=true
# Windows
dotnet publish ./Server.Stdio/Server.Stdio.csproj -c Release -r win-x64   -p:PublishSingleFile=true -p:SelfContained=true
# macOS (unsigned)
dotnet publish ./Server.Stdio/Server.Stdio.csproj -c Release -r osx-arm64 -p:PublishSingleFile=true -p:SelfContained=true
```
