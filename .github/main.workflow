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
