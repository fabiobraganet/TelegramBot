#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ESB/Services/ESB.Services.Messaging/ESB.Services.Messaging.csproj", "ESB/Services/ESB.Services.Messaging/"]
RUN dotnet restore "ESB/Services/ESB.Services.Messaging/ESB.Services.Messaging.csproj"
COPY . .
WORKDIR "/src/ESB/Services/ESB.Services.Messaging"
RUN dotnet build "ESB.Services.Messaging.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESB.Services.Messaging.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESB.Services.Messaging.dll"]