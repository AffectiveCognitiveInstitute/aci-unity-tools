workflow "Push => GitHub Pages" {
  on = "push"
  resolves = [
    "Initialize submodules",
  ]
}

action "Initialize submodules" {
  uses = "./Actions/init-submodules"
  secrets = ["ACITOOLS_SSH"]
}
