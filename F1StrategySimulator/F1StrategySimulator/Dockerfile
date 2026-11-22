# Этап 1: Сборка проекта с помощью .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["F1StrategySimulator.csproj", "./"]
RUN dotnet restore "F1StrategySimulator.csproj"

COPY . .
RUN dotnet publish "F1StrategySimulator.csproj" -c Release -o /app/publish

# Этап 2: Финальный лёгкий образ только для запуска
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "F1StrategySimulator.dll"]