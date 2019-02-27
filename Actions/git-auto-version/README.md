# Auto semantic versioning for GitHub Actions
This action creates tags with semantic versioning for the selected commit in GITHUB_WORKSPACE. They have to be in MAJOR.MINOR.PATCH format.
At least one tag in this format has to exist on the master branch in order for this to work. Only master branch and a specified nightly branch are versioned.
```
action "Create version tag" {
    uses = "actions/git-auto-version",
    NIGHTLY_BRANCH = required, the branch for nightly versions
}
```