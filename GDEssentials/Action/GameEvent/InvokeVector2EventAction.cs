using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class InvokeVector2EventAction : GameAction
{
    [Export] Vector2Event vector2Event;
    [Export] Vector2 param;

    public override bool Invoke(Node node) {
        vector2Event.Invoke(param);
        return true;
    }
}
