#!/bin/sh -l

# Install git
apt-get update
apt-get install -y git

# Configure git
git config --global user.email "$GH_EMAIL"
git config --global user.name "$GH_USER"

git clone ${GH_REPO} --depth 1 --branch ${GH_PAGES_BRANCH} --single-branch ../docs-repo

# Publish docs
cp -a -r ${CONTENT} ../docs-repo/
git add *
git commit -m "Update docs"
git push -f