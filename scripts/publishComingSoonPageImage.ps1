# Rough script for quick build-and-release to a particular env
docker build -t ntbscontainerregistry.azurecr.io/coming-soon ./coming-soon/
docker push ntbscontainerregistry.azurecr.io/coming-soon