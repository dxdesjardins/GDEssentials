using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public partial class ButtonListener : Node
{
    [Export] private Button target;
    [Export] private GameAction[] buttonDownActions;
    [Export] private GameAction[] buttonUpActions;
    [Export] private GameAction[] buttonPressedActions;
    [Export] private GameAction[] buttonToggledActionsBool;

    public override void _EnterTree() {
        RequestReady();
    }

    public override void _ExitTree() {
        target.ButtonDown -= InvokeButtonDownActions;
        target.ButtonUp -= InvokeButtonUpActions;
        target.Pressed -= InvokeButtonPressedActions;
        target.Toggled -= InvokeButtonToggledActionsBool;
    }

    public override void _Ready() {
        target ??= this.GetParent<Button>();
        target.ButtonDown += InvokeButtonDownActions;
        target.ButtonUp += InvokeButtonUpActions;
        target.Pressed += InvokeButtonPressedActions;
        target.Toggled += InvokeButtonToggledActionsBool;
    }

    public void InvokeButtonDownActions() {
        buttonDownActions.Invoke(this);
    }

    public void InvokeButtonUpActions() {
        buttonUpActions.Invoke(this);
    }

    public void InvokeButtonPressedActions() {
        buttonPressedActions.Invoke(this);
    }

    public void InvokeButtonToggledActionsBool(bool state) {
        buttonToggledActionsBool.Invoke<bool>(state, this);
    }
}
