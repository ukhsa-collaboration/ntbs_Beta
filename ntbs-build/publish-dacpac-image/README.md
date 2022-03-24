This Dockerfile is designed to build an image called `publish-dacpac`.

The `publish-dacpac` image is consumed by the OpenShift pipelines, and used to publish a DACPAC to a database based on a publish profile.

There is a github action in this repository which will build the image and push it to the github container registry. This action can be triggered manually.