using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

public partial class TimerListener : Node
{
    [Export] private Timer target;
    [Export] private GameAction[] timeoutAtions;

    public override void _EnterTree() {
        RequestReady();
    }

    public override void _ExitTree() {
        target.Timeout -= InvokeTimeoutActions;
    }

    public override void _Ready() {
        target ??= this.GetParent<Timer>();
        target.Timeout += InvokeTimeoutActions;
    }

    public void InvokeTimeoutActions() {
        timeoutAtions.Invoke(this);
    }
}
