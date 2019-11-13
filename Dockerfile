FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# restore audit lib (in a separate layer to speed up the build)
COPY EFAuditer/EFAuditer.csproj ./EFAuditer/
RUN dotnet restore EFAuditer/EFAuditer.csproj

# restore app (in a separate layer to speed up the build)
COPY ntbs-service/ntbs-service.csproj ./ntbs-service/
RUN dotnet restore ntbs-service/ntbs-service.csproj

# copy and build app
COPY ntbs-service/ ./ntbs-service/
COPY EFAuditer/ ./EFAuditer/
RUN dotnet publish ntbs-service/*.csproj -c Release -o out

FROM node AS build-frontend
WORKDIR /app

# copy package.json and restore as distinct layers
COPY ntbs-service/package.json .
COPY ntbs-service/package-lock.json .
RUN npm install

# copy everything else and build frontend app
COPY ./ntbs-service/wwwroot ./wwwroot
COPY ./ntbs-service/tsconfig.json ./
COPY ./ntbs-service/webpack* ./
RUN npm run build:prod


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/ntbs-service/out ./
COPY --from=build-frontend /app/wwwroot/dist ./wwwroot/dist/
ENTRYPOINT ["dotnet", "ntbs-service.dll"]