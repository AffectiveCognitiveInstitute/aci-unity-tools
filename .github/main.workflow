workflow "Push => GitHub Pages" {
  on = "push"
  resolves = ["Publish to GitHub Pages"]
}

action "DocFX" {
  uses = "./Actions/docfx"
}

action "Publish to GitHub Pages" {
  uses = "./Actions/publish-gh-pages"
  needs = ["DocFX"]
  secrets = ["GITHUB_TOKEN"]
  env = {
    CONTENT = "docs/_site"
    GH_EMAIL = "moritz.umfahrer@hs-offenburg.de"
    GH_USER = "umfahrem"
    GH_PAGES_BRANCH = "gh-pages"
    GH_REPO = "git@github.com:AffectiveCognitiveInstitute/aci-unity-tools-development.git"
  }
}
