#!/bin/sh -l

# try to intialize submodules
apt-get install -y git
mkdir ~/.ssh
chmod 700 ~/.ssh
eval $(ssh-agent -s)
echo "${ACITOOLS_SSH}" > ~/.ssh/id_rsa
chmod 600 ~/.ssh/id_rsa
ssh-add ~/.ssh/id_rsa
ssh-keyscan github.com >> ~/.ssh/known_hosts
echo -e "Host github.com\n\tStrictHostKeyChecking no\n" >> ~/.ssh/config
ssh -Tv git@github.com
git status
git submodule update --init