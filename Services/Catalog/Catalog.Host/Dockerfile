FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Catalog/Catalog.Host/Catalog.Host.csproj", "Services/Catalog/Catalog.Host/"]
RUN dotnet restore "Services/Catalog/Catalog.Host/Catalog.Host.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/Catalog.Host"
RUN dotnet build "Catalog.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Host.dll"]