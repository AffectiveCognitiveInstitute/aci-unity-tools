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

echo "Trying ~/.ssh/"
for entry in ~/.ssh/*
do
  echo "$entry"
done

echo "Trying /root/.ssh/"
for entry in /root/.ssh/*
do
  echo "$entry"
done

# try to intialize submodules
apt-get install -y git
eval $(ssh-agent -s)
echo "${ACITOOLS_SSH}" > ~/.ssh/id_rsa
chmod 400 ~/.ssh/id_rsa
ssh-add ~/.ssh/id_rsa
ssh-keyscan github.com >> ~/.ssh/known_hosts
git status
git submodule update --init

# check if submodules were checked out
for entry in Assets/*
do
  echo "$entry"
done

# Build docs
mono _docfx/docfx.exe docs/docfx.json