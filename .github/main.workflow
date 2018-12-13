workflow "Push => GitHub Pages" {
  on = "push"
  resolves = [
    "Publish to GitHub Pages",
    "Publish to GitHub Pages-1",
  ]
}

action "Initialize submodules" {
  uses = "./Actions/init-submodules"
  secrets = ["ACITOOLS_SSH"]
}

action "DocFX" {
  uses = "./Actions/docfx"
  needs = ["Initialize submodules"]
}

action "Publish to GitHub Pages" {
  uses = "./Actions/publish-gh-pages"
  needs = ["DocFX"]
  secrets = ["GITHUB_TOKEN"]
  env = {
    CONTENT = "docs/_site"
    GH_EMAIL = "moritz.umfahrer@hs-offenburg.de"
    GH_USER = "Moritz Umfahrer"
  }
}

action "Publish to GitHub Pages-1" {
  uses = "./Actions/publish-gh-pages"
  needs = ["DocFX"]
  secrets = ["ACITOOLS_WRITE"]
  env = {
    CONTENT = "docs/_site"
    GH_EMAIL = "moritz.umfahrer@hs-offenburg.de"
    GH_USER = "Moritz Umfahrer"
    GH_REPO = "git@github.com:AffectiveCognitiveInstitute/aci-unity-tools.git"
    RSA_VAR = "ACITOOLS_WRITE"
  }
}