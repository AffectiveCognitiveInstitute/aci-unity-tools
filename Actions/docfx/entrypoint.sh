#!/bin/sh -l

ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Install dependencies
apt-get update \
	&& apt-get install -y gnupg gnupg2 gnupg1

# Install MONO
apt-get install -y apt-transport-https dirmngr
apt-key adv --no-tty --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | tee /etc/apt/sources.list.d/mono-official-stable.list \
  && apt-get update \
  && apt-get install -y mono-devel \
&& rm -rf /var/lib/apt/lists/* /tmp/*

# Install DocFX
apt-get update
apt-get install -y unzip wget

wget https://github.com/dotnet/docfx/releases/download/v2.40.4/docfx.zip
unzip docfx.zip -d _docfx

# Build docs
mono _docfx/docfx.exe docs/docfx.json