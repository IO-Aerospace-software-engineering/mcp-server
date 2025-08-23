# CI/CD

- CI: Builds the `MCP.sln` solution on every push and PR (.github/workflows/ci.yml).
- CD: Publishes single-file executables for `server.Stdio` for three OS targets in one run (.github/workflows/release.yml).

How to trigger deployment:
- Tag-based: push a tag like `v1.0.0`.
- Manual: Actions → "Release single-file (server.Stdio)" → Run workflow (no inputs).

Targets built (one shot):
- linux-x64, win-x64, osx-arm64

Change targets:
- Edit the matrix in `.github/workflows/release.yml` (key `matrix.rid`) to add/remove RIDs.

Result:
- Three artifacts:
  - `io-aerosapce-mcp-<tag or branch>-linux-x64`
  - `io-aerosapce-mcp-<tag or branch>-win-x64`
  - `io-aerosapce-mcp-<tag or branch>-osx-arm64`
- Each contains only the produced executable.

Note on cross-publishing (ubuntu-latest):
- Yes, Windows and macOS binaries are generated on Ubuntu by setting the RID.
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

## Prevent merging PRs if the build fails

Use GitHub Branch protection rules:

1) Trigger the CI at least once so the check appears:
   - Actions → CI → Run workflow (manual), or push a commit/open a PR to the protected branch.
2) Go to: Repository → Settings → Branches → Branch protection rules → New rule.
3) Branch name pattern: main (or your default branch).
4) Enable:
   - Require a pull request before merging
   - Require status checks to pass before merging
5) In “Status checks that are required”:
   - If populated: select “CI / build”.
   - If empty: type “CI / build” and add it manually.
6) (Optional) Also enable:
   - Require branches to be up to date before merging
   - Require approvals
   - Include administrators

After saving, PRs can’t be merged unless the “CI / build” check succeeds.
