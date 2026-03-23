# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY BoardGameShelf/BoardGameShelf.csproj BoardGameShelf/
RUN dotnet restore BoardGameShelf/BoardGameShelf.csproj

COPY . .
RUN dotnet publish BoardGameShelf/BoardGameShelf.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BoardGameShelf.dll"]
