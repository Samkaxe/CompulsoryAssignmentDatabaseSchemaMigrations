FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EntityFrameworkCoreMigrations/EntityFrameworkCoreMigrations.csproj", "EntityFrameworkCoreMigrations/"]
RUN dotnet restore "EntityFrameworkCoreMigrations/EntityFrameworkCoreMigrations.csproj"
COPY . .
WORKDIR "/src/EntityFrameworkCoreMigrations"
RUN dotnet build "EntityFrameworkCoreMigrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EntityFrameworkCoreMigrations.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EntityFrameworkCoreMigrations.dll"]
