FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY WebApi/Formicarius.WebApi.csproj WebApi/Formicarius.WebApi.csproj

RUN dotnet new sln --format slnx --name Formicarius
RUN dotnet sln add WebApi
RUN dotnet restore

COPY . .

RUN dotnet build WebApi/Formicarius.WebApi.csproj --configuration Release --output /app/build
RUN dotnet publish WebApi/Formicarius.WebApi.csproj --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

RUN apt-get update && apt-get install -y curl=7.88.1-10+deb12u14 --no-install-recommends && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

RUN groupadd -g 10000 dotnet && useradd -u 10000 -g dotnet dotnet && chown -R dotnet:dotnet /app
USER dotnet:dotnet

ENV ASPNETCORE_HTTP_PORTS=5080
ENV DOTNET_URLS=
ENV HTTPS_PORTS=
EXPOSE 5080

HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --fail http://localhost:5080/health || exit 1

ENTRYPOINT ["dotnet", "Formicarius.WebApi.dll"]
