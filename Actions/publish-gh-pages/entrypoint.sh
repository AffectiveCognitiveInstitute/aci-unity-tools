#!/bin/sh -l

# Install git
apt-get update
apt-get install -y git

# Configure git
git config --global user.email "$GH_EMAIL"
git config --global user.name "$GH_USER"

git clone --depth 1 --branch ${GH_PAGES_BRANCH} --single-branch https://x-access-token:${GITHUB_TOKEN}@${GH_REPO} ../docs-repo

# Publish docs
cd ../docs-repo/
rm -rf *
cd ${GITHUB_WORKSPACE}
cp -a -r ${CONTENT}/. ../docs-repo/

cd ../docs-repo/

git status --short | grep -v "??" | cut -d " " -f 3 | xargs git add
git add *
git status --short
git commit -m "Generated new documentation for ${GITHUB_SHA}"
git push -f