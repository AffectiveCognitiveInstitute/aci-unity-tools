# Setup
## Dependencies
ACI Unity Tools has several dependencies you need to add to your project.

1. Zenject
* IOC Framework, ACI Tools is highly dependent on this.
* Unity Asset Store entry: https://assetstore.unity.com/packages/tools/integration/zenject-dependency-injection-ioc-17758
* Official GitHub repository: https://github.com/svermeulen/Zenject
* see Zenject documentation how to setup Zenject for your project

2. Json.NET
* extensive C# JSON framework
* https://www.newtonsoft.com/json
* just copy the binary into your project folder

3. Async Await Support
* Enables the usage of async/await for unity coroutines
* Unity Asset Store entry: https://assetstore.unity.com/packages/tools/integration/async-await-support-101056
* Official GitHub repository: https://github.com/svermeulen/Unity3dAsyncAwaitUtil
* There is no special setup needed, just install the .unitypackage

## Project Setup
Incorporating the ACI Unity Tools into your project is fairly simple. Just copy the repository contents into your project. If you installed the dependencies you should be good to go.

Internally we use git submodules to add the tools to our projecets. It's a fairly easy process and eliminates the need to check the same files into multiple repositories.

We currently do not provide a .unitypackage. This might change at a later date, but at the moment there are no plans to do so. If you want to use a unitypackage you will have to build it yourself.