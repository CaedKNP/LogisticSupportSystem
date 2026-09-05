FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY RoutingService/RoutingService.csproj RoutingService/

RUN dotnet restore RoutingService/RoutingService.csproj

COPY RoutingService/ RoutingService/

RUN dotnet publish RoutingService/RoutingService.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8090

EXPOSE 8090

ENTRYPOINT ["dotnet", "RoutingService.dll"]