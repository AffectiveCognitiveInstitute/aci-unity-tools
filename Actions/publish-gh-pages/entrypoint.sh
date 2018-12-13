#!/bin/sh -l

# Install git
apt-get update
apt-get install -y git

# Configure git
git config --global user.email "$GH_EMAIL"
git config --global user.name "$GH_USER"

# check if we are using a different repository with ssh
if [ -z "${GH_REPO}" ]; then
	git clone --depth 1 --branch gh-pages --single-branch https://x-access-token:${GITHUB_TOKEN}@github.com/${GITHUB_REPOSITORY}.git ../docs-repo
else
	# update rsa
	mkdir /root/.ssh
	chmod 700 /root/.ssh
	eval $(ssh-agent -s)
	var=${RSA_VAR}
	eval 'echo "${'"$var"'}"' > /root/.ssh/id_rsa
	chmod 600 /root/.ssh/id_rsa
	ssh-add /root/.ssh/id_rsa
	ssh-keyscan github.com >> /root/.ssh/known_hosts

	git clone --depth 1 --branch gh-pages --single-branch ${GH_REPO} ../docs-repo
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
