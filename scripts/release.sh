#!/bin/bash
set -e

env="$1"
build="$2"

echo "Deploying to int"

echo "Applying resource definion $env.yml"
kubectl apply -f ./ntbs-service/deployments/$env.yml

echo "Setting image to build $build"
# This sets the image to current build. Should be the same as "latest", but triggers pull of the image.
kubectl set image deployment/ntbs-$env ntbs-$env=ntbscontainerregistry.azurecr.io/ntbs-service:$build

notReady=1
attempts=0

while [[ $notReady -ne 0 ]]; do
  ((attempts=attempts+1))
  sleep .5
  notReady=$(kubectl get deployment ntbs-int -o jsonpath="{.status.unavailableReplicas}")
  echo "Attempt $attempts - unavailable replicas found: ${notReady}"
  
  if [[ $attempts -eq 20 ]]; then break; fi
done

if [[ $notReady -ne 0 ]]; then
  exit 1;
fi

echo "Deployment done"