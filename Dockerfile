FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Install nodejs
RUN curl -sL https://deb.nodesource.com/setup_12.x |  bash -
RUN apt-get install -y nodejs

# Copy csproj and restore as distinct layers
COPY host/AppText.Host/*.csproj ./host/AppText.Host/
COPY src/AppText/*.csproj ./src/AppText/
COPY src/AppText.Translations/*.csproj ./src/AppText.Translations/
COPY src/AppText.Storage.LiteDb/*.csproj ./src/AppText.Storage.LiteDb/
RUN dotnet restore ./host/AppText.Host/

#Copy package.json and install Javascript dependencies
COPY ["src/AppText.AdminApp/ClientApp/package.json", "./src/AppText.AdminApp/ClientApp/"]
COPY ["src/AppText.AdminApp/ClientApp/package-lock.json", "./src/AppText.AdminApp/ClientApp/"]
WORKDIR /app/src/AppText.AdminApp/ClientApp
RUN npm install

# Copy everything else and build
COPY . /app/

# Build Javascript AppText.AdminApp before the .NET build
WORKDIR /app/src/AppText.AdminApp/ClientApp
RUN npm run prod
WORKDIR /app
RUN dotnet publish ./host/AppText.Host -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AppText.Host.dll"]
VOLUME ["/DATA"]
ENV DataFolder=/DATA