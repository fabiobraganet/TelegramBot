#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TelegramBot.WebHook/TelegramBot.WebHook.csproj", "TelegramBot.WebHook/"]
COPY ["Libraries/TelegramBot.CrossCutting/TelegramBot.CrossCutting.csproj", "Libraries/TelegramBot.CrossCutting/"]
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