# Use the official .NET 9 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files for dependency resolution
COPY ["Server.Sse/Server.Sse.csproj", "Server.Sse/"]
COPY ["AI/AI.csproj", "AI/"]
COPY ["Data/Data.csproj", "Data/"]

# Restore dependencies
RUN dotnet restore "Server.Sse/Server.Sse.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/Server.Sse"
RUN dotnet build "Server.Sse.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "Server.Sse.csproj" -c Release -o /app/publish --no-restore

# Use the official .NET 9 runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Copy the Solar System data from the Data project
COPY --from=build /src/Data/SolarSystem ./SolarSystem

# Expose the default ASP.NET Core port
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "Server.Sse.dll"]