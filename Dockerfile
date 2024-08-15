FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_MONGO_ENV="mongodb"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CodeAPI/CodeAPI.csproj", "CodeAPI/"]
RUN dotnet restore "CodeAPI/CodeAPI.csproj"
COPY . .
WORKDIR "/src/CodeAPI"
RUN dotnet build "CodeAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS development
WORKDIR /src
COPY ["CodeAPI/CodeAPI.csproj", "CodeAPI/"]
WORKDIR "/src/CodeAPI"
CMD dotnet run --no-launch-profile

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CodeAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeAPI.dll"]
