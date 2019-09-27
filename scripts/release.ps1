Param([string]$env, [string]$build)

kubectl apply -f .\ntbs-service\$env.yml
kubectl set image deployment/ntbs-$env ntbs-$env=ntbscontainerregistry.azurecr.io/ntbs-service:$build