#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StellarStock.Presentation/StellarStock.Presentation.csproj", "StellarStock.Presentation/"]
COPY ["StellarStock.Application/StellarStock.Application.csproj", "StellarStock.Application/"]
COPY ["StellarStock.Domain/StellarStock.Domain.csproj", "StellarStock.Domain/"]
COPY ["StellarStock.Infrastructure/StellarStock.Infrastructure.csproj", "StellarStock.Infrastructure/"]
RUN dotnet restore "StellarStock.Presentation/StellarStock.Presentation.csproj"
COPY . .
WORKDIR "/src/StellarStock.Presentation"
RUN dotnet build "StellarStock.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StellarStock.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StellarStock.Presentation.dll"]