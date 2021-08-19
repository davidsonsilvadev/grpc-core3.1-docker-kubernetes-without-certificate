FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build  
WORKDIR /src
COPY ["*.sln", "./"]
COPY ["Poc.Grcp/Poc.Grcp.csproj", "Poc.Grcp/"]
RUN dotnet restore "Poc.Grcp/Poc.Grcp.csproj"
COPY . .
WORKDIR "/src/Poc.Grcp"
RUN dotnet build "Poc.Grcp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Poc.Grcp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poc.Grcp.dll"]