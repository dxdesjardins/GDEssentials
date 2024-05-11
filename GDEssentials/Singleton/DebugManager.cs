using Godot;
using System;
using System.Collections.Generic;
using Chomp.Essentials;

namespace Chomp.IntoTheAbyss;

public partial class DebugManager : Singleton<DebugManager> {

    [Export] public bool PrintObjectPoolStatus {
        get { return false; }
        set {
            PoolManager.PrintStatus();
        }
    }

    [Export] public bool ToggleTimeScale {
        get { return false; }
        set {
            if (Engine.TimeScale == 0)
                Engine.TimeScale = 1;
            else
                Engine.TimeScale = 0;
        }
    }
}
