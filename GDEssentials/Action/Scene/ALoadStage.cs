using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

// TODO: There is an engine limitation where resources cannot reference each other without causing recursion.
// UID string use is a temporary workaround. If the below proposal is ever closed, this should be rewritten.
// https://github.com/godotengine/godot-proposals/issues/7363

[GlobalClass]
[Tool]
public partial class ALoadStage : ParamAction<PackedScene>
{
    [Export] private bool awaitUnloadingCompletion = true;
    [Export] private PackedScene stage;
    [Export] private string stageName;
    [Export] private string StageUID {
        get => stageUID;
        set {
            if (!string.IsNullOrEmpty(value)) {
                stageName = System.IO.Path.GetFileName(GDE.UidToPath(value));
                stageUID = value;
            }
            else {
                stageName = "";
                stageUID = "";
            }
        }
    }
    private string stageUID;

    public override void Invoke(PackedScene stage, Node node) {
        if (!string.IsNullOrEmpty(stageUID))
            stage = GDE.LoadFromUid<PackedScene>(stageUID);
        else if (this.stage != null)
            stage = this.stage;
        _ = StageManager.LoadStage(stage, awaitUnloadingCompletion);
    }

    public override void Invoke(Node node) => Invoke(stage, node);
}
