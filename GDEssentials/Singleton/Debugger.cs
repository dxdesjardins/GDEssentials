using Godot;
using System;
using System.Collections.Generic;
using Lambchomp.Essentials;

namespace Lambchomp.IntoTheAbyss;

public partial class Debugger : Singleton<Debugger> {

    [Export] public bool Invoke {
        get {
            return false;
        }
        set {
            Debug();
        }
    }

    public void Debug() {
        PoolManager.I.PrintStatus();
    }
}
