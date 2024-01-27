using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class InvokeVector3EventAction : GameAction
{
    [Export] Vector3Event vector3Event;
    [Export] Vector3 param;

    public override bool Invoke(Node node) {
        vector3Event.Invoke(param);
        return true;
    }
}
