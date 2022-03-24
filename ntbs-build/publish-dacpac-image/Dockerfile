FROM ubuntu:latest AS build

RUN apt-get update && apt-get install -y \
    curl \
    unzip \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /root
RUN curl -L https://go.microsoft.com/fwlink/?linkid=2157202 --output sqlpackage.zip
RUN mkdir sqlpackage
RUN unzip sqlpackage.zip -d sqlpackage
RUN chmod a+x ~/sqlpackage/sqlpackage

FROM ubuntu:latest
COPY --from=build /root/sqlpackage /root/sqlpackage
RUN DEBIAN_FRONTEND=noninteractive apt-get update && apt-get install -y --no-install-recommends \
    libunwind8 \
    libicu66 \
    && rm -rf /var/lib/apt/lists/*
