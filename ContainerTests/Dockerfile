FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ContainerTests/ContainerTests.csproj", "ContainerTests/"]
RUN dotnet restore "ContainerTests/ContainerTests.csproj"
COPY . .
WORKDIR "/src/ContainerTests"
RUN dotnet build "ContainerTests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContainerTests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "ContainerTests.dll", "--verbosity:diagnostic", "--logger:console;verbosity=detailed"]
