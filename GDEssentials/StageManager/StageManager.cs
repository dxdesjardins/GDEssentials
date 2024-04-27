using Godot;
using System;
using System.Collections.Generic;
using Lambchomp.Essentials;
using System.IO;

namespace Lambchomp.Essentials;

public partial class StageManager : Singleton<StageManager>
{
	[Export] private string[] stageDirectories = new string[2] {"Scene/Stage/Core/", "Scene/Stage/Location/" };
    private Dictionary<string, StageData> stageLookup = new();
    public Dictionary<string, StageData> StageLookup => stageLookup;
	private int stageCount = 0;
	public int StageCount => stageCount;
	private  List<Node> loadedStages = new();
	public List<Node> LoadedStages => loadedStages;
    public List<Node> totalStages = new();
    public List<Node> TotalStages => totalStages;
	private Node activeStage;
	public Node ActiveStage => activeStage;
	public Action<Node> ActiveStageChanged = delegate { };
    public Action<Node> StageLoaded = delegate { };
    public Action<Node> StageUnloaded = delegate { };
    public Action<Node> StageUnloading = delegate { };
    public List<Node> unloadingStages = new();

    public struct StageData {
		public StageData(string ScenePath, string SceneName, PackedScene PackedScene) {
			this.Path = ScenePath;
			this.Name = SceneName;
			this.PackedScene = PackedScene;
		}
        public string Path;
        public string Name;
        public PackedScene PackedScene;
    }

	public override void _EnterTree() {
        foreach (var directory in stageDirectories) {
            foreach (var filePath in Directory.GetFiles(directory, "*.tscn")) {
                string name = Path.GetFileNameWithoutExtension(filePath);
                var path = filePath;
                var packedScene = GD.Load<PackedScene>(path);
                stageLookup.Add(name, new StageData(path, name, packedScene));
            }
        }
        this.ChildEnteredTree += (child) => {
            if (activeStage == null && !child.Name.ToString().StartsWith("Persistant")) {
                activeStage = child;
                ActiveStageChanged.Invoke(child);
                child.TreeExiting += () => {
                    if (activeStage == child)
                        activeStage = null;
                };
            }
            totalStages.Add(child);
            stageCount++;
            child.Ready += () => {
                loadedStages.Add(child);
                StageLoaded.Invoke(child);
            };
            child.TreeExited += () => {
                totalStages.Remove(child);
                loadedStages.Remove(child);
                stageCount--;
                StageUnloaded.Invoke(child);
            };
        };
    }

    public void UnloadStage(Node scene) {
        unloadingStages.Add(scene);
        scene.TreeExited += () => unloadingStages.Remove(scene);
        StageUnloading.Invoke(scene);
        scene.QueueFree();
    }

    public bool IsStageUnloading(Node scene) {
        return unloadingStages.Contains(scene);
    }

	public void SetActiveStage(Node scene) {
        if (activeStage == scene)
            return;
        activeStage = scene;
		scene.TreeExiting += () => {
			if (activeStage == scene)
				activeStage = null;
		};
        ActiveStageChanged.Invoke(scene);
    }

    public void SetActiveStage(string sceneName) {
		foreach (var scene in loadedStages) {
			if (scene.Name == sceneName) {
                SetActiveStage(scene);
                return;
            }
		}
    }

    public Node GetLoadedStage(string sceneName) {
        foreach (var scene in loadedStages) {
            if (scene.Name == sceneName)
                return scene;
        }
        return null;
    }
}

