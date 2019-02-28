FROM debian:9.5-slim

LABEL "com.github.actions.name"="Git UPM version"
LABEL "com.github.actions.description"="Creates a commit in an separate upm branch based on the current branch and creates a semantic versioning tag."
LABEL "com.github.actions.icon"="git-commit"
LABEL "com.github.actions.color"="blue"

LABEL "repository"="https://github.com/AffectiveCognitiveInstitute/aci-unity-tools-development"
LABEL "maintainer"="Moritz Umfahrer <moritz.umfahrer@hs-ofenburg.de>"

ENV TZ=Europe/Berlin

ADD entrypoint.sh /entrypoint.sh
RUN chmod +x entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]
