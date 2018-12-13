FROM debian:9.5-slim

LABEL "com.github.actions.name"="Publish to GitHub Pages"
LABEL "com.github.actions.description"="Moves generated documentation content to a GitHub Pages branch (gh-pages). Must exist beforehand."
LABEL "com.github.actions.icon"="upload-cloud"
LABEL "com.github.actions.color"="blue"

LABEL "repository"="https://github.com/AffectiveCognitiveInstitute/aci-unity-tools-development"
LABEL "maintainer"="Moritz Umfahrer <moritz.umfahrer@hs-ofenburg.de>"

ADD entrypoint.sh /entrypoint.sh
RUN chmod +x entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]