FROM debian:9.5-slim

LABEL "com.github.actions.name"="DocFX"
LABEL "com.github.actions.description"="Build and publish documentation"
LABEL "com.github.actions.icon"="book"
LABEL "com.github.actions.color"="green"

LABEL "repository"="https://github.com/AffectiveCognitiveInstitute/aci-unity-tools-development"
LABEL "maintainer"="Moritz Umfahrer <moritz.umfahrer@hs-ofenburg.de>"

# Install dependencies
RUN apt-get update \
  && apt-get install -y gnupg gnupg2 gnupg1

# Install MONO
ENV MONO_VERSION 5.16.0.179

RUN apt-key adv --no-tty --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian stable-jessie/snapshots/$MONO_VERSION main" > /etc/apt/sources.list.d/mono-official-stable.list \
  && apt-get update \
  && apt-get install -y mono-runtime \
&& rm -rf /var/lib/apt/lists/* /tmp/*

RUN apt-get update \
  && apt-get install -y binutils curl mono-devel ca-certificates-mono fsharp mono-vbnc nuget referenceassemblies-pcl \
&& rm -rf /var/lib/apt/lists/* /tmp/*

ADD entrypoint.sh /entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]
