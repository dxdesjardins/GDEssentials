using Godot;
using System;
using System.Collections.Generic;
using Chomp.Essentials;
using System.IO;

namespace Chomp.Essentials;

public partial class StageManager : Singleton<StageManager>
{
    [Export] private string[] stageDirectories = new string[2] {"Scene/Stage/Core/", "Scene/Stage/Location/" };
    [Export] private GameAction[] gameStartActions;
    private Dictionary<string, StageData> stageLookup = new();
    public static Dictionary<string, StageData> StageLookup => Instance.stageLookup;
    private int stageCount = 0;
    public static int StageCount => Instance.stageCount;
    private List<Node> loadedStages = new();
    public static List<Node> LoadedStages => Instance.loadedStages;
    private List<Node> totalStages = new();
    public static List<Node> TotalStages => Instance.totalStages;
    private Node activeStage;
    public static Node ActiveStage => Instance.activeStage;
    public Action<Node> activeStageChanged = delegate { };
    public static Action<Node> ActiveStageChanged { get { return Instance.activeStageChanged; } set { Instance.activeStageChanged = value; } }
    public Action<Node> stageLoaded = delegate { };
    public static Action<Node> StageLoaded { get { return Instance.stageLoaded; } set { Instance.stageLoaded = value; } }
    public Action<Node> stageUnloaded = delegate { };
    public static Action<Node> StageUnloaded { get { return Instance.stageUnloaded; } set { Instance.stageUnloaded = value; } }
    public Action<Node> stageUnloading = delegate { };
    public static Action<Node> StageUnloading { get { return Instance.stageUnloading; } set { Instance.stageUnloading = value; } }
    private List<Node> unloadingStages = new();

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

    public override void _Ready() {
        gameStartActions.Invoke(this);
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

    public static void UnloadStage(Node scene) {
        Instance.unloadingStages.Add(scene);
        scene.TreeExited += () => Instance.unloadingStages.Remove(scene);
        StageUnloading.Invoke(scene);
        scene.QueueFree();
    }

    public static Node LoadStage(PackedScene packedScene) {
        return Instance.InstantiateChild(packedScene, default, false);
	}

    public static bool IsStageUnloading(Node scene) {
        return Instance.unloadingStages.Contains(scene);
    }

    public static void SetActiveStage(Node scene) {
        if (Instance.activeStage == scene)
            return;
        Instance.activeStage = scene;
        scene.TreeExiting += () => {
            if (Instance.activeStage == scene)
                Instance.activeStage = null;
        };
        ActiveStageChanged.Invoke(scene);
    }

    public static void SetActiveStage(string sceneName) {
        foreach (var scene in Instance.loadedStages) {
            if (scene.Name == sceneName) {
                SetActiveStage(scene);
                return;
            }
        }
    }

    public static Node GetLoadedStage(string sceneName) {
        foreach (var scene in Instance.loadedStages) {
            if (scene.Name == sceneName)
                return scene;
        }
        return null;
    }

    public static StageData GetStageData(string sceneName) {
        if (Instance.stageLookup.ContainsKey(sceneName))
            return Instance.stageLookup[sceneName];
        return default;
    }
}

