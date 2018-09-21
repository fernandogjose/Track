
FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY src/Front/Track.Webapi/Track.Webapi.csproj src/Front/Track.Webapi/Track.Webapi/
RUN dotnet restore src/Front/Track.Webapi/Track.Webapi/Track.Webapi.csproj
WORKDIR /src/Front/Track.Webapi/Track.Webapi
COPY . .
RUN dotnet build src/Front/Track.Webapi/Track.Webapi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish src/Front/Track.Webapi/Track.Webapi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "src/Front/Track.Webapi/Track.Webapi.dll"]
