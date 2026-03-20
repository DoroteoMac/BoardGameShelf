# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BoardGameShelf is an ASP.NET Core Web API project targeting .NET 10.0. Currently at the initial scaffolding stage (default weather forecast template), intended to become a board game shelf/collection management API.

## Commands

```bash
# Run the API (http on port 5147)
dotnet run --project BoardGameShelf/BoardGameShelf.csproj

# Run with https profile
dotnet run --project BoardGameShelf/BoardGameShelf.csproj --launch-profile https

# Build
dotnet build

# Run tests (once test projects are added)
dotnet test
```

OpenAPI docs are available at `/openapi/v1.json` when running in Development mode.

## Architecture

- **Single project** solution: `BoardGameShelf/BoardGameShelf.csproj`
- **Target framework**: net10.0
- **Style**: Minimal API (top-level statements in `Program.cs`, no controllers)
- **Nullable reference types** and **implicit usings** are enabled
- Only dependency: `Microsoft.AspNetCore.OpenApi`

New endpoints should follow the minimal API pattern already established in `Program.cs` using `app.MapGet/MapPost/etc`.
