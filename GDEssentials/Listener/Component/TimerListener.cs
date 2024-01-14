using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public partial class TimerListener : Node
{
    [Export] private Timer target;
    [Export] private GameAction[] timeoutAtions;

    public override void _EnterTree() {
        RequestReady();
    }

    public override void _ExitTree() {
        target.Timeout -= TimeoutActions;
    }

    public override void _Ready() {
        target ??= this.GetParent<Timer>();
        target.Timeout += TimeoutActions;
    }

    public void TimeoutActions() {
        timeoutAtions.InvokeGameActions(this);
    }
}
