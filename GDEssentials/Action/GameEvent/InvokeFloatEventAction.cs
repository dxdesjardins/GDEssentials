using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class InvokeFloatEventAction : GameAction
{
    [Export] FloatEvent floatEvent;
    [Export] float param;

    public override bool Invoke(Node node) {
        floatEvent.Invoke(param);
        return true;
    }
}
