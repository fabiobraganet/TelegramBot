#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ESB/Workers/ESB.Workers.Bots/ESB.Workers.Bots.csproj", "ESB/Workers/ESB.Workers.Bots/"]
COPY ["ESB/Libraries/ESB.Data/ESB.Data.csproj", "ESB/Libraries/ESB.Data/"]
COPY ["ESB/Libraries/ESB.Domain/ESB.Domain.csproj", "ESB/Libraries/ESB.Domain/"]
RUN dotnet restore "ESB/Workers/ESB.Workers.Bots/ESB.Workers.Bots.csproj"
COPY . .
WORKDIR "/src/ESB/Workers/ESB.Workers.Bots"
RUN dotnet build "ESB.Workers.Bots.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESB.Workers.Bots.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESB.Workers.Bots.dll"]