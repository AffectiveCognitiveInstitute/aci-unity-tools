FROM debian:9.5-slim

LABEL "com.github.actions.name"="Publish to GitHub Pages"
LABEL "com.github.actions.description"="Publish static content to a GitHub Pages branch"
LABEL "com.github.actions.icon"="book"
LABEL "com.github.actions.color"="green"

LABEL "repository"="https://github.com/AffectiveCognitiveInstitute/kobelu-bot"
LABEL "maintainer"="Jannik <jannik.lassahn@uid.com>"

ADD entrypoint.sh /entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]