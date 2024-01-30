#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root 

RUN apt-get update 
RUN apt-get install -y wget
RUN apt-get install -y dnsutils
RUN apt-get install -y mtr
RUN apt-get install -y iputils-ping
RUN groupadd -f tools
RUN usermod -aG tools app

USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.Cards365/Tgs.GameInterface.Cards365.csproj", "Tgs.GameInterface.Cards365/"]
COPY ["Tgs.GameInterface.Cards365.Core/Tgs.GameInterface.Cards365.Core.csproj", "Tgs.GameInterface.Cards365.Core/"]
RUN dotnet restore "./Tgs.GameInterface.Cards365/./Tgs.GameInterface.Cards365.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.Cards365"
RUN dotnet build "./Tgs.GameInterface.Cards365.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameInterface.Cards365.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.Cards365.dll"]