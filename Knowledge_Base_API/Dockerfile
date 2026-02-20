FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Knowledge_Base_API/Knowledge_Base_API.csproj", "Knowledge_Base_API/"]
RUN dotnet restore "Knowledge_Base_API/Knowledge_Base_API.csproj"
COPY . .
WORKDIR "/src/Knowledge_Base_API"
RUN dotnet build "./Knowledge_Base_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Knowledge_Base_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Knowledge_Base_API.dll"]
