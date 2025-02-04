#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EduQuest_API/EduQuest_API.csproj", "EduQuest_API/"]
COPY ["EduQuest_Application/EduQuest_Application.csproj", "EduQuest_Application/"]
COPY ["EduQuest_Domain/EduQuest_Domain.csproj", "EduQuest_Domain/"]
COPY ["EduQuest_Infrastructure/EduQuest_Infrastructure.csproj", "EduQuest_Infrastructure/"]
RUN dotnet restore "./EduQuest_API/EduQuest_API.csproj"
COPY . .
WORKDIR "/src/EduQuest_API"
RUN dotnet build "./EduQuest_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EduQuest_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EduQuest_API.dll"]