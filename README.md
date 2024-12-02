GDEssentials
=================
Custom event system, referencing system, and collection of tools for the Godot Game Engine. Many features reimplement functionality and popular repositories from Unity to Godot. This repository is designed to allow architecture of highly modular and independant entities, and to completely eliminate the need to use signal connections through the editor interface (reducing reliance on breakable string references).

Core Features
----
1. A custom event and referencing system for creation and handling of serializable GameEvents, GameActions, Node/Resource References, and Variables. Various listener scripts interacting with this system are included. This is useful for architecting highly modular and independant scenes that do not rely on editor interface signals to communicate.
2. Generic singleton and reference implementations for Nodes and Resources.
3. A collection of extension methods and utilities.
4. Custom StageManager and PreferenceManager singletons. I define a stage as a highest level scene, such as a GameLevel or PersistentDataStage.

Requirements
----
1. Godot .NET v4.3+
2. .NET 8+

Setup
----
If using my StageManager (required by my GDPool/GDSave repositories and some extensions):
1. All Stages in the project must have a root node inheriting IStage. For a 2D game, a level/world would have a root node of type Stage2D.
2. Create a scene with a single StageManager node. Configure the StageManager with the path to the directory containing your stages. You can either:
   1. Set the StageManager scene as an autoload. All spawned stages will become children of the tree root.Set the StageManager scene as your main scene.
   2. All spawned stages will become children of the StageManager. This is ideal for 2D games which need to enable `Y Sort`.

Usage Examples
----
```csharp
// Declaring a Node singleton.
public partial class ExampleSingleton : NodeSingleton<ExampleSingleton> {
    void ExampleMethod() { }
}

// Getting the Node singleton instance in code.
// If the singleton did not exist, it will automatically be created and added to the tree by default.
ExampleSingleton.Instance.ExampleMethod();

// Declaring a PackedScene resource reference using its UID.
[Tool] [ResourceUid("uid://cnnqtg7jofccw")]
public partial class PlayerPackedSceneReference : ResourceReference<PlayerPackedSceneReference, PackedScene> { }

// Declaring a PackedScene resource reference using its path.
[Tool] [ResourcePath("res://Scene/Entities/Character/Player.tscn")]
public partial class PlayerPackedSceneReference : ResourceReference<PlayerPackedSceneReference, PackedScene> { }

// Declaring a resource reference.
[GlobalClass] [Tool] [ResourceUid("uid://c07plqdm81bke")]
public partial class GameSettings : ResourceReference<GameSettings, GameSettings> {
    bool exampleSetting;
}

// Getting the resource reference instance in code. If the resource is not found, it will automatically be created.
PackedScene playerPackedScene = PlayerPackedSceneReference.Instance;
GameSettings gamesettings = GameSettings.Instance;

```
