using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chomp.Essentials;

public partial class StageManager : NodeSingleton<StageManager>
{
    [Export] private string stageDirectory = "res://Scene/Stage";
    [Export] private GameAction[] gameStartActions;
    [Export] private PackedScene[] gameStartStages;
    [Export] private PackedScene defaultTransition;
    private static bool initialized = false;

    public static Dictionary<string, StageData> CachedStageData { get; private set; } = new();
    public static int StageCount { get; private set; }
    public static string LastActiveStage { get; private set; }
    public static bool IsQuittingGame { get; private set; }
    public static Node ActiveStage { get; private set; }
    public static bool IsTransitioning { get; private set; }
    public static List<Node> LoadedStages { get; private set; } = new();
    public static List<Node> TotalStages { get; private set; } = new();
    public static List<string> LoadingStages { get; private set; } = new();
    public static List<Node> UnloadingStages { get; private set; } = new();

    public static Action<Node> TransitionBeforeFadeOut { get; set; } = delegate { };
    public static Action<Node> TransitionAfterFadeOut { get; set; } = delegate { };
    public static Action<Node> TransitionBeforeFadeIn { get; set; } = delegate { };
    public static Action<Node> TransitionAfterFadeIn { get; set; } = delegate { };

    public static Action<Node> ActiveStageUnloading { get; set; } = delegate { };
    public static Action<Node> ActiveStageUnloaded { get; set; } = delegate { };
    public static Action<Node> ActiveStageLoaded { get; set; } = delegate { };
    public static Action<Node> StageUnloading { get; set; } = delegate { };
    public static Action<Node> StageUnloaded { get; set; } = delegate { };
    public static Action<Node> StageLoaded { get; set; } = delegate { };
    public static Action<Node> UnloadingComplete { get; set; } = delegate { };
    public static Action<Node> LoadingComplete { get; set; } = delegate { };

    public static Action<bool> ApplicationFocused { get; set; } = delegate { };
    public static Action<bool> ApplicationPaused { get; set; } = delegate { };
    public static Action ApplicationQuitting { get; set; } = delegate { };

    public struct StageData {
        public string path;
        public string name;
        public PackedScene packedScene;

        public StageData(string path, string name, PackedScene packedScene) {
            this.path = path;
            this.name = name;
            this.packedScene = packedScene;
        }
    }

    public StageManager() => CacheStageData();

    private void CacheStageData() {
        foreach (var packedScene in GDE.GetResourcesInDirectory<PackedScene>(stageDirectory)) {
            string path = packedScene.GetPath();
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            CachedStageData.Add(name, new StageData(path, name, packedScene));
        }
    }

    public override void _Notification(int what) {
        switch (what) {
            case (int)NotificationEnterTree:
                Initialize();
                break;
            case (int)NotificationReady:
                gameStartActions.Invoke(this);
                if (gameStartStages != null) {
                    for (int i = 0; i < gameStartStages.Length; i++)
                        _ = LoadStage(gameStartStages[i]);
                }
                break;
            case (int)NotificationApplicationPaused:
                ApplicationPaused.Invoke(true);
                break;
            case (int)NotificationApplicationResumed:
                ApplicationPaused.Invoke(false);
                break;
            case (int)NotificationApplicationFocusIn:
                ApplicationFocused.Invoke(true);
                break;
            case (int)NotificationApplicationFocusOut:
                ApplicationFocused.Invoke(false);
                break;
            case (int)NotificationWMGoBackRequest:
            case (int)NotificationWMCloseRequest:
                IsQuittingGame = true;
                ApplicationQuitting.Invoke();
                break;
        }
    }

    public void Initialize() {
        if (initialized)
            return;
        initialized = true;
        this.ChildEnteredTree += (child) => {
            if (ActiveStage == null && !child.IsInGroup("Persistant")) {
                ActiveStage = child;
                if (string.IsNullOrEmpty(LastActiveStage))
                    LastActiveStage = child.GetFileName();
                child.TreeExiting += () => {
                    if (ActiveStage == child)
                        ActiveStage = null;
                };
                child.Ready += () => ActiveStageLoaded.Invoke(child);
            }
            TotalStages.Add(child);
            StageCount++;
            child.Ready += () => {
                LoadingStages.Remove(child.GetFileName());
                LoadedStages.Add(child);
                StageLoaded.Invoke(child);
                if (LoadingStages.Count == 0)
                    LoadingComplete.Invoke(child);
            };
            child.TreeExited += () => {
                TotalStages.Remove(child);
                LoadedStages.Remove(child);
                UnloadingStages.Remove(child);
                StageCount--;
                if (ActiveStage == child) {
                    LastActiveStage = child.GetFileName();
                    ActiveStageUnloaded.Invoke(child);
                }
                StageUnloaded.Invoke(child);
                if (StageCount == 0)
                    UnloadingComplete.Invoke(child);
            };
        };
    }

    public static async Task StartTransition(PackedScene packedScene) {
        if (IsTransitioning) {
            GDE.LogErr("Failed to start stage transition. A transition is already in process.");
            return;
        }
        IsTransitioning = true;
#if GDPOOL
        PoolManager.Spawn(packedScene, Instance.GetTree().Root);
#else
        Instance.GetTree().Root.InstantiateChild(packedScene);
#endif
        TaskCompletionSource<Node> tcs = new();
        void action(Node node) {
            tcs.TrySetResult(node);
            TransitionAfterFadeIn -= action;
        }
        TransitionAfterFadeIn += action;
        await tcs.Task;
        IsTransitioning = false;
    }

    public static async Task StartTransition() => await StartTransition(Instance.defaultTransition);

    public static async Task<Node> UnloadStage(Node stage) {
        if (IsTransitioning) {
            TaskCompletionSource<Node> fadeOutTcs = new();
            void fadeOutAction(Node node) {
                fadeOutTcs.TrySetResult(node);
                TransitionAfterFadeOut -= fadeOutAction;
            }
            TransitionAfterFadeIn += fadeOutAction;
            await fadeOutTcs.Task;
        }
        UnloadingStages.Add(stage);
        if (ActiveStage == stage)
            ActiveStageUnloading.Invoke(stage);
        StageUnloading.Invoke(stage);
        stage.QueueFree();
        TaskCompletionSource<Node> tcs = new();
        void action(Node node) {
            if (node == stage) {
                tcs.TrySetResult(node);
                StageUnloaded -= action;
            }
        }
        StageUnloaded += action;
        return await tcs.Task;
    }

    public static async Task<Node> UnloadAllStages(bool includePersistant = false) {
        for (int i = LoadedStages.Count - 1; i >= 0; i--)
            if (includePersistant || !LoadedStages[i].IsInGroup("Persistant"))
                _ = UnloadStage(LoadedStages[i]);
        TaskCompletionSource<Node> tcs = new();
        void action(Node node) {
            tcs.TrySetResult(node);
            UnloadingComplete -= action;
        }
        UnloadingComplete += action;
        return await tcs.Task;
    }

    public static async Task<Node> LoadStage(PackedScene packedScene, bool awaitUnloadingCompletion = true) {
        LoadingStages.Add(packedScene.GetFileName());
        if (awaitUnloadingCompletion && UnloadingStages.Count > 0) {
            TaskCompletionSource tcs = new();
            void action(Node _) {
                tcs.TrySetResult();
                UnloadingComplete -= action;
            }
            UnloadingComplete += action;
            await tcs.Task;
        }
        return Instance.InstantiateChild(packedScene, false);
    }

    public static async Task<Node> LoadStage(string stageName, bool awaitUnloadingCompletion = true) {
        var stageData = CachedStageData[stageName];
        return await LoadStage(stageData.packedScene, awaitUnloadingCompletion);
    }

    public static bool IsStageUnloading(Node stage) {
        return UnloadingStages.Contains(stage);
    }

    public static void SetActiveStage(Node stage) {
        if (ActiveStage == stage)
            return;
        ActiveStage = stage;
        stage.TreeExiting += () => {
            if (ActiveStage == stage)
                ActiveStage = null;
        };
        ActiveStageLoaded.Invoke(stage);
    }

    public static void SetActiveStage(string stageName) {
        foreach (var scene in LoadedStages) {
            if (scene.Name == stageName) {
                SetActiveStage(scene);
                return;
            }
        }
    }

    public static Node GetLoadedStage(string stageName) {
        foreach (var scene in LoadedStages) {
            if (scene.Name == stageName)
                return scene;
        }
        return null;
    }

    public static StageData GetStageData(string stageName) {
        if (CachedStageData.TryGetValue(stageName, out StageData value))
            return value;
        return default;
    }

    public static bool StageExists(string stageName) {
        return CachedStageData.ContainsKey(stageName);
    }
}
