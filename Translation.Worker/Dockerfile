#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Translation.Worker/Translation.Worker.csproj", "Translation.Worker/"]
COPY ["Translation.Domain/Translation.Domain.csproj", "Translation.Domain/"]
COPY ["Translation.Service/Translation.Service.csproj", "Translation.Service/"]
RUN dotnet restore "Translation.Worker/Translation.Worker.csproj"
COPY . .
WORKDIR "/src/Translation.Worker"
RUN dotnet build "Translation.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Translation.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Translation.Worker.dll"]