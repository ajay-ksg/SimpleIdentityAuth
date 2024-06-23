FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimpleAuthenticationAuthorization/SimpleAuthenticationAuthorization.csproj", "SimpleAuthenticationAuthorization/"]
RUN dotnet restore "SimpleAuthenticationAuthorization/SimpleAuthenticationAuthorization.csproj"
COPY . .
WORKDIR "/src/SimpleAuthenticationAuthorization"
RUN dotnet build "SimpleAuthenticationAuthorization.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleAuthenticationAuthorization.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleAuthenticationAuthorization.dll"]
