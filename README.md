GDEssentials
=================
A collection of tools and extension methods for the Godot Game Engine. Many features attempt to reimplement functionality and popular repositories from Unity to Godot. The original purpose of this project was to design a system to replace the use of Godot Signals in the Editor Interface.

Features
----
1. A collection of extension methods implementing functionality inspired by Unity.

2. Generic singleton class implementations for Nodes and Resources.

3. An object pooling system. See the GDPool repository for documentation.

4. A StageManager class implementing functionality of Unity's SceneManager. I define a stage as a highest level scene, such as a GameLevel or PersistentScene.

5. A system for handling of GameEvents, GameActions, References and Variables notably allowing them to be used as Resources that can be dragged into subscribing Nodes. Various listener scripts working with the system are included. The system is designed to replace the need to use signal connections through the editor interface and eliminate reliance on breakable string references.

Known Issues
----
This project may experience crashes due to an engine bug related to C# generic class compilation. It should fixed for the upcoming 4.3 release; I am currently using an unofficial engine patch to mitigate. Reference the issue report at:
https://github.com/godotengine/godot/issues/79519

Licence
---
MIT
