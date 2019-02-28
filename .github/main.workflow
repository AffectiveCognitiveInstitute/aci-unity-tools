workflow "Push => GitHub Pages" {
  on = "push"
  resolves = [
    "Publish to GitHub Pages",
  ]
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
    GH_USER = "Moritz Umfahrer"
  }
}

workflow "Create UPM release" {
  on = "push"
  resolves = ["Git UPM version"]
}

action "Filters for GitHub Actions" {
  uses = "actions/bin/filter@712ea355b0921dd7aea27d81e247c48d0db24ee4"
  args = "branch master"
}

action "Git UPM version" {
  uses = "./Actions/git-upm-version"
  needs = ["Filters for GitHub Actions"]
  secrets = ["GITHUB_TOKEN"]
  env = {
    UPM_FOLDER = "Assets/aci-unity-tools"
  }
}
