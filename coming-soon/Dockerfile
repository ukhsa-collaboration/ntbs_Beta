FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# restore app (in a separate layer to speed up the build)
COPY ./coming-soon.csproj ./coming-soon.csproj
RUN dotnet restore ./coming-soon.csproj

# copy and build app
COPY ./ ./
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime

# Satisfying Openshift requirements:
# - this tells it that the app is OK to run under random user id
USER 1001
# - we don't have the permissions to run on default 80 port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "coming-soon.dll"]