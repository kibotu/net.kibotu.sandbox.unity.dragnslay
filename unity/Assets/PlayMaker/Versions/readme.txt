This folder contains different versions of the Playmaker DLLs.
Double click the unitypackage to import and replace the current version.

PlaymakerDefault

- The default DLLs. Import this to revert to the original install.

PlaymakerNACL

- Playmaker version for NaCL builds. 
- Switch Platform to NaCL then import this package.
- NOTE: Networking actions are not supported in NaCL.
- Actions respect the UNITY_NACL flag introduced in Unity 4.
- In earlier versions you will have to manually delete unsupported actions (e.g., PlayMaker/Actions/Network). 

PlaymakerPreview 

- A preview only version of Playmaker. Editing is disabled.
- Useful if you want to share a Playmaker project with someone who doesn't own Playmaker.
- NOTE: Please do not include the Versions folder if distributing a Preview Project!
