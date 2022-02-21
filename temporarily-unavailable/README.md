# Temporarily unavailable page app
This is a very simple dotnet app with a single static page to be used as a placeholder on the real urls while ntbs is
undergoing maintenance. It is NTBS-branded to allow future users to confidently test future network access to the app.

## Building the app
The temporarily-unavailable folder contains a Dockerfile for building the coming-soon image. Due to the simplicity of it, no
versioning system is used for the images.

We use the same repository for the image as the main app - see `publishTemporarilyUnavailablePageImage.ps1` script for publishing
the image.

Before running the above script you must authenticate to the github container registry (information taken from https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry)
* Navigate to https://github.com/settings/tokens/new?scopes=write:packages to generate a new personal access token with write:packages enabled.
* In powershell navigate to the ntbs_Beta folder.
* Run `$env:GITHUB_TOKEN='YOUR_TOKEN'`
* Run `scripts\publishTemporarilyUnavailablePageImage.ps1`

## Deploying
The kubernetes definition files for the PHE environment include a set of resources to deploy the app. See comments in
those files for choosing which app (temporarily-unavailable or ntbs-service) are being exposed on which url.