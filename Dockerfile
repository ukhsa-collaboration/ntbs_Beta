FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# restore audit lib (in a separate layer to speed up the build)
COPY EFAuditer/EFAuditer.csproj ./EFAuditer/
RUN dotnet restore EFAuditer/EFAuditer.csproj

# restore app (in a separate layer to speed up the build)
COPY ./NuGet.Config ./NuGet.Config
COPY ./frontend-dotnetcore/dist ./frontend-dotnetcore/dist
COPY ntbs-service/ntbs-service.csproj ./ntbs-service/
RUN dotnet restore ntbs-service/ntbs-service.csproj

# copy and build app

COPY ntbs-service/ ./ntbs-service/
COPY ./EFAuditer ./EFAuditer
RUN dotnet publish ntbs-service/*.csproj -c Release -o out

FROM node AS build-frontend
WORKDIR /app

# copy package.json and restore as distinct layers
COPY ntbs-service/package.json .
COPY ntbs-service/package-lock.json .
RUN npm install

# Finish the install of @sentry/cli
# This downloads a binary needed by this package at webpack build
# time. This should be run during the install, but it is not for some
# unknown reason so we run it here.
RUN node node_modules/@sentry/cli/scripts/install.js

# copy everything else and build frontend app
COPY ./ntbs-service/wwwroot ./wwwroot
COPY ./ntbs-service/tsconfig.json ./
COPY ./ntbs-service/webpack* ./

ENV SENTRY_ORG=phe-ntbs
ENV SENTRY_PROJECT=ntbs-frontend
ARG SENTRY_AUTH_TOKEN
ARG RELEASE

RUN npm run build:prod


FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

RUN apt-get update \
    && apt-get install -y krb5-user \
    && apt-get install -y procps

# Satisfying Openshift requirements:
# - this tells it that the app is OK to run under random user id
USER 1001
# - we don't have the permissions to run on default 80 port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

WORKDIR /app
COPY --from=build /app/ntbs-service/out ./
COPY --from=build-frontend /app/wwwroot/dist ./wwwroot/dist/

ARG RELEASE
# We want it to be avaialble inside the container
ENV RELEASE=$RELEASE 

ENTRYPOINT ["dotnet", "ntbs-service.dll"]
