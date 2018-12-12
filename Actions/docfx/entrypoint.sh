#!/bin/sh -l

# Install DocFX
apt-get update
apt-get install -y unzip wget

wget https://github.com/dotnet/docfx/releases/download/v2.40.2/docfx.zip
unzip docfx.zip -d _docfx

# Build docs
mono _docfx/docfx.exe docs/docfx.json

# Debug (TODO: remove)
ls docs/_site