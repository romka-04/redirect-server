#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Romka04.RedirectServer/Romka04.RedirectServer.csproj", "Romka04.RedirectServer/"]
RUN dotnet restore "Romka04.RedirectServer/Romka04.RedirectServer.csproj"
COPY . .
WORKDIR "/src/Romka04.RedirectServer"
RUN dotnet build "Romka04.RedirectServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Romka04.RedirectServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Romka04.RedirectServer.dll"]