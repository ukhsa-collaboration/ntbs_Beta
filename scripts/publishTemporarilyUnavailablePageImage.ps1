# Rough script for quick build-and-release to a particular env
docker build -t ghcr.io/publichealthengland/ntbs-service:temporarily-unavailable ./temporarily-unavailable/
docker push ghcr.io/publichealthengland/ntbs-service:temporarily-unavailable