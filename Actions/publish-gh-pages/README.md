# Publish to GitHub Pages
Publishes static content to a gh-pages branch.

```
action "Publish to gh-pages" {
    secrets = ["GITHUB_TOKEN"],    
    env = {
        CONTENT = "docs/_site/"
        GH_EMAIL = "moritz.umfahrer@hs-offenburg.de"
        GH_USER = "Moritz Umfahrer"
        GH_REPO = Target Repo
    }```
}