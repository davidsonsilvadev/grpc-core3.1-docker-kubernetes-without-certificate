FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build  
WORKDIR /src
COPY ["*.sln", "./"]
COPY ["Poc.Grpc.Client/Poc.Grpc.Client.csproj", "Poc.Grpc.Client/"]
RUN dotnet restore "Poc.Grpc.Client/Poc.Grpc.Client.csproj"
COPY . .
WORKDIR "/src/Poc.Grpc.Client"
RUN dotnet build "Poc.Grpc.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Poc.Grpc.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poc.Grpc.Client.dll"]
