#!/bin/sh -l

# Install git
apt-get update
apt-get install -y git

# update rsa
mkdir /root/.ssh
chmod 700 /root/.ssh
eval $(ssh-agent -s)
echo "${ACITOOLS_SSH}" > /root/.ssh/id_rsa
chmod 600 /root/.ssh/id_rsa
ssh-add /root/.ssh/id_rsa
ssh-keyscan github.com >> /root/.ssh/known_hosts
ssh -Tv git@github.com

# try submodules
git status
git submodule update --init