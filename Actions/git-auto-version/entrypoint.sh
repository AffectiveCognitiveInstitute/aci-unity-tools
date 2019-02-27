#!/bin/bash

# Install git
apt-get update
apt-get install -y git

# Configure git
git config --global user.email "$GH_EMAIL"
git config --global user.name "$GH_USER"
git remote rm origin
git remote add origin https://x-access-token:${GITHUB_TOKEN}@github.com/${GITHUB_REPOSITORY}.git

# setup local branches if not master
branch=$(git rev-parse --abbrev-ref HEAD)
if [[ ${branch} == master ]]; then
  echo "branch is master, not fetching"
else
  git fetch origin master
  git checkout origin/master -b master
  git checkout ${branch}
fi

# check if a version already exists for head
if [[ $(git tag --points-at HEAD) ]]; then
  echo "Tag already exists for commit ${GITHUB_SHA}"
  exit 0;
fi

# get most recent head in master for version
latest=$(git tag  -l --merged master --sort='-*authordate' | head -n1)

echo ${latest}
# get semantic version
read -a semver_parts <<< ${latest//./ }
major=${semver_parts[0]}
minor=${semver_parts[1]}
patch=${semver_parts[2]}

# get number of changes
count=$(git rev-list HEAD ^${latest} --ancestry-path ${latest} --count)
version=""
message=""
 
# check if nightly branch
case $branch in
   "master")
    	version=${major}.$((minor+1)).0
    	message="Release build version ${version}"
    	;;
   "${NIGHTLY_BRANCH}")
    	version=${major}.${minor}.${patch}-nightly-${count}
    	message="Nightly build version ${version}"
    	;;
   *)
		>&2 echo "unsupported branch type"
    	exit 1
    	;;
esac

# create tag
git tag -a ${version} -m "${message}"

# push tag
git push origin ${version}