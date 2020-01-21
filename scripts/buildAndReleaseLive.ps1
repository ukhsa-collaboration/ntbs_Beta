# Rough script for build-and-releases to particular env - here, live
$buildNumber="live$($args[0])"

docker build -t ntbscontainerregistry.azurecr.io/ntbs-service:$buildNumber .
docker push ntbscontainerregistry.azurecr.io/ntbs-service:$buildNumber
wsl scripts/release.sh live "$buildNumber"