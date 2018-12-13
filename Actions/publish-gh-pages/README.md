# Publish to GitHub Pages
Publishes static content to a gh-pages branch.

```
action "Publish to GitHub Pages" {
  	uses = "./Actions/publish-gh-pages"
  	needs = ["DocFX"]
  	secrets = ["GITHUB_TOKEN"],    
  	env = {
      	CONTENT = "docs/_site/"
      	GH_EMAIL = "moritz.umfahrer@hs-offenburg.de"
        GH_USER = "Moritz Umfahrer"
    }
}```

or

```
action "Publish to GitHub Pages" {
  	uses = "./Actions/publish-gh-pages"
  	needs = ["DocFX"]
  	secrets = [
  		"GITHUB_TOKEN",
  		"<REPO_TOKEN>"
  	]
  	env = {
    	CONTENT = "docs/_site"
    	GH_EMAIL = "moritz.umfahrer@hs-offenburg.de"
    	GH_USER = "Moritz Umfahrer"
    	GH_REPO = "<target repository ssh address>"
    	RSA_VAR = "<REPO_TOKEN>"
  	}
}```
