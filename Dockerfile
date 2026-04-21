# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything (respects .dockerignore)
COPY . .

RUN dotnet restore BlockApp.Api/BlockApp.Api.csproj
RUN dotnet publish BlockApp.Api/BlockApp.Api.csproj -c Release -o /app/publish

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
RUN mkdir -p /app/data
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BlockApp.Api.dll"]
