FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Order/Order.Host/Order.Host.csproj", "Services/Order/Order.Host/"]
RUN dotnet restore "Services/Order/Order.Host/Order.Host.csproj"
COPY . .
WORKDIR "/src/Services/Order/Order.Host"
RUN dotnet build "Order.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Host.dll"]