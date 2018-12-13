#!/bin/sh -l

# Install git
apt-get update
apt-get install -y git

# Configure git
git config --global user.email "$GH_EMAIL"
git config --global user.name "$GH_USER"

# check if we are using a different repository with ssh
if [[  -z "${GH_REPO}" ]]; then
	# update rsa
	mkdir /root/.ssh
	chmod 700 /root/.ssh
	eval $(ssh-agent -s)
	var=${RSA_VAR}
	echo "${!var}" > /root/.ssh/id_rsa
	chmod 600 /root/.ssh/id_rsa
	ssh-add /root/.ssh/id_rsa
	ssh-keyscan github.com >> /root/.ssh/known_hosts

	git clone --depth 1 --brnach gh-pages --single-branch ${GH_REPO} ../docs-repo
else
	git clone --depth 1 --branch gh-pages --single-branch https://x-access-token:${GITHUB_TOKEN}@${GITHUB_REPOSITORY} ../docs-repo
fi


# Publish docs
cd ../docs-repo/
rm -rf *
cd ${GITHUB_WORKSPACE}
cp -a -r ${CONTENT}/. ../docs-repo/

cd ../docs-repo/

git status --short | grep -v "??" | cut -d " " -f 3 | xargs git add
git add *
git status --short
git commit -m "Generated new documentation"
git push -f