# init-submodules for GitHub Actions
This action tries to initialize all submodules within the repository.
```
action "Initialize submodules" {
    uses = "actions/init-submodules",
    RSA_PRIVATE_KEY = private rsa key for access to private repositories
}
```