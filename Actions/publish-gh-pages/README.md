# Publish to GitHub Pages
Publishes static content to a gh-pages branch.

```
action "Publish to gh-pages" {
    secrets = ["GITHUB_TOKEN"],    
    env = {
        CONTENT = "docs/_site/"
        GH_EMAIL = "jannik.lassahn@uid.com"
        GH_USER = "JannikLassahn"
        GH_REPO = "https://github.com/AffectiveCognitiveInstitute/kobelu-bot"
    }```
}