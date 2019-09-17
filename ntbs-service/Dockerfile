FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . ./
RUN dotnet publish -c Release -o out

FROM node AS build-frontend
WORKDIR /app

# copy package.json and restore as distinct layers
COPY package.json .
COPY package-lock.json .
RUN npm install

# copy everything else and build frontend app
COPY . ./
RUN npm run build:prod


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
COPY --from=build-frontend /app/wwwroot/dist ./wwwroot/dist/
ENTRYPOINT ["dotnet", "ntbs-service.dll"]