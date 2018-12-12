FROM debian:9.5-slim

LABEL "com.github.actions.name"="DocFX"
LABEL "com.github.actions.description"="Build and publish documentation"
LABEL "com.github.actions.icon"="book"
LABEL "com.github.actions.color"="green"

LABEL "repository"="https://github.com/AffectiveCognitiveInstitute/aci-unity-tools-development"
LABEL "maintainer"="Moritz Umfahrer <moritz.umfahrer@hs-ofenburg.de>"

ENV TZ=Europe/Berlin

ADD entrypoint.sh /entrypoint.sh
RUN chmod +x entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]
