#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Bdriano.App/Bdriano.App.csproj", "Bdriano.App/"]
COPY ["Bdriano.Persistence/Bdriano.Persistence.csproj", "Bdriano.Persistence/"]
COPY ["Bdriano/Bdriano.csproj", "Bdriano/"]
RUN dotnet restore "Bdriano.App/Bdriano.App.csproj"
COPY . .
WORKDIR "/src/Bdriano.App"
RUN dotnet build "Bdriano.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bdriano.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bdriano.App.dll"]