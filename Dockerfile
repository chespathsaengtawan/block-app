# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files first (layer cache for restore)
COPY BlockApp.Shared/BlockApp.Shared.csproj BlockApp.Shared/
COPY BlockApp.Api/BlockApp.Api.csproj BlockApp.Api/
RUN dotnet restore BlockApp.Api/BlockApp.Api.csproj

# Copy full source and publish
COPY BlockApp.Shared/ BlockApp.Shared/
COPY BlockApp.Api/ BlockApp.Api/
WORKDIR /src/BlockApp.Api
RUN dotnet publish -c Release -o /app/publish

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
RUN mkdir -p /app/data
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BlockApp.Api.dll"]
