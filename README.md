GDEssentials
=================
Custom event system and collection of tools for the Godot Game Engine. Many features attempt to reimplement functionality and popular repositories from Unity to Godot. This project is designed to allow architecture of highly modular and independant entities, and to completely replace the need to use signal connections through the editor interface (eliminating reliance on breakable string references).

Core Features
----
1. A custom event and referencing system for creation and handling of serializable GameEvents, GameActions, Node/Resource References, and Variables. Various listener scripts interacting with this system are included. This is useful for creating highly modular scene structures that do not rely on editor interface signals to communicate.
2. Generic singleton implementations for Nodes and Resources.
3. A collection of extension methods and utilities.
4. A StageManager singleton. I define a stage as a highest level scene, such as a GameLevel or PersistentDataStage.

Requirements
----
1. Godot .NET v4.3+
2. .NET 8+

Setup
----
If using my StageManager (required by my GDPool/GDSave repositories and some extensions):
1. All Stages in the project must have a root node inheriting IStage. For a 2D game, a level/world would have a root node of type Stage2D.
2. Set a single StageManager Node as your main scene. Configure this node with the path to the directory containing your stages and the PackedScenes of the stages you want to load on game start.
