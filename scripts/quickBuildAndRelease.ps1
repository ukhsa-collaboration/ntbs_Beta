# Rough script for quick build-and-release to a particular env
$env=$args[0]
$buildNumber="$env-$($args[1])"

docker build --build-arg RELEASE=$buildNumber -t ntbscontainerregistry.azurecr.io/ntbs-service:$buildNumber .
docker push ntbscontainerregistry.azurecr.io/ntbs-service:$buildNumber
wsl scripts/release.sh "$env" "$buildNumber"