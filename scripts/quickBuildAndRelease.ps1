# Rough script for quick build-and-release to a particular env
$env=$args[0]
$buildNumber="live$($args[1])"

docker build -t ntbscontainerregistry.azurecr.io/ntbs-service:$buildNumber .
docker push ntbscontainerregistry.azurecr.io/ntbs-service:$buildNumber
wsl scripts/release.sh live "$buildNumber"