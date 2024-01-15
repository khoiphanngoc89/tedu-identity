#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tedu.Identity.IDP/Tedu.Identity.IDP.csproj", "Tedu.Identity.IDP/"]
COPY ["Tedu.Identity.Common/Tedu.Identity.Infrastructure.csproj", "Tedu.Identity.Infrastructure/"]
COPY ["Tedu.Identity.Common/Tedu.Identity.Presentation.csproj", "Tedu.Identity.Presentation/"]
RUN dotnet restore "Tedu.Identity.IDP/Tedu.Identity.IDP.csproj"
COPY . .
WORKDIR "/src/Tedu.Identity.IDP"
RUN dotnet build "Tedu.Identity.IDP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tedu.Identity.IDP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tedu.Identity.IDP.dll"]