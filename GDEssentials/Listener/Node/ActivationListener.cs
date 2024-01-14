using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public partial class ActivationListener : Node
{
    [Export] private GameAction[] activateActions;
    [Export] private GameAction[] deactivateActions;

    public override void _EnterTree() {
        RequestReady();
    }

    public override void _ExitTree() {
        deactivateActions.InvokeGameActions(this);
    }

    public override void _Ready() {
        activateActions.InvokeGameActions(this);
    }
}
