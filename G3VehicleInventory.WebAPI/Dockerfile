# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["G3VehicleInventory.WebAPI/G3VehicleInventory.WebAPI.csproj", "G3VehicleInventory.WebAPI/"]
COPY ["G3SharedKernel/G3SharedKernel.csproj", "G3SharedKernel/"]
COPY ["G3VehicleInventory.Application/G3VehicleInventory.Application.csproj", "G3VehicleInventory.Application/"]
COPY ["G3VehicleInventory.Domain/G3VehicleInventory.Domain.csproj", "G3VehicleInventory.Domain/"]
COPY ["G3VehicleInventory.Infrastructure/G3VehicleInventory.Infrastructure.csproj", "G3VehicleInventory.Infrastructure/"]
RUN dotnet restore "./G3VehicleInventory.WebAPI/G3VehicleInventory.WebAPI.csproj"
COPY . .
WORKDIR "/src/G3VehicleInventory.WebAPI"
RUN dotnet build "./G3VehicleInventory.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./G3VehicleInventory.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "G3VehicleInventory.WebAPI.dll"]