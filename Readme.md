# ACI Unity Tools
A collection of utilities for development in Unity created by Affective Lab.

## Usage
This can either be added as a git submodule to your Unity project or be added using Unity's Package Manager (2018.3+).

To add this using Unity Package Manager, simply open the manifest.json file in your Unity project under YOUR_PROJECT/Packages/. 
Add the following line to the manifest:


> ```"com.aci.unitytools": "https://github.com/AffectiveCognitiveInstitute/aci-unity-tools.git"``` 

You can also specify a concrete version:
> ```"com.aci.unitytools": "https://github.com/AffectiveCognitiveInstitute/aci-unity-tools.git#1.0.0"``` 

## Required dependencies
Hopefully this can be automated soon, but in the meantime the following must be added manually to Unity:
- Zenject: get it [here](https://assetstore.unity.com/packages/tools/integration/zenject-dependency-injection-ioc-17758)
- Async Await: get it [here](https://www.assetstore.unity3d.com/#!/content/101056)

## Other dependencies
These are dependencies that are already embedded into the project:
- Json.Net
