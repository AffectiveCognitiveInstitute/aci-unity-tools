# Auto-Creation of UnityPackageManager branch commit and tag for GitHub Actions
This action creates a new commit and tag on a separate upm branch for use with unity package manager. The versioning is done via semantic versioning in MAJOR.MINOR.PATCH format. Custom versions can be set by modifying the package.json version manually before committing on the active branch.
An orphan upm branch has to be created beforehand containing the initial version in the package.json. The target UPM folder in the git repository is defined via the environment variable UPM_FOLDER.
```
action "Create version tag" {
    uses = "actions/git-auto-version",
    UPM_FOLDER = required, the folder to base the upm release on
}
```