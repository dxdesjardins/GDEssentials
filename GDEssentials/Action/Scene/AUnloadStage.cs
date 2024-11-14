using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
[Tool]
public partial class AUnloadStage : ParamAction<PackedScene>
{
    [Export] private PackedScene stagePacked;

    public override void Invoke(PackedScene stage, Node node) {
        Node _target;
        if (stagePacked == null)
            _target = node.GetStage();
        else
            _target = StageManager.GetLoadedStage(stage.ResourceName);
        _ = StageManager.UnloadStage(_target);
    }

    public override void Invoke(Node node) => Invoke(stagePacked, node);
}
