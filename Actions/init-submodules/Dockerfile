FROM debian:9.5-slim

LABEL "com.github.actions.name"="Initialize submodules"
LABEL "com.github.actions.description"="Initializes submodules in the repository"
LABEL "com.github.actions.icon"="download-cloud"
LABEL "com.github.actions.color"="blue"

LABEL "repository"="https://github.com/AffectiveCognitiveInstitute/aci-unity-tools-development"
LABEL "maintainer"="Moritz Umfahrer <moritz.umfahrer@hs-ofenburg.de>"

ENV TZ=Europe/Berlin

ADD entrypoint.sh /entrypoint.sh
RUN chmod +x entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]
