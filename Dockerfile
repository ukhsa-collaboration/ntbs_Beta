FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy and build app
COPY ntbs-service/ .
COPY EFAuditer/ .
RUN dotnet restore ntbs-service/*.csproj
RUN dotnet publish ntbs-service/*.csproj -c Release -o out

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