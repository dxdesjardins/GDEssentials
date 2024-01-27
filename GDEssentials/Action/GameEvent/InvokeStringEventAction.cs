using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class InvokeStringEventAction : GameAction
{
    [Export] StringEvent stringEvent;
    [Export] string param;

    public override bool Invoke(Node node) {
        stringEvent.Invoke(param);
        return true;
    }
}
