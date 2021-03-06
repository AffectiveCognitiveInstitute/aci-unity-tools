#!/bin/bash

# Install git
apt-get update
apt-get install -y git \
jq \
moreutils

# Configure git
git config --global user.email "$GH_EMAIL"
git config --global user.name "$GH_USER"
git remote rm origin
git remote add origin https://x-access-token:${GITHUB_TOKEN}@github.com/${GITHUB_REPOSITORY}.git
git remote update
git fetch

# copy upm files to temporary location
mkdir -p /temp
cp -a ${UPM_FOLDER}/. /temp
rm -r /temp/.git

# checkout & clean upm
git checkout origin/upm -b upm
git rm -rf .

# add changed files
cp -a /temp/. .
git add . --force

# get most recent tag
latest=$(git tag -l --sort='-*authordate' | head -n1)

echo ${latest}
# get semantic version
read -a semver_parts <<< ${latest//./ }
major=${semver_parts[0]}
minor=${semver_parts[1]}
patch=${semver_parts[2]}

# get version number from package.json
storedVersion=$(jq '.version' package.json | sed "s/\"//g")
echo ${storedVersion}
# get semantic version
read -a semver_parts2 <<< ${storedVersion//./ }
storedMajor=${semver_parts2[0]}
storedMinor=${semver_parts2[1]}
storedPatch=${semver_parts2[2]}

# set version number
version=""
if [[ "$storedMajor" -gt "$major" ]] || [[ "$storedMinor" -gt "$minor" ]]  ; then
  version=$storedVersion
else
  version=${major}.$((minor)).$((patch+1))
fi

message="Automated release for UnityPackageManager with version ${version}"

# update version number in package.json
jq ".version = \"${version}\"" package.json|sponge package.json

# commit
git commit -a -m "${message}"

# push branch
git push origin upm

# create tag
git tag -a ${version} -m "${message}"

# push tag
git push origin ${version}