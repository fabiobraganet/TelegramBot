#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TelegramBot.WebHook/TelegramBot.WebHook.csproj", "TelegramBot.WebHook/"]
COPY ["Libraries/TelegramBot.Application/TelegramBot.Application.csproj", "Libraries/TelegramBot.Application/"]
COPY ["Libraries/TelegramBot.CrossCutting/TelegramBot.CrossCutting.csproj", "Libraries/TelegramBot.CrossCutting/"]
COPY ["ESB/Libraries/ESB.Domain/ESB.Domain.csproj", "ESB/Libraries/ESB.Domain/"]
RUN dotnet restore "TelegramBot.WebHook/TelegramBot.WebHook.csproj"
COPY . .
WORKDIR "/src/TelegramBot.WebHook"
RUN dotnet build "TelegramBot.WebHook.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelegramBot.WebHook.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramBot.WebHook.dll"]