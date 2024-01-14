using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class InvokeBoolEventAction : GameAction
{
    [Export] BoolEvent boolEvent;
    [Export] bool state;

    public override bool Invoke(Node node) {
        boolEvent.Invoke(state);
        return true;
    }
}
