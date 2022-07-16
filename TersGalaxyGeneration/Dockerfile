FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TersGalaxyGeneration/TersGalaxyGeneration.csproj", "TersGalaxyGeneration/"]
RUN dotnet restore "TersGalaxyGeneration/TersGalaxyGeneration.csproj"
COPY . .
WORKDIR "/src/TersGalaxyGeneration"
RUN dotnet build "TersGalaxyGeneration.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TersGalaxyGeneration.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TersGalaxyGeneration.dll"]
