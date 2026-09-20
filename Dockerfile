# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY KampusRota.sln ./
COPY src/KampusRota.Domain/KampusRota.Domain.csproj src/KampusRota.Domain/
COPY src/KampusRota.Application/KampusRota.Application.csproj src/KampusRota.Application/
COPY src/KampusRota.Infrastructure/KampusRota.Infrastructure.csproj src/KampusRota.Infrastructure/
COPY src/KampusRota.API/KampusRota.API.csproj src/KampusRota.API/
COPY tests/KampusRota.UnitTests/KampusRota.UnitTests.csproj tests/KampusRota.UnitTests/

RUN dotnet restore

# Copy remaining source code and publish
COPY . ./
RUN dotnet publish src/KampusRota.API/KampusRota.API.csproj -c Release -o /out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "KampusRota.API.dll"]
