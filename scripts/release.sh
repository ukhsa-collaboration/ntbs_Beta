#!/bin/bash
set -e

envFile="$1"
build="$2"

echo "Deploying to $envFile"

echo "Applying resource definition $envFile.yml"
kubectl apply -f ./ntbs-service/deployments/$envFile.yml --record=true

# Temporary logic - whilst we have 2 envs that we refer to as `uat`
if [[ $envFile == "uat-phe" ]]
  then
    echo "Overriding env value to uat"
    env="uat"
  else
    env=$envFile
fi

echo "Setting image to build $build"
# This sets the image to current build. Should be the same as "latest", but triggers pull of the image.
kubectl set image deployment/ntbs-$env ntbs-$env=ghcr.io/publichealthengland/ntbs-service:$build --record=true

attempts=0
maxAttempts=30

while [[ $attempts -lt $maxAttempts ]]; do
  ((attempts=attempts+1))
  sleep 1
  unavailableReplicas=$(kubectl get deployment ntbs-$env -o jsonpath="{.status.unavailableReplicas}")
  echo "Attempt $attempts/$maxAttempts - unavailable replicas found: ${unavailableReplicas}"
  
  if [[ -z $unavailableReplicas ]] || [[ $unavailableReplicas -eq 0 ]]; then
    echo "Deployment done"
    exit 0
  fi
done

# Max number of attempts exceeded
echo "!!!WARNING: The deployment has not completed in $maxAttempts seconds. Double check the deployment status!"
exit 1
