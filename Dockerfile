FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY host/AppText.Host/*.csproj ./host/AppText.Host
COPY src/AppText/*.csproj ./src/AppText
COPY src/AppText.Translations/*.csproj ./src/AppText.Translations
COPY src/AppText.Storage.LiteDb/*.csproj ./src/AppText.Storage.LiteDb
RUN dotnet restore ./host/AppText.Host

# Copy everything else and build
COPY . ./
RUN dotnet publish ./host/AppText.Host -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AppText.Host.dll"]
VOLUME ["/DATA"]
ENV DataFolder=/DATA