workflow "Push => GitHub Pages" {
  on = "push"
  resolves = ["Publish to GitHub Pages"]
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
