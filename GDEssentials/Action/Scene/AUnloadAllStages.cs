using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
[Tool]
public partial class AUnloadAllStages : GameAction
{
    [Export] bool includePersistant = false;

    public override void Invoke(Node node) {
        _ = StageManager.UnloadAllStages(includePersistant);
    }
}
