FROM buildpack-deps:jessie-scm

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libcurl3 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu52 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        libunwind8 \
        libuuid1 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.0.0-preview1-005977
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-dev-linux-x64.$DOTNET_SDK_VERSION.tar.gz

RUN curl -SL $DOTNET_SDK_DOWNLOAD_URL --output dotnet.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Trigger the population of the local package cache
ENV NUGET_XMLDOC_MODE skip
RUN mkdir warmup \
    && cd warmup \
    && dotnet new \
    && cd .. \
    && rm -rf warmup \
    && rm -rf /tmp/NuGetScratch

RUN mkdir /home/candidate && cd /home/candidate && git clone -b Server https://github.com/gabyzaaf/Candidate-Management.git && apt-get update -y && apt-get install vim -y \
 && mkdir -p /var/candidate/logs/ \
 && mkdir -p /var/candidate/plugins/ \
 && cd /var/candidate/plugins/ \
 && git clone -b pluginEmail https://github.com/gabyzaaf/Candidate-Management.git \
 #&& cd /var/candidate/plugins/ && dotnet restore \
 #&& cp sample.txt bin/Debug/netcoreapp2.0 \
 && touch /var/candidate/logs/plugins/emailPlugins.txt \
 && mkdir -p /var/candidate/emailTemplates/ \
 && apt-get install at -y