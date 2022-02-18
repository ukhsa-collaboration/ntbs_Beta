# Rough script for quick build-and-release to a particular env
# Follow the instructions in temporarily-unavailable/README.md to generate a personal access token and add it as an environment variable
echo $env:GITHUB_TOKEN | docker login ghcr.io -u USERNAME --password-stdin
docker build -t ghcr.io/publichealthengland/ntbs-service:temporarily-unavailable ./temporarily-unavailable/
docker push ghcr.io/publichealthengland/ntbs-service:temporarily-unavailable