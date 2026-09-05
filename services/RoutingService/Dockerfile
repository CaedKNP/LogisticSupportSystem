# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY RoutingService.csproj .
RUN dotnet restore

COPY . .

RUN dotnet publish RoutingService.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8090

ENTRYPOINT ["dotnet", "RoutingService.dll"]