FROM debian:9.5-slim

LABEL "com.github.actions.name"="Publish to GitHub Pages"
LABEL "com.github.actions.description"="Moves generated documentation content to a githubPages branch"
LABEL "com.github.actions.icon"="book"
LABEL "com.github.actions.color"="green"

LABEL "repository"="https://github.com/AffectiveCognitiveInstitute/aci-unity-tools-development"
LABEL "maintainer"="Moritz Umfahrer <moritz.umfahrer@hs-ofenburg.de>"

ADD entrypoint.sh /entrypoint.sh
RUN chmod +x entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]